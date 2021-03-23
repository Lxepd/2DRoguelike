using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anima;
    Rigidbody2D rg2d;

    Vector2 keyPos;
    public float speed;

    public float startTimeToAttack;
    public float timeToAttack;

    public Transform attackPos;
    public float attackRange;
    public LayerMask hurtMask;

    void Start()
    {
        speed = 4;
        anima = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
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
        rg2d.MovePosition(rg2d.position + keyPos * speed * Time.fixedDeltaTime);
    }

    void SwitchAnima()
    {
        anima.SetFloat("speed", keyPos.magnitude);

        Attack();
    }

    void Attack()
    {
        if (timeToAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Collider2D[] enemyToDamage =
                    Physics2D.OverlapCircleAll(attackPos.position, attackRange, hurtMask);

                for (var i = 0; i < enemyToDamage.Length; i++)

                anima.SetTrigger("Attack");
                timeToAttack = startTimeToAttack;
            }
        }
        else
            timeToAttack -= Time.deltaTime;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    public void ChangeSpeed(float cspeed)
    {
        speed = cspeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TODO:钥匙开门？找npc开门？…………
        //看情况改

        if (collision.transform.tag == "UDoor")
            transform.position += new Vector3(0, 2, 0);
        else if (collision.transform.tag == "DDoor")
            transform.position += new Vector3(0, -2, 0);
        else if (collision.transform.tag == "LDoor")
            transform.position += new Vector3(-3, 0, 0);
        else if (collision.transform.tag == "RDoor")
            transform.position += new Vector3(3, 0, 0);
    }
}
