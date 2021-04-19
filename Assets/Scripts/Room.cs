using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    public List<GameObject> doorDatas = new List<GameObject>();

    string wallIndex;

    public static Room instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {

    }

    public int GetWallIndex()
    {
        wallIndex = "";

        int len = doorDatas.Count;

        for (var i = 0; i < len; i++)
            GetIndex(doorDatas[i].activeSelf);

        return Convert.ToInt32(wallIndex, 2) - 1;
    }

    string GetIndex(bool rombool)
    {
        if (rombool)
            return wallIndex += "1";
        else
            return wallIndex += "0";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraController.instance.UpdateCameraPos(transform);
        }
    }
}

[Serializable]
public class WallType
{
    public List<GameObject> wallFrefab;
    
}