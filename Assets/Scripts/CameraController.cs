using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    Transform target;

    Vector3 fristPoint = new Vector3(0,1,-10);

    public float speed;

    public bool isCheck;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x,
                target.position.y, target.position.z) + fristPoint, speed * Time.deltaTime);

        GetPlayerNowRoomIndex(transform.position - fristPoint, isCheck);

    }  

    public void UpdateCameraPos(Transform newPos)
    {
        isCheck = false;

        target = newPos;      
    }

    void GetPlayerNowRoomIndex(Vector2 pos, bool tof)
    {
        if (!tof)
            for (var i = 0; i < RoomController.instance.roomPoints.Count; i++)
                if (RoomController.instance.roomPoints[i] == pos)
                {
                    Player.instance.playerIsRoomIndex = i;
                    break;
                }
    }
}
