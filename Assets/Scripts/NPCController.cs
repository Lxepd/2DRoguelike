using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,

    }
    public Direction direction;
    //NPCList
    public List<GameObject> npcList = new List<GameObject>();
    //Npc
    GameObject npc1;
    //
    Animator npc1Anima;
    //
    public int npcRoomIndex;
    //
    public float npcBehaviorTotalTime;
    public float npcBehaviorTime;
    public float moreTime;
    int npc1Dir;
    //
    Vector3 keepPos;
    //
    public float moveSpeed;
    //
    public static NPCController instance;

    Vector3 targetPos;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        npcBehaviorTime = npcBehaviorTotalTime;
        npc1 = Instantiate(npcList[0], RoomController.instance.roomPoints[npcRoomIndex], Quaternion.identity);
        targetPos = npc1.transform.position;
        npc1Anima = npc1.GetComponent<Animator>();

        npc1Dir = 3;
    }

    // Update is called once per frame
    void Update()
    {
        float tdt = Time.deltaTime;

        NpcMove(tdt);
    }

    public void NpcMove(float deltatime)
    {
        if (Vector2.Distance(npc1.transform.position, targetPos) != 0)
        {
            if (keepPos == npc1.transform.position || Vector2.Distance(npc1.transform.position, keepPos) < 0.1f)
            {
                moreTime += deltatime;
                if (moreTime > 2f)
                {
                    targetPos = npc1.transform.position;
                    npc1Anima.SetBool("Run", false);
                    moreTime = 0;
                    return;
                }
            }

            keepPos = npc1.transform.position;
            npc1.transform.position = Vector2.MoveTowards(npc1.transform.position, targetPos, moveSpeed * deltatime);
            npc1Anima.SetBool("Run", true);
            return;
        }
        else
            npc1Anima.SetBool("Run", false);

        if (Random.Range(1, 101) <= 60 && npcBehaviorTime <= 0)
        {
            npcBehaviorTime = npcBehaviorTotalTime;

            int select = Random.Range(0, 4);

            if ((select == 2 || select == 3) && npc1Dir != select && (npc1Dir != 0 || npc1Dir != 1))
            {
                npc1.transform.localScale = new Vector2(npc1.transform.localScale.x * -1, npc1.transform.localScale.y);

                Debug.Log(select + "," + npc1Dir);
            }

            npc1Dir = select;
            switch ((Direction)select)
            {
                case Direction.Up:
                    targetPos += new Vector3(0, 1.5f, 0);
                    break;
                case Direction.Down:
                    targetPos += new Vector3(0, -1.5f, 0);
                    break;
                case Direction.Left:
                    targetPos += new Vector3(-1.5f, 0, 0);
                    break;
                case Direction.Right:
                    targetPos += new Vector3(1.5f, 0, 0);
                    break;
            }
        }
        else
            npcBehaviorTime -= deltatime * 0.8f;

    }
}
