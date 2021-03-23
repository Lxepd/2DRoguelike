using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Vector3 target;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
    }

    public void UpdateCameraPos(Vector3 pos)
    {
        transform.position += pos;
    }
}
