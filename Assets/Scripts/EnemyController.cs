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

    bool nowInit = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EnemyCreate();
        nowInit = true;
    }
    private void FixedUpdate()
    {

    }

    void EnemyCreate()
    {
        eir[0].cEnemyNum = 0;
        int len1 = roomController.roomPoints.Count;

        for (var i = 1; i < len1; i++)
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
    void RemoveEnemy()
    {
        //TOADD
        //当玩家离开地牢时，移除所以东西
    }

    Vector2 SwitchCreatePos(Vector2 roomPos)
    {
        float randomX = Random.Range(-6, 6);
        float randomY = Random.Range(2.5f, -2.5f);

        roomPos = new Vector2(roomPos.x + randomX, roomPos.y + randomY);

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
            case "史莱姆":
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