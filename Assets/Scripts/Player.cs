using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anima;
    Rigidbody2D rg2d;

    Vector2 keyPos;

    public PlayerData playerData;
    public EquipmentData[] equipmentData;

    public float startTimeToAttack;
    public float timeToAttack;

    public Transform attackPos;

    public LayerMask hurtMask;

    public bool isAttack;
    public bool isHurt;

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

        playerData.cHp = 10;
    }

    private void Update()
    {
        Move();

        if (isHurt)
            Hurt();
    }

    void Move()
    {
        keyPos.x = Input.GetAxisRaw("Horizontal");
        keyPos.y = Input.GetAxisRaw("Vertical");

        if (keyPos.x != 0)
            transform.localScale = new Vector2(keyPos.x * 1.65f, transform.localScale.y);

        SwitchAnima();
    }

    private void FixedUpdate()
    {
        rg2d.MovePosition(rg2d.position + keyPos * playerData.cSpeed * Time.fixedDeltaTime);
    }

    void SwitchAnima()
    {
        anima.SetFloat("speed", keyPos.magnitude);

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

                anima.SetTrigger("Attack");
                timeToAttack = startTimeToAttack;
            }
        }
        else
            timeToAttack -= Time.deltaTime;

    }

    public void Hurt()
    {
        Debug.Log("不要打我啊，好痛啊，呜呜呜/(ㄒoㄒ)/~~\nBy player");
        isHurt = false;
    }

    public void ChangeSpeed(float cspeed)
    {
        playerData.cSpeed = cspeed;
    }

}

[System.Serializable]
public class PlayerData
{
    [HideInInspector]
    public float cSpeed;
    public float cHp;
}
[System.Serializable]
public class EquipmentData
{
    public Sprite cEquipment;
    public float cAttackRange;
}