using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public Direction direction;


    [Header("房间信息")]
    //房间预制体
    public GameObject roomprefab;
    //x房间偏移量
    public int xOffset;
    //y房间偏移量
    public int yOffset;
    //房间数量
    public int roomsNum;
    public Color startColor;
    public Color endColor;

    public Transform roomParent;
    int roomToWall = 0;
    public int endRoomIndex;

    //房间表
    public List<GameObject> rooms = new List<GameObject>();
    //房间坐标存放表
    public List<Vector2> roomPoints = new List<Vector2>();
    //下一个房间的位置
    Vector2 roomNextPos;
    //保存上一个房间的变量
    Vector2 LastRoom = new Vector2(0,0);
    //房间Layer
    [Tooltip("房间碰撞判断Layer")]
    public LayerMask roomLayer;
    //墙壁
    public WallType[] wall;
    
    public EnemyController enemyController;

    bool nowInit = false;

    void Start()
    {
        InitRoom();
        nowInit = true;
    }
    private void Update()
    {
        //if (Input.anyKeyDown)
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    private void FixedUpdate()
    {
        if (nowInit && Player.instance.playerIsRoomIndex != -1)
            IsCanGoNextRoom();
    }

    public void InitRoom()
    {
        roomPoints.Add(new Vector2(0, 0));

        CreateRoom();

        for (var i = 0; i < roomPoints.Count; i++)
        {
            rooms.Add(Instantiate(roomprefab, roomPoints[i], Quaternion.identity));
            rooms[i].transform.parent = roomParent;
        }

        FindEndRoom();

        foreach (GameObject go in rooms)
            SetRoomDoors(go.GetComponent<Room>(), go.transform.position, roomToWall++);

        enemyController.enabled = true;

        instance = this;
    }

    void RemoveRoom()
    {
        //TOADD
        //当玩家离开地牢时，移除所有东西
    }

    void CreateRoom()
    {
        if (roomPoints.Count == roomsNum)
            return;

        roomNextPos = LastRoom;

        direction = (Direction)Random.Range(0, 4);

        switch (direction)
        {
            case Direction.Up:
                roomNextPos += new Vector2(0, yOffset);
                break;
            case Direction.Down:
                roomNextPos += new Vector2(0, -yOffset);
                break;
            case Direction.Left:
                roomNextPos += new Vector2(-xOffset, 0);
                break;
            case Direction.Right:
                roomNextPos += new Vector2(xOffset, 0);
                break;
        }

        LastRoom = roomNextPos;

        bool isHave = false;
        for (var i = 0; i < roomPoints.Count; i++)
            if (roomNextPos == roomPoints[i])
            {
                isHave = true;
                break;
            }

        if (isHave)
            CreateRoom();
        else if (!isHave && roomPoints.Count < roomsNum)
        {
            roomPoints.Add(roomNextPos);
            CreateRoom();
        }

    }
    
    void FindEndRoom()
    {
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        GameObject endRoomVec = rooms[0];

        for (var i = 1; i < roomsNum; i++)
        {
            if (Vector2.Distance(rooms[i].transform.position, rooms[0].transform.position) >
                Vector2.Distance(endRoomVec.transform.position, rooms[0].transform.position))
            {
                endRoomVec = rooms[i];
                endRoomIndex = i;
            }
        }

        endRoomVec.GetComponent<SpriteRenderer>().color = endColor;
    }

    void SetRoomDoors(Room rom, Vector2 rompos, int roomindex)
    {
        rom.doorDatas[0].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(0, yOffset), 0.2f, roomLayer));
        rom.doorDatas[1].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(0, -yOffset), 0.2f, roomLayer));
        rom.doorDatas[2].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(-xOffset, 0), 0.2f, roomLayer));
        rom.doorDatas[3].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(xOffset, 0), 0.2f, roomLayer));

        CreateRoomWall(rom, rompos).transform.parent = rooms[roomindex].transform;
    }

    GameObject CreateRoomWall(Room rom,Vector2 rompos)
    {
        //                                         ↓这是墙的类别↓
        return Instantiate(wall[rom.GetWallIndex()].wallFrefab[0], rompos, Quaternion.identity);
    }

    public void IsCanGoNextRoom()
    {
        if (CheckEnemyIsNull())
        {
            for (var i = 0; i < rooms.Count; i++)
            {
                GameObject go = rooms[i];

                for (var j = 0; j < Room.instance.doorDatas.Count; j++)
                {
                    if (!go.GetComponent<Room>().doorDatas[j].activeSelf)
                        continue;

                    go.GetComponent<Room>().doorDatas[j].GetComponent<BoxCollider2D>().enabled = false;
                }
            }

            Debug.Log(11123);
            //TOADD
            //掉落遗物或者道具


        }
        else if (!CheckEnemyIsNull())
        {
            GameObject go = rooms[Player.instance.playerIsRoomIndex];

            for (var i = 0; i < Room.instance.doorDatas.Count; i++)
            {
                if (!go.GetComponent<Room>().doorDatas[i].activeSelf)
                    continue;

                go.GetComponent<Room>().doorDatas[i].GetComponent<BoxCollider2D>().enabled = true; ;
            }
        }
    }

    public bool CheckEnemyIsNull()
    {
        if (enemyController.eir[Player.instance.playerIsRoomIndex].cEnemy.Count == 0)
            return true;

        return false;
    }
}