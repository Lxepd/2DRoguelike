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

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBehaviorController.instance.Hit();

        }
    }
}
