using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagController : MonoBehaviour
{
    public static BagController instance;

    private void Awake()
    {
        instance = this;
    }

    public List<GameObject> box = new List<GameObject>();
    int itemInBoxIndex;
    int boxIndex;

    public GameObject bagPrefab;
    public GameObject bagGround;
    public GameObject bagSelect;

    [HideInInspector]
    public bool bagPanel;

    public GameObject bagGo;

    public GameObject itemGo;
    public GameObject bagItemGo;

    private void Start()
    {
        itemInBoxIndex = 0;
        boxIndex = 0;
        for (var i = 0; i < 20; i++)
        {
            GameObject go = Instantiate(bagPrefab);        
            go.transform.parent = bagGround.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
            box.Add(go);
        }

        bagSelect.transform.parent = box[0].transform;
        bagSelect.transform.position = box[0].transform.position;

        //for(var i = 0;i<bagList.Count;i++)
        //{
        //    Debug.Log(bagList[i].itemId);
        //    Debug.Log(bagList[i].itemName);
        //    Debug.Log(bagList[i].itemText);
        //}

        bagPanel = false;
    }

    private void Update()
    {
        if (!bagPanel)
            return;

        BagMsgUpdate();
    }

    void BagMsgUpdate()
    {
        int boxNum = box.Count;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && boxIndex > 0)
            boxIndex--;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && boxIndex < boxNum - 1)
            boxIndex++;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && boxIndex > 3)
            boxIndex -= 4;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && boxIndex < 17)
            boxIndex += 4;

        bagSelect.transform.parent = box[boxIndex].transform;
        bagSelect.transform.position = box[boxIndex].transform.position;

    }

    public void ItemGoBag(GameObject itemgo)
    {
        if (itemInBoxIndex == box.Count - 1)
            return;

        //创建背包道具预制体
        GameObject go = Instantiate(bagItemGo);
        //将图片赋予预制体
        go.GetComponent<Image>().sprite = itemgo.GetComponent<SpriteRenderer>().sprite;
        //按顺序设置父物体
        go.transform.parent = box[itemInBoxIndex].transform;
        //移到合适位置
        go.transform.position = box[itemInBoxIndex++].transform.position;
        //调整合适的大小
        go.transform.localScale = new Vector2(.7f, .7f);
    }
}

public class Bag
{
    public int itemId;
    public string itemName;
    public string itemText;
    public Sprite itemSprite;
}