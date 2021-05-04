using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //??
    AnimatorStateInfo stateInfo;
    //动画机
    [HideInInspector]
    public Animator anima;
    //刚体
    Rigidbody2D rg2d;
    //移动的变量
    Vector2 keyPos;
    //玩家属性
    [HideInInspector]
    public PlayerData playerData;
    //装备属性
    public EquipmentData[] equipmentData;
    //lian ji
    public int hitCount = 0;
    //0: idle | 1: attack1 | 2: attack2 | 3: attack3
    //初始攻击时间
    public float startTimeToAttack;
    //攻击判定点
    public Transform attackPos;
    //伤害Mask
    public LayerMask hurtMask;
    //检测是否允许跑动
    public bool isAttackMove;
    //检测是否攻击
    public bool isAttack;
    //造成伤害的时刻
    public bool damageTime;
    //检测是否被攻击
    public bool isHurt;
    //血条上限
    
    //血条
    public Slider hpSlider;
    //累积收到的伤害
    [HideInInspector]
    public float hpBuffer;
    //血条文本
    public Text hpText;
    //
    public int playerIsRoomIndex;
    /************************/
    public static Player instance;

    bool attackToIdle;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerData.cSpeed = 4;
        anima = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();

        hpSlider.value = hpSlider.maxValue;
    }

    private void Update()
    {
        if (BagController.instance.bagPanel)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                BagController.instance.bagPanel = false;
                BagController.instance.bagGo.SetActive(false);
            }
            return;
        }

        stateInfo = anima.GetCurrentAnimatorStateInfo(0);
        if ((stateInfo.IsName("PlayerAttack1") || stateInfo.IsName("PlayerAttack2") || stateInfo.IsName("PlayerAttack3")) &&
            stateInfo.normalizedTime >= 0.85f)
        {
            hitCount = 0;
            anima.SetInteger("Attack", hitCount);
        }

        if((int)hpSlider.value == 0)
        {
            anima.Play("PlayerDie");
            return;
        }

        
        SwitchIdle();

        Move();
        Attack();
        HpBarUpdate();

    }

    void Move()
    { 
        keyPos.x = Input.GetAxisRaw("Horizontal");
        keyPos.y = Input.GetAxisRaw("Vertical");


        if (keyPos.x != 0)
            transform.localScale = new Vector2(keyPos.x * 2f, transform.localScale.y);

        anima.SetFloat(name + "Speed", keyPos.magnitude);

    }
    private void FixedUpdate()
    {
        rg2d.MovePosition(rg2d.position + keyPos * playerData.cSpeed * Time.fixedDeltaTime);
    }
    void Attack()
    {      
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (stateInfo.IsName("PlayerAttackIdle") || stateInfo.IsName("PlayerIdle") || stateInfo.IsName("PlayerRun"))
                 hitCount = 1;
            else if (stateInfo.IsName("PlayerAttack1") && stateInfo.normalizedTime < 0.85f)
                hitCount = 2;
            else if (stateInfo.IsName("PlayerAttack2") && stateInfo.normalizedTime < 0.85f)
                hitCount = 3;

            isAttackMove = true;
            if (AttackCollder.instance.AttackGo.Count != 0)
                isAttack = true;

            anima.SetInteger("Attack", hitCount);
        }

    }

    void SwitchIdle()
    {
        if (playerIsRoomIndex == -1)
            anima.SetBool("HaveEnemy", false);
        else if (EnemyController.instance.eir[playerIsRoomIndex].cEnemy.Count == 0)
            anima.SetBool("HaveEnemy", false);
        else
            anima.SetBool("HaveEnemy", true);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BagController.instance.bagPanel = true;
            BagController.instance.bagGo.SetActive(true);
        }

    }

    void HpBarUpdate()
    {
        if (isHurt)
        {
            anima.SetTrigger("PlayerHurt");
            isHurt = false;
        }
        float buckleBloodSpeed = Time.deltaTime * 5;

        hpSlider.value -= hpBuffer * buckleBloodSpeed;
        hpBuffer -= hpBuffer * buckleBloodSpeed;

        hpText.text = (int)hpSlider.value + " / " + hpSlider.maxValue;
    }
    ///////////////////////////////////////

    ///////////////////////////////////////

    void ReSpeed()
    {
        playerData.cSpeed = 4;
    }
    //关键帧事件
    void EnemyCanPlayHitAnima()
    {
        damageTime = true;
    }
    void rrrrrrrrrrr()
    {
        damageTime = false;
    }
    void AttackTimeOver()
    {
        isAttackMove = false;
        ReSpeed();
    }
    void AttackSpeed()
    {
        playerData.cSpeed = 0;
    }
    ///////////////////////////////////////

    public float range;

    void CheckItem()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
            Debug.Log("You Pick it");
    }
}

[System.Serializable]
public class PlayerData
{
    public float cSpeed;
}
[System.Serializable]
public class EquipmentData
{
    public Sprite cEquipment;
    public float cAttackRange;
}