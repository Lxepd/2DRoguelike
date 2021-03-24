using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    Transform target;

    Vector3 fristPoint = new Vector3(0,1,-10);

    public float speed;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (target != null)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x,
                target.position.y, target.position.z)+fristPoint, speed * Time.deltaTime);
    }

    public void UpdateCameraPos(Transform newPos)
    {
        target = newPos;
    }
}
