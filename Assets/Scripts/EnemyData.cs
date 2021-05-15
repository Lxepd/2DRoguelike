using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyData
{
    public string EnemyName;
    public int EnemyId;
    public bool isBigEnemy;
    public int EnemyHp;
    public int EnemyDamage;
    public string EnemySkill;
    public float EnemySpeed;
    public string EnemyKind;
    public string KindText;

}
[System.Serializable]
public class EnemyInRoom
{
    public List<int> cEnemyId = new List<int>();
    public int cEnemyNum;

    public void ArrayUpdata()
    {
        cEnemyNum = cEnemyId.Count;
    }

}