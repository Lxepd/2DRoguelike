using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public static Item instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Bag> itemAllList = new List<Bag>();
    public List<Bag> itemList = new List<Bag>();
    int itemAllNum;

    int itemRan;

    private void Start()
    {
        itemAllList = XmlManager.instance.LoadXML();
        itemAllNum = itemAllList.Count;
        int num = 8;
        for (var i = 0; i < num; i++)
        {
            int ran = Random.Range(0, itemAllList.Count);

            while (itemList.IndexOf(itemAllList[ran]) > -1)
                ran = Random.Range(0, itemAllList.Count);

            itemList.Add(itemAllList[ran]);
        }

    }

    public void RoomIsNull()
    {
        itemRan = Random.Range(0, itemList.Count - 1);
        Vector2 playerRoomPos = RoomController.instance.roomPoints[Player.instance.playerIsRoomIndex];

        GameObject go = Instantiate(BagController.instance.itemGo);
        go.GetComponent<SpriteRenderer>().sprite = itemList[itemRan].itemSprite;
        //遗物生成的位置
        go.transform.position = new Vector2(playerRoomPos.x + Random.Range(-7.5f, 7.5f), playerRoomPos.y + Random.Range(-2.5f, 3.5f));

        
    }

    public int ReturnItemIndex() { return itemRan; }

    public string ReturnItemText(Bag go)
    {
        for (var i = 0; i < itemAllNum; i++)
            if (go.itemId == itemAllList[i].itemId)
                return itemAllList[i].itemText;

        return "";
    }
}
