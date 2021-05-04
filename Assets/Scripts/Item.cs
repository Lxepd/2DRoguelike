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

    private void Start()
    {
        itemAllList = XmlManager.instance.LoadXML();

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
        int ran = Random.Range(0, itemList.Count - 1);
        Vector2 playerRoomPos = RoomController.instance.roomPoints[Player.instance.playerIsRoomIndex];

        GameObject go = Instantiate(BagController.instance.itemGo);
        go.GetComponent<SpriteRenderer>().sprite = itemList[ran].itemSprite;
        go.transform.position = new Vector2(playerRoomPos.x, playerRoomPos.y);

        itemList.Remove(itemList[ran]);
    }
}
