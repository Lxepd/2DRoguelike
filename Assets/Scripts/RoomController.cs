using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviour
{
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
    //x位移量
    public int xOffset;
    //y位移量
    public int yOffset;
    //房间总数
    public int roomsNum;
    public Color startColor;
    public Color endColor;

    List<GameObject> rooms = new List<GameObject>();
    //记录房间坐标
    List<Vector2> roomPoints = new List<Vector2>();
    //下一个需要生成房间的位置
    Vector2 roomNextPos;
    //
    Vector2 LastRoom = new Vector2(0,0);
    //
    [Tooltip("判断产生门的图层")]
    public LayerMask roomLayer;
    //
    public WallType[] wall;

    void Start()
    {
        roomPoints.Add(new Vector2(0, 0));

        CreateRoom();

        for (var i = 0; i < roomPoints.Count; i++)
            rooms.Add(Instantiate(roomprefab, roomPoints[i], Quaternion.identity));

        FindEndRoom();

        foreach (GameObject go in rooms)
            SetRoomDoors(go.GetComponent<Room>(), go.transform.position);
            

        
    }

    private void Update()
    {
        //if (Input.anyKeyDown)
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            if (Vector2.Distance(rooms[i].transform.position, rooms[0].transform.position) > 
                Vector2.Distance(endRoomVec.transform.position, rooms[0].transform.position))
                endRoomVec = rooms[i];

        endRoomVec.GetComponent<SpriteRenderer>().color = endColor;
    }

    void SetRoomDoors(Room rom,Vector2 rompos)
    {
        rom.isUp = Physics2D.OverlapCircle(rompos + new Vector2(0, yOffset), 0.2f, roomLayer);
        rom.isDown = Physics2D.OverlapCircle(rompos + new Vector2(0, -yOffset), 0.2f, roomLayer);
        rom.isLeft = Physics2D.OverlapCircle(rompos + new Vector2(-xOffset,0), 0.2f, roomLayer);
        rom.isRight = Physics2D.OverlapCircle(rompos + new Vector2(xOffset,0), 0.2f, roomLayer);

        CreateRoomWall(rom, rompos);
    }

    void CreateRoomWall(Room rom,Vector2 rompos)
    {
        Instantiate(wall[rom.GetWallIndex()].wallFrefab[0], rompos, Quaternion.identity);
    }
}
