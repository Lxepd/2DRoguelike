using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollder : MonoBehaviour
{
    public static AttackCollder instance;

    public List<GameObject> AttackGo = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }   

    private void Update()
    {
        if (Player.instance.isCanAttack)
            PlayerAttack();
    }

    public void PlayerAttack()
    {
        int len = AttackGo.Count;
        int temp;
        for (var i = 0; i < len; i++)
        {
            temp = i;
            AttackGo[i].GetComponent<EnemyBehaviorController>().hp -= 1;

            if (AttackGo[i].GetComponent<EnemyBehaviorController>().hp == 0)
            {
                AttackGo[i].GetComponent<EnemyBehaviorController>().isDeath = true;
                AttackGo.Remove(AttackGo[i]);

                i = temp - 1;
                len -= 1;
            }
        }

        Player.instance.isCanAttack = false;
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

    public bool isInAttackList(GameObject go)
    {
        if (AttackGo.IndexOf(go) == 0 && AttackGo.Count > 0)
            return true;

        return false;
    }    
}
