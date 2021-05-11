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

    public List<GameObject> bag = new List<GameObject>();
    int itemInBagIndex;
    int bagSelectIndex;
    public List<Bag> itemMsgInBag = new List<Bag>();

    public GameObject bagPrefab;
    public GameObject bagGround;
    public GameObject bagSelectGo;

    [HideInInspector]
    public bool bagPanel;

    public GameObject bagGo;

    public GameObject itemGo;
    public GameObject bagItemGo;
    public Text itemTextInBag;

    private void Start()
    {
        itemInBagIndex = 0;
        bagSelectIndex = 0;
        for (var i = 0; i < 24; i++)
        {
            GameObject go = Instantiate(bagPrefab);        
            go.transform.parent = bagGround.transform;
            go.transform.localScale = new Vector3(1, 1, 1);
            bag.Add(go);
        }

        bagSelectGo.transform.parent = bag[0].transform;
        bagSelectGo.transform.position = bag[0].transform.position;

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
        int boxNum = bag.Count;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) && bagSelectIndex > 0)
            bagSelectIndex--;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) && bagSelectIndex < boxNum - 1)
            bagSelectIndex++;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && bagSelectIndex > 6)
            bagSelectIndex -= 6;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && bagSelectIndex < 17)
            bagSelectIndex += 6;

        bagSelectGo.transform.parent = bag[bagSelectIndex].transform;
        bagSelectGo.transform.position = bag[bagSelectIndex].transform.position;
        //更改背包中选中该道具的文本
        ChangeItemTextOfBag();
    }

    public void ItemGoBag(GameObject itemgo)
    {
        if (itemInBagIndex == bag.Count - 1)
            return;
        //创建背包道具预制体
        GameObject go = Instantiate(bagItemGo);
        //将图片赋予预制体
        go.GetComponent<Image>().sprite = itemgo.GetComponent<SpriteRenderer>().sprite;
        //按顺序设置父物体
        go.transform.parent = bag[itemInBagIndex].transform;
        //移到合适位置
        go.transform.position = bag[itemInBagIndex++].transform.position;
        //调整合适的大小
        go.transform.localScale = new Vector2(.7f, .7f);
    }

    void ChangeItemTextOfBag()
    {
        if (bagSelectIndex > itemMsgInBag.Count - 1)
        {
            itemTextInBag.text = "";
            return;
        }

        itemTextInBag.text = Item.instance.ReturnItemText(itemMsgInBag[bagSelectIndex]);
    }

    public void AddItemMsgInList(int index)
    {
        itemMsgInBag.Add(Item.instance.itemList[index]);
        Item.instance.itemList.Remove(Item.instance.itemList[index]);
    }

}

public class Bag
{
    public int itemId;
    public string itemName;
    public string itemText;
    public Sprite itemSprite;
}