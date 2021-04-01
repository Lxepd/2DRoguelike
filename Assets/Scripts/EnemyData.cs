using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyData
{
    public GameObject cEnemyPrefabs;
    public float cHp;
    public float cDamage;
    public string cEnemyKind;
    public string cEnemyExplain;
    public bool cBigEnemy;
}
[System.Serializable]
public class EnemyInRoom
{
    public List<GameObject> cEnemy = new List<GameObject>();
    public int cEnemyNum;

    public void ArrayUpdata()
    {
        cEnemyNum = cEnemy.Count;
    }

}