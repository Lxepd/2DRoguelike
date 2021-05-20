using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//魔物基本属性
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

    public static EnemyData CopyData(EnemyData indexoflist)
    {
        EnemyData data = new EnemyData();

        data.EnemyName = indexoflist.EnemyName;
        data.EnemyId = indexoflist.EnemyId;
        data.isBigEnemy = indexoflist.isBigEnemy;
        data.EnemyHp = indexoflist.EnemyHp;
        data.EnemyDamage = indexoflist.EnemyDamage;
        data.EnemySkill = indexoflist.EnemySkill;
        data.EnemySpeed = indexoflist.EnemySpeed;
        data.EnemyKind = indexoflist.EnemyKind;
        data.KindText = indexoflist.KindText;

        return data;
    }
}
//房间魔物列表
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