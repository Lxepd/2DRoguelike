using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        UpAndLeft,
        UpAndRight,
        DownAndLeft,
        DownAndRight
    }
    public Direction direction;

    public enum Behaviour
    { 
        idle,
        move
    }
    public Behaviour behaviour;

    public float animaNextTime;
    public float animaTime;

    public float moveCheckRange;
    public float speed;
    public bool isCanMove;
    Animator anima;

    Vector3 targetPos;
    Vector3 moreTimeMove;
    float t = 0;
    Direction lastDirection;
    int nextMoveNum;
    int moveKeep;

    private void Start()
    {
        speed *= 0.1f;
        isCanMove = true;
        anima = GetComponent<Animator>();
        nextMoveNum = 0;
        moveKeep = 0;
    }

    private void Update()
    {
        if (animaTime <= 0 && isCanMove)
        {
            SwitchAnim();
            animaTime = animaNextTime;
        }
        else if(animaTime <= -2 && !isCanMove)
        {
            //撞墙，*-1反方向移动
            moreTimeMove *= -1;
            SwitchAnim();
            animaTime = animaNextTime;
        }
        else
            animaTime -= Time.deltaTime;


        if (Vector2.Distance(transform.position, targetPos) == 0)
            isCanMove = true;
    }

    private void FixedUpdate()
    {
        if (!isCanMove)
        {
            t += Time.fixedDeltaTime;
            transform.position = Vector2.Lerp(transform.position, targetPos, t);
        }
        
    }

    public void IsHurt()
    {
        Debug.Log("不要打我啊，好痛啊，呜呜呜/(ㄒoㄒ)/~~");
    }

    void SwitchAnim()
    {
        if (nextMoveNum == 0 || moveKeep ==nextMoveNum)
            behaviour = (Behaviour)Random.Range(0, 2);
        else if(Vector3.Distance(transform.position,targetPos)!=0)
            behaviour = Behaviour.move;

        switch (behaviour)
        {
            case Behaviour.idle:
                break;
            case Behaviour.move:
                //移动
                EnemyMove();
                break;
        }
    }

    public void EnemyMove()
    {
        //检查移动位置是否有墙壁，否则换个方向移动
        //Check::TODO

        //决定移动方向
        if (moveKeep == nextMoveNum)
            while (direction == lastDirection)
                direction = (Direction)Random.Range(0, 8);

        lastDirection = direction;
        //连续移动
        if (moveKeep == nextMoveNum)
        {
            nextMoveNum = Random.Range(1, 5);
            moveKeep = 0;
        }
        //记录移动次数
        moveKeep++;
        //重复移动
        if (moveKeep != 1)
        {
            isCanMove = false;
            targetPos = transform.position;
            targetPos += moreTimeMove;
            anima.SetTrigger("move");
            t = Vector2.Distance(transform.position, targetPos) * Time.deltaTime / speed;

            return;
        }
        //首次移动
        isCanMove = false;  

        targetPos = transform.position;
        anima.SetTrigger("move");

        switch (direction)
        {
            case Direction.Up:
                moreTimeMove = new Vector3(0, speed*2, 0);
                break;
            case Direction.Down:
                moreTimeMove = new Vector3(0, -speed*2, 0);
                break;
            case Direction.Left:
                moreTimeMove = new Vector3(-speed*2, 0, 0);
                break;
            case Direction.Right:
                moreTimeMove = new Vector3(speed*2, 0, 0);
                break;
            case Direction.UpAndLeft:
                moreTimeMove = new Vector3(-speed*2, speed*2, 0);
                break;
            case Direction.UpAndRight:
                moreTimeMove = new Vector3(speed*2, speed*2, 0);
                break;
            case Direction.DownAndLeft:
                moreTimeMove = new Vector3(-speed*2, -speed*2, 0);
                break;
            case Direction.DownAndRight:
                moreTimeMove = new Vector3(speed*2, -speed*2, 0);
                break;
        }

        targetPos += moreTimeMove;
        t = Vector2.Distance(transform.position, targetPos) * Time.deltaTime / speed;
    }

    void CheckMove()
    {
        
    }

}
