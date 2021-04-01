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

    public EnemyInRoom[] eir;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EnemyCreate();
    }
    private void Update()
    {
        
    }

    void EnemyCreate()
    {
        eir[0].cEnemyNum = 0;

        for (var i = 1; i < roomController.roomPoints.Count; i++)
        {
            if (i == RoomController.instance.endRoomIndex)
                continue;

            enemyNum = Random.Range(3, 6);
            eir[i].cEnemyNum = enemyNum;

            for (var j = 0; j < eir[i].cEnemyNum; j++)
            {
                GameObject go = Instantiate(SwitchEnemy(), SwitchCreatePos(roomController.roomPoints[i]),
                    Quaternion.identity);
                go.transform.parent = roomController.rooms[i].transform;
                go.GetComponent<EnemyBehaviorController>().roomindex = i;
                eir[i].cEnemy.Add(go);

            }
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

    public void SwitchEnemySkill(string tag, GameObject parent, int index)
    {
        GameObject go = null;

        switch (tag)
        {
            case "Ê·À³Ä·":
                for (var i = 0; i < 2; i++)
                {
                    go = Instantiate(enemyDatas[0].cEnemyPrefabs,
                        new Vector3(parent.transform.position.x - 0.6f + i * 1.2f, parent.transform.position.y,
                        parent.transform.position.z), Quaternion.identity);
                    go.transform.parent = roomController.rooms[index].transform;
                    go.GetComponent<EnemyBehaviorController>().roomindex = index;
                    eir[index].cEnemy.Add(go);
                }
                break;
        }

    }

    public void ReMoveDeathEnemy(int index, GameObject go)
    {
        eir[index].cEnemy.Remove(go);
        eir[index].ArrayUpdata();   
    }
}