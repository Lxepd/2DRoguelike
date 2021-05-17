using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollder : MonoBehaviour
{
    public static AttackCollder instance;

    private void Awake()
    {
        instance = this;
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyBehaviorController>().EnemyHit();

            if (Player.instance.hitCount == 3)
                collision.GetComponent<EnemyBehaviorController>().isdiff = true;
        }
    }
}
