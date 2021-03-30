using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //动画机
    Animator anima;
    //刚体
    Rigidbody2D rg2d;
    //移动的变量
    Vector2 keyPos;
    //玩家属性
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
    public bool isAttack;
    //检测是否被攻击
    public bool isHurt;
    //血条
    public Slider hpSlider;
    //累积收到的伤害
    public float hpSum; 
    /************************/
    public static Player instance;

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
        if(hpSlider.value == 0)
        {
            anima.Play("PlayerDie");
            return;
        }

        Move();
        HpBarUpdate();
    }

    void Move()
    {
        keyPos.x = Input.GetAxisRaw("Horizontal");
        keyPos.y = Input.GetAxisRaw("Vertical");

        if (keyPos.x != 0)
            transform.localScale = new Vector2(keyPos.x * 2f, transform.localScale.y);

        SwitchAnima();
    }

    private void FixedUpdate()
    {
        rg2d.MovePosition(rg2d.position + keyPos * playerData.cSpeed * Time.fixedDeltaTime);
    }

    void SwitchAnima()
    {
        anima.SetFloat(name + "speed", keyPos.magnitude);

        isAttack = false;
        Attack();
    }

    void Attack()
    {
        if (timeToAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (AttackCollder.instance.AttackGo.Count != 0)
                    isAttack = true;

                anima.SetTrigger(name + "Attack");
                timeToAttack = startTimeToAttack;
            }
        }
        else
            timeToAttack -= Time.deltaTime;

    }

    public void ChangeSpeed(float cspeed)
    {
        playerData.cSpeed = cspeed;
    }

    void HpBarUpdate()
    {
        if (isHurt)
        {
            Debug.Log("啊，被打了.jpg  //By Player");
            anima.SetTrigger("PlayerHurt");
            isHurt = false;
        }
        float buckleBloodSpeed = Time.deltaTime * 5;

        hpSlider.value -= hpSum * buckleBloodSpeed;
        hpSum -= hpSum * buckleBloodSpeed;
    }
}

[System.Serializable]
public class PlayerData
{
    [HideInInspector]
    public float cSpeed;
}
[System.Serializable]
public class EquipmentData
{
    public Sprite cEquipment;
    public float cAttackRange;
}