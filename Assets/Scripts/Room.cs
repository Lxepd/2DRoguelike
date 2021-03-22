using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour
{
    public GameObject upDoor,downDoor,leftDoor,rightDoor;

    [HideInInspector]
    public bool isUp, isDown, isLeft, isRight;

    string wallIndex;

    public static Room instance;

    void Start()
    {
        instance = this;

        upDoor.SetActive(isUp);
        downDoor.SetActive(isDown);
        leftDoor.SetActive(isLeft);
        rightDoor.SetActive(isRight);
    }

    public int GetWallIndex()
    {
        wallIndex = "";

        GetIndex(isUp);
        GetIndex(isDown);
        GetIndex(isLeft);
        GetIndex(isRight);

        return Convert.ToInt32(wallIndex, 2) - 1;
    }

    string GetIndex(bool rombool)
    {
        if (rombool)
            return wallIndex += "1";
        else
            return wallIndex += "0";
    }
}

[Serializable]
public class WallType
{
    public List<GameObject> wallFrefab;
    
}