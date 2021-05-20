using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    //魔物所在房间
    public int roomindex;
    //是否死亡
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
    //动画状态组件
    AnimatorStateInfo info;
    //刚体组件
    Rigidbody2D rb;
    //碰撞器组件
    Collider2D col;
    //当前攻击的时间
    public float attackTime;
    //允许攻击的时间
    public float attackNextTime;
    //攻击范围
    public float attackRange;
    //玩家所在图层
    public LayerMask playerMask;
    //判断是否已经分裂
    public int isDivision;
    //是否允许攻击
    bool isCanAttack;
    //速度恢复
    float abc;
    //控制角色是否受伤
    bool hurtPlayerNum;
    //是否受伤
    public bool isHit;
    //反向位移总量
    float diffBuffer;
    //是否在击退状态
    public bool isdiff;
    //玩家位置
    Vector3 playerPos;
    //朝向玩家位置的反向向量
    Vector3 different;
    //该魔物对象的基本信息
    public EnemyData thisEnemy;
    //
    float speedTemp;
    //脚本单例
    public static EnemyBehaviorController instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isCanMove = true;
        isDeath = false;
        isdiff = false;
        hurtPlayerNum = false;

        anima = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (Player.instance.playerIsRoomIndex != roomindex)
            return;
        if (BagController.instance.bagPanel)
            return;

        EnemyDie();
        EnemyAttack();
        ////////////////////////////////////////Idle
        ////默认动作就是idle

        ////////////////////////////////////////Move

        if (!CheckIsAttack() && !isCanAttack && !isdiff && !isDeath && Vector2.Distance(transform.position, Player.instance.gameObject.transform.position) >= attackRange)
            EnemyMove();

        ////////////////////////////////////////Hit
        info = anima.GetCurrentAnimatorStateInfo(0);
        if (isHit)
        {
            anima.Play(thisEnemy.EnemyName + "Hit");
            if (info.normalizedTime > 0.6f)
                isHit = false;
        }

        if(isdiff)
        {
            float td = Time.deltaTime;
            transform.position = new Vector2(transform.position.x + different.x * diffBuffer * td, transform.position.y + different.y * diffBuffer * td);

            diffBuffer -= Time.deltaTime * 2.5f;
            if (diffBuffer <= 0.5f)
            {
                diffBuffer = 0;
                isdiff = false;
            }
        }

    }
    //Move
    public void EnemyMove()
    {
        anima.SetBool(thisEnemy.EnemyName + "Move", true);
        transform.position = Vector2.MoveTowards(transform.position, Player.instance.gameObject.transform.position, thisEnemy.EnemySpeed * Time.deltaTime);

    }
    //Attack
    public void EnemyAttack()
    {
        if (CheckIsAttack())
        {
            anima.SetBool(thisEnemy.EnemyName + "Move", false);
            isCanAttack = true;
        }

        col = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);

        if (isCanAttack)
        {
            //if (!CheckIsAttack() && attackTime >= attackNextTime)
            //{
            //    isCanAttack = false;
            //    attackTime = 0;
            //    return;
            //}

            if (attackTime >= attackNextTime)
            {
                if (CheckIsAttack())
                    anima.SetTrigger(thisEnemy.EnemyName + "Attack");
                attackTime = 0;
                if (anima.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
                    isCanAttack = false;
            }
            else if (attackTime < attackNextTime)
                attackTime += Time.deltaTime;
        }

        if (CheckIsAttack() && hurtPlayerNum)
        {
            Player.instance.anima.SetTrigger("PlayerHurt");
            Player.instance.hpBuffer += thisEnemy.EnemyDamage;
            hurtPlayerNum = false;
        }
    }
    //Die
    public void EnemyDie()
    {
        if (isDeath && thisEnemy.isBigEnemy)
        {
            if (isDivision == 1)
            {
                EnemyController.instance.SwitchEnemySkill(thisEnemy.EnemyKind, gameObject, roomindex);
                EnemyController.instance.ReMoveDeathEnemy(roomindex, thisEnemy.EnemyId);
                isDivision++;
                Destroy(gameObject);
            }
            else
                anima.Play(thisEnemy.EnemyName + "Skill");

            return;
        }
        else if (isDeath && !thisEnemy.isBigEnemy)
        {
            anima.Play(thisEnemy.EnemyName + "Death");
            EnemyController.instance.ReMoveDeathEnemy(roomindex, thisEnemy.EnemyId);
            enabled = false;
            return;
        }
    }
    //Hit
    public void EnemyHit()
    {
        if (thisEnemy.EnemyHp == 0)
        {
            isDeath = true;
            return;
        }

        if (thisEnemy.EnemyHp != 0)
        {
            isHit = true;

            thisEnemy.EnemyHp--;
            
            diffBuffer = 3;

            //受伤后向反方向移动
            //方向为 自身 - 对方（对方到自身的向量方向） 的值，再加上自身坐标各个坐标分量的值
            playerPos = Player.instance.gameObject.transform.position;
            different = transform.position - playerPos;

        }
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
    public void ReSpeed()
    {
        thisEnemy.EnemySpeed = speedTemp;
    }
    public void SetSpeed()
    {
        speedTemp = thisEnemy.EnemySpeed;
        thisEnemy.EnemySpeed = 0;
    }
    public void SetDivision(int a)
    {
        isDivision = 1;
    }
    public void HurtPlayer()
    {
        hurtPlayerNum = true;
    }
 
}
