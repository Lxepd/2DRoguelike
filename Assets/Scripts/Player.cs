using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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
    //初始攻击时间
    public float startTimeToAttack;
    //判断是否允许攻击的时间
    public float timeToAttack;
    //攻击判定点
    public Transform attackPos;
    //伤害Mask
    public LayerMask hurtMask;
    //检测是否已经攻击
    public bool isCanAttack;
    //
    public bool isAttack;
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
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerData.cSpeed = 3;
        anima = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();

        hpSlider.value = hpSlider.maxValue;
    }

    private void Update()
    {
        if((int)hpSlider.value == 0)
        {
            anima.Play("PlayerDie");
            return;
        }

        if (playerIsRoomIndex != -1)
            SwitchIdle();

        SwitchAnima();
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

    void SwitchAnima()
    {
        if (!isAttack)
            Move();

        Attack();
    }

    void Attack()
    {
        if (timeToAttack <= 0 && !isCanAttack)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isAttack = true;
                if (AttackCollder.instance.AttackGo.Count != 0)
                    isCanAttack = true;

                anima.SetTrigger(name + "Attack");
                timeToAttack = startTimeToAttack;
            }
        }
        else
            timeToAttack -= Time.deltaTime;

    }

    void SwitchIdle()
    {
        if (EnemyController.instance.eir[playerIsRoomIndex].cEnemy.Count == 0)
            anima.SetBool("HaveEnemy", false);
        else
            anima.SetBool("HaveEnemy", true);
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


    //关键帧事件
    public void EnemyCanPlayHitAnima()
    {
        if (playerIsRoomIndex != -1)
            EnemyBehaviorController.instance.isCanPlayHitAnim = true;
    }
    public void AttackTimeOver()
    {
        isAttack = false;
        ReSpeed();
    }
    public void AttackSpeed()
    {
        playerData.cSpeed = 0;
    }
    void ReSpeed()
    {
        playerData.cSpeed = 3;
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