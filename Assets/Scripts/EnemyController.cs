using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    public EnemyData[] enemyDatas;
    public RoomController roomController;

    public int enemyNum;
    public Transform roomEnemyParent;

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
        for (var i = 0; i < roomController.roomPoints.Count; i++)
            for (var j = 0; j < enemyNum; j++)
            {
               GameObject go = Instantiate(enemyDatas[0].cEnemyPrefabs, SwitchCreatePos(roomController.roomPoints[i]),
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


}
