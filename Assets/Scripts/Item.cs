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
        itemAllList = XmlManager.instance.CopyItemList();
        itemAllNum = itemAllList.Count;
        int roomNum = 8;
        for (var i = 0; i < roomNum; i++)
        {
            int ran = Random.Range(1, itemAllList.Count);

            while (itemList.IndexOf(itemAllList[ran]) > -1)
                ran = Random.Range(1, itemAllList.Count);

            itemList.Add(itemAllList[ran]);
        }

    }

    public void RoomIsNull(int ywgl)
    {
        if (ywgl <= 20)
        {
            itemRan = Random.Range(1, itemList.Count - 1);
            Vector2 playerRoomPos = RoomController.instance.roomPoints[Player.instance.playerIsRoomIndex];

            GameObject go = Instantiate(BagController.instance.itemGo);
            go.GetComponent<SpriteRenderer>().sprite = itemList[itemRan].itemSprite;
            //遗物生成的位置
            go.transform.position = new Vector2(playerRoomPos.x + Random.Range(-7.5f, 7.5f), playerRoomPos.y + Random.Range(-2.5f, 3.5f));
        }
        else
        {
            itemRan = 0;
            Vector2 playerRoomPos = RoomController.instance.roomPoints[Player.instance.playerIsRoomIndex];

            int coinNum = Random.Range(3, 6);
            Vector2 coinPos = new Vector2(playerRoomPos.x + Random.Range(-7.5f, 7.5f), playerRoomPos.y + Random.Range(-2.5f, 3.5f));
            for (var i = 0; i < coinNum; i++)
            {
                GameObject go = Instantiate(BagController.instance.itemGo);
                go.GetComponent<SpriteRenderer>().sprite = itemList[itemRan].itemSprite;
                go.transform.localScale = new Vector2(.5f, .5f);
                //Coin生成的位置
                go.transform.position = coinPos + new Vector2(Random.Range(-.7f, .7f), Random.Range(-.7f, .7f));
                go.GetComponent<Animator>().SetTrigger("Coin");
            }
        }
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
