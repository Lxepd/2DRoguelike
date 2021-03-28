using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollder : MonoBehaviour
{
    public static AttackCollder instance;

    public List<GameObject> AttackGo = new List<GameObject>();

    string enemyKind;

    private void Awake()
    {
        instance = this;
    }   

    private void Update()
    {
        if (Player.instance.isAttack)
            FindEnemyTag(AttackGo[0]);
    }

    void CheckeQuipment()
    {

    }

    void DamageToHp(GameObject obj, int damage)
    {
        //Debug.Log("不要打我啊，好痛啊，呜呜呜/(ㄒoㄒ)/~~\nBy Enemy");
        obj.GetComponent<EnemyBehaviorController>().hp -= damage;

        if (obj.GetComponent<EnemyBehaviorController>().hp == 0)
        {
            obj.GetComponent<EnemyBehaviorController>().isDeath = true;
            AttackGo.Remove(obj);          
        }

    }

    public void FindEnemyTag(GameObject obj)
    {
        for (var i = 0; i < EnemyController.instance.enemyDatas.Length; i++)
            if (obj.CompareTag(EnemyController.instance.enemyDatas[i].cEnemyKind))
            {
                enemyKind = EnemyController.instance.enemyDatas[i].cEnemyKind;
                break;
            }

        switch (enemyKind)
        {
            case "史莱姆":
                DamageToHp(obj, 1);
                break;
            case "蝙蝠":
                DamageToHp(obj, 1);
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 &&!collision.GetComponent<EnemyBehaviorController>().isDeath)
            AttackGo.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        AttackGo.Remove(collision.gameObject);
    }
}
