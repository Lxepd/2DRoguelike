using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviorController : MonoBehaviour
{
    public int roomindex;

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
    //
    AnimatorStateInfo info;
    //目标位置
    Vector3 targetPos;
    //位置偏移量
    Vector3 moreTimeMove;
    //每段移动的百分比
    float t = 0;
      
    //该行为的次数（用于怪物散步）
    int nextMoveNum;
    //当前移动次数记录
    int moveKeep;

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

    public EnemyData thisEnemy;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isCanMove = true;
        isDeath = false;

        anima = GetComponent<Animator>();
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
        Debug.Log(thisEnemy.EnemySpeed);

        if (Vector2.Distance(transform.position, Player.instance.gameObject.transform.position) >= attackRange)
            transform.position = Vector2.MoveTowards(transform.position, Player.instance.gameObject.transform.position, thisEnemy.EnemySpeed * Time.deltaTime);
        

        ////////////////////////////////////////Hits
        if (isHit)
        {
            anima.Play(objname + "Hit");
        }
    }
    //Attack
    public void EnemyAttack()
    {
        
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
                anima.SetTrigger(thisEnemy.EnemyName + "Attack");
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
            Player.instance.hpBuffer += thisEnemy.EnemyDamage;
            hurtPlayerNum = 0;
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

    public void Hit()
    {
        if (thisEnemy.EnemyHp != 0)
        {
            isHit = true;

            thisEnemy.EnemyHp--;
        }
        else
            isDeath = true;
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
