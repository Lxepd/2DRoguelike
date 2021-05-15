using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    public static Entrance instance;

    public GameObject roomControllerGo;
    public GameObject enemyControllerGo;

    private void FixedUpdate()
    {
        if (CameraController.instance.isEntrance)
            Player.instance.playerIsRoomIndex = -1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "地牢初始化点":
                    {
                        InitGame();

                        break;
                    }
                case "地牢入口":
                    {
                        Player.instance.transform.position = new Vector2(0, 0);
                        CameraController.instance.DungeonCameraInit();
                        CameraController.instance.isEntrance = false;

                        break;
                    }
            }
        }
    }

    void InitGame()
    {
        roomControllerGo.GetComponent<RoomController>().enabled = true;
        enemyControllerGo.GetComponent<EnemyController>().enabled = true;
    }
}
