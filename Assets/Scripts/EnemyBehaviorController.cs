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

    public LayerMask playerMask;
    public float attackRange;
    //判断是否允许攻击

    //判断是否已经分裂
    public int isDivision;

    public static EnemyBehaviorController instance;
    public string objname;

    bool isCanAttack;
    Collider2D col;

    float abc;
    int hurtPlayerNum;

    public bool isHit;
    public bool isCanPlayHitAnim;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isCanMove = true;

        anima = GetComponent<Animator>();
        nextMoveNum = 0;
        moveKeep = 0;
        isDeath = false;

        int len = EnemyController.instance.enemyDatas.Length;
        for (var i = 0; i < len; i++)
            if (name == EnemyController.instance.enemyDatas[i].cEnemyPrefabs.name + "(Clone)")
                index = i;

        hp = EnemyController.instance.enemyDatas[index].cHp;
        damage = EnemyController.instance.enemyDatas[index].cDamage;
        objname = EnemyController.instance.enemyDatas[index].cEnemyPrefabs.name;
    }

    private void Update()
    {
        if (Player.instance.playerIsRoomIndex != roomindex)
            return;

        ////////////////////////////////////////Die
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
            EnemyController.instance.ReMoveDeathEnemy(roomindex, gameObject);
            enabled = false;
            return;
        }
        ////////////////////////////////////////Idle
        ////默认动作就是idle
        ////////////////////////////////////////Attack
        if (CheckIsAttack())
            isCanAttack = true;

        col = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);

        if (isCanAttack)
        {
            if (!CheckIsAttack() && attackTime >= attackNextTime)
            {
                isCanAttack = false;
                attackTime = 0;
                return;
            }

            if (attackTime >= attackNextTime)
            {
                anima.SetTrigger(objname + "Attack");
                attackTime = 0;
                if (anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    Invoke(nameof(ReMove), 0.5f);
            }
            else if (attackTime < attackNextTime)
                attackTime += Time.deltaTime;
        }

        if (hurtPlayerNum == 1)
        {
            Player.instance.anima.SetTrigger("PlayerHurt");
            Player.instance.hpBuffer += damage;
            hurtPlayerNum = 0;
        }
        ////////////////////////////////////////Move
        float aaa = Vector3.Cross(Vector3.forward, (Player.instance.transform.position - transform.position).normalized).y;
        if ((aaa > 0 && transform.localScale.x < 0) || (aaa < 0 && transform.localScale.x > 0))
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        if (abc == 1)
            speed = EnemyController.instance.enemyDatas[index].cSpeed;
        else if (abc == 0)
            speed = 0;

        targetPos = Player.instance.gameObject.transform.position;
        if (Vector2.Distance(transform.position, targetPos) < attackRange + 3.0f && !isCanAttack)
        {
            anima.SetTrigger(objname + "Move");

            float aa = speed / Vector2.Distance(transform.position, targetPos) / 2;
            transform.position = Vector2.Lerp(transform.position, targetPos, aa * Time.deltaTime);
        }
        ////////////////////////////////////////Hit
        Debug.Log(isHit + "11111");
        Debug.Log(isCanPlayHitAnim + "22222");
        if (isHit && isCanPlayHitAnim)
        {
            Debug.Log("isHurt");
            anima.Play(objname + "Hit");
            isHit = false;
            isCanPlayHitAnim = false;
        }
    }

    private void FixedUpdate()
    {

    }

    void ReMove()
    {
        isCanAttack = false;
    }

    bool CheckIsAttack()
    {
        return Physics2D.OverlapCircle(transform.position, attackRange, playerMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    //动作关键帧事件
    public void SetSpeed(int a)
    {
        abc = a;
    }
    public void SetDivision(int a)
    {
        isDivision = 1;
    }
    public void HurtPlayer(int a)
    {
        hurtPlayerNum = a;
    }
 
}
