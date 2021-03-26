using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public EnemyData[] enemyDatas;
    public RoomController roomController;

    public Transform roomEnemyParent;

    int enemyNum;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EnemyCreate();
    }

    void EnemyCreate()
    {
        enemyNum = Random.Range(3, 6);

        for (var i = 1; i < roomController.roomPoints.Count-1; i++)
            for (var j = 0; j < enemyNum; j++)
            {
               GameObject go = Instantiate(SwitchEnemy(), SwitchCreatePos(roomController.roomPoints[i]),
                    Quaternion.identity);
                go.transform.parent = roomController.rooms[i].transform;
            }
    }

    Vector2 SwitchCreatePos(Vector2 roomPos)
    {
        int ran = Random.Range(0, 3);

        switch (ran)
        {
            case 0:
                //ÖÐ¼ä               
                break;
            case 1:
                //×ó±ß
                roomPos += new Vector2(-5, 2f);
                break;
            case 2:
                //ÓÒ±ß
                roomPos += new Vector2(5f, 2f);
                break;
        }

        return roomPos;
    }

    GameObject SwitchEnemy()
    {
        int enemyInAllEnemy = Random.Range(0, enemyDatas.Length);

        return enemyDatas[enemyInAllEnemy].cEnemyPrefabs;
    }
}
