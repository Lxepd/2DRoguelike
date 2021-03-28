using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    public int roomindex;

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
        move,
        attack,
        die
    }
    public Behaviour behaviour;

    public bool isDeath;

    //行为间隔
    public float animaNextTime;
    //当前行为时间
    public float animaTime;
    //速度
    float speed;
    //判断是否允许行走
    public bool isCanMove;
    //动画组件
    Animator anima;
    //目标位置
    Vector3 targetPos;
    //位置偏移量
    Vector3 moreTimeMove;
    //每段移动的百分比
    float t = 0;
    //上一次的行为
    Direction lastDirection;
    //该行为的次数（用于怪物散步）
    int nextMoveNum;
    //当前移动次数记录
    int moveKeep;

    public float hp;
    public float damage;
    int index;

    public float attackTime;
    public float attackNextTime;
    bool localScaleTurn;

    public LayerMask playerMask;
    public float attackRange;
    //判断是否允许攻击
    public bool isCanAttack;
    //判断是否已经分裂
    public int isDivision;

    public static EnemyBehaviorController instance;
    string objname;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        speed = 0;
        isCanMove = true;
        isCanAttack = false;
        anima = GetComponent<Animator>();
        nextMoveNum = 0;
        moveKeep = 0;
        isDeath = false;

        for (var i = 0; i < EnemyController.instance.enemyDatas.Length; i++)
            if (name == EnemyController.instance.enemyDatas[i].cEnemyPrefabs.name + "(Clone)")
                index = i;

        hp = EnemyController.instance.enemyDatas[index].cHp;
        damage = EnemyController.instance.enemyDatas[index].cDamage;
        objname = EnemyController.instance.enemyDatas[index].cEnemyPrefabs.name;
    }

    private void Update()
    {
        if (isDeath && EnemyController.instance.enemyDatas[index].cBigEnemy)
        {
            if (isDivision == 1)
            {
                EnemyController.instance.SwitchEnemySkill(EnemyController.instance.enemyDatas[index].cEnemyKind, gameObject, roomindex);
                EnemyController.instance.ReMoveDeathEnemy(roomindex, gameObject);
                isDivision++;
                Destroy(gameObject);
            }
            else
                anima.Play(objname + "Skill");

            return;
        }
        else if (isDeath && !EnemyController.instance.enemyDatas[index].cBigEnemy)
        {
            anima.Play(objname + "Death");
            Debug.LogError(111);
            EnemyController.instance.ReMoveDeathEnemy(roomindex, gameObject);
            enabled = false;
            return;
        }

        if (CheckIsAttack())
        {
            if (attackTime >= attackNextTime)
            {
                isCanAttack = true;
                SwitchAnim();
            }
            attackTime += Time.deltaTime;
        }
        else if (animaTime <= 0 && (isCanMove || !isCanMove))
        {
            SwitchAnim();
            animaTime = animaNextTime;
        }
        else
        {
            animaTime -= Time.deltaTime;
            attackTime = 0;
            isCanAttack = false;
        }

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

    void SwitchAnim()
    {
        if (nextMoveNum == 0 || moveKeep == nextMoveNum)
            behaviour = (Behaviour)Random.Range(0, 2);
        else if (Vector3.Distance(transform.position, targetPos) != 0)
            behaviour = Behaviour.move;

        if (isCanAttack)
            behaviour = Behaviour.attack;

        switch (behaviour)
        {
            case Behaviour.idle:
                break;
            case Behaviour.move:
                MoveTimesDecide();
                TargetDecide();
                break;
            case Behaviour.attack:
                Attack();
                attackTime = 0;
                break;
        }
    }

    public void MoveTimesDecide()
    {
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
            anima.SetTrigger(objname + "Move");
            t = Vector2.Distance(transform.position, targetPos) * Time.deltaTime / speed;

            return;
        }
    }

    void TargetDecide()
    {
        //决定移动方向
        if (moveKeep == nextMoveNum)
        {
            while (direction == lastDirection)
                direction = (Direction)Random.Range(0, 8);

            if ((lastDirection == (Direction)2 || lastDirection == (Direction)4 || lastDirection == (Direction)6)
                   && (direction == (Direction)3 || direction == (Direction)5 || direction == (Direction)7))
                localScaleTurn = true;
            else if ((lastDirection == (Direction)3 || lastDirection == (Direction)5 || lastDirection == (Direction)7)
                  && (direction == (Direction)2 || direction == (Direction)4 || direction == (Direction)6))
                localScaleTurn = true;
            else
                localScaleTurn = false;

        }

        lastDirection = direction;
        //首次移动
        isCanMove = false;
        targetPos = transform.position;
        anima.SetTrigger(objname + "Move");


        switch (direction)
        {
            case Direction.Up:
                moreTimeMove = new Vector3(0, speed * 2, 0);
                break;
            case Direction.Down:
                moreTimeMove = new Vector3(0, -speed * 2, 0);
                break;
            case Direction.Left:
                moreTimeMove = new Vector3(-speed * 2, 0, 0);
                break;
            case Direction.Right:
                moreTimeMove = new Vector3(speed * 2, 0, 0);
                break;
            case Direction.UpAndLeft:
                moreTimeMove = new Vector3(-speed * 2, speed * 2, 0);
                break;
            case Direction.UpAndRight:
                moreTimeMove = new Vector3(speed * 2, speed * 2, 0);
                break;
            case Direction.DownAndLeft:
                moreTimeMove = new Vector3(-speed * 2, -speed * 2, 0);
                break;
            case Direction.DownAndRight:
                moreTimeMove = new Vector3(speed * 2, -speed * 2, 0);
                break;
        }

        targetPos += moreTimeMove;
        t = Vector2.Distance(transform.position, targetPos) * Time.deltaTime / speed;

        if (localScaleTurn)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    bool CheckIsAttack()
    {
        return Physics2D.OverlapCircle(transform.position, attackRange, playerMask);
    }

    void Attack()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);
        //玩家在怪物左边
        //if (col.transform.position.y - transform.position.y < 0)
        //    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        anima.SetTrigger(objname + "Attack");

        Player.instance.isHurt = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
            moreTimeMove *= -1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    
    //关键帧事件
    void SetMoveSpeed(float setSpeed)
    {
        speed = setSpeed * 0.1f;
    }
    void SetDivision(int div)
    {
        isDivision = div;
    }
}
