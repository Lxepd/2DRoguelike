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

    //小地图
    public GameObject mapPrefab;
    public GameObject mapParent;

    void Start()
    {
        InitRoom();
        CreateMap();
    }
    private void Update()
    {
        if (Player.instance.playerIsRoomIndex >= 1 && Player.instance.playerIsRoomIndex <= rooms.Count - 1)
        {
            IsCanGoNextRoom(CheckEnemyIsNull());

            if (!rooms[Player.instance.playerIsRoomIndex].GetComponent<Room>().yiwuCreate && CheckEnemyIsNull())
            {
                rooms[Player.instance.playerIsRoomIndex].GetComponent<Room>().yiwuCreate = true;
                //Debug.Log("111111111111111111111111");
                //掉落遗物或者道具

                int ywGl = Random.Range(1, 101);
                Item.instance.RoomIsNull(ywGl);

            }
        }
        else
        {
            int roomCount = rooms.Count;
            int dootDatasCount = Room.instance.doorDatas.Count;

            for (var i = 0; i < roomCount; i++)
            {
                Room go = rooms[i].GetComponent<Room>();

                for (var j = 0; j < dootDatasCount; j++)
                {
                    if (!go.doorDatas[j].activeSelf)
                        continue;

                    go.doorDatas[j].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

    }
    //房间初始化
    public void InitRoom()
    {
        roomPoints.Add(new Vector2(0, 0));

        CreateRoom();

        int len = roomPoints.Count;

        for (var i = 0; i < len; i++)
        {
            rooms.Add(Instantiate(roomprefab, roomPoints[i], Quaternion.identity));
            rooms[i].transform.parent = roomParent;
        }

        FindEndRoom();    

        foreach (GameObject go in rooms)
            SetRoomDoors(go.GetComponent<Room>(), go.transform.position, roomToWall++);

        CreateShopRoom();

        enemyController.enabled = true;

        instance = this;
    }
    //移除房间
    void RemoveRoom()
    {
        //TOADD
        //当玩家离开地牢时，移除所有东西
    }
    //生成房间
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
    //寻找距离远的房间
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
    //生成每个房间的门
    void SetRoomDoors(Room rom, Vector2 rompos, int roomindex)
    {
        rom.doorDatas[0].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(0, yOffset), 0.2f, roomLayer));
        rom.doorDatas[1].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(0, -yOffset), 0.2f, roomLayer));
        rom.doorDatas[2].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(-xOffset, 0), 0.2f, roomLayer));
        rom.doorDatas[3].SetActive(Physics2D.OverlapCircle(rompos + new Vector2(xOffset, 0), 0.2f, roomLayer));

        CreateRoomWall(rom, rompos).transform.parent = rooms[roomindex].transform;
    }
    //生成墙壁
    //墙壁类别素材可以随时替换
    GameObject CreateRoomWall(Room rom,Vector2 rompos)
    {
        //                                         ↓这是墙的类别↓
        return Instantiate(wall[rom.GetWallIndex()].wallFrefab[0], rompos, Quaternion.identity);
    }
    //创建商店房间
    void CreateShopRoom()
    {
        int shopRan = Random.Range(1, 101);

        if (shopRan < 30)
            rooms[Random.Range(0, rooms.Count - 1)].GetComponent<Room>().isShop = true;

        //创建商人及物品
    }

    //是否允许进入下一个房间
    public void IsCanGoNextRoom(bool isnull)
    {
        int roomCount = rooms.Count;
        int dootDatasCount = Room.instance.doorDatas.Count;

        if (isnull)
        {
            for (var i = 0; i < roomCount; i++)
            {
                Room go = rooms[i].GetComponent<Room>();

                for (var j = 0; j < dootDatasCount; j++)
                {
                    if (!go.doorDatas[j].activeSelf)
                        continue;

                    go.doorDatas[j].GetComponent<BoxCollider2D>().enabled = false;
                }
            }

        }
        else if (!isnull)
        {
            Room go = rooms[Player.instance.playerIsRoomIndex].GetComponent<Room>();

            for (var i = 0; i < dootDatasCount; i++)
            {
                if (!go.doorDatas[i].activeSelf)
                    continue;

                go.doorDatas[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    //判断怪物房间怪物是否为空
    public bool CheckEnemyIsNull()
    {
        if (enemyController.eir[Player.instance.playerIsRoomIndex].cEnemyId.Count == 0)
            return true;

        return false;
    }
    //
    public void CreateMap()
    {
        foreach(Vector2 pos in roomPoints)
        {
            int gox = (int)pos.x / 16;
            int goy = (int)pos.y / 11;

            GameObject go = Instantiate(mapPrefab);
            go.transform.parent = mapParent.transform;
            go.transform.position = new Vector2(gox * 16, goy * 11);
        }

    }
}