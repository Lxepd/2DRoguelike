using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

public class XmlManager : MonoBehaviour
{
    public static XmlManager instance;

    private void Awake()
    {
        instance = this;
    
    }

    public string bagData = "Assets/XML/BagData.xml";

    public List<Bag> LoadXML()
    {
        var itemList = new List<Bag>();

        //创建xml文档
        XmlDocument xml = new XmlDocument();
        //读取xml
        xml.Load(bagData);
        //读取xxx节点下的所有子节点
        XmlNodeList BagList = xml.SelectSingleNode("道具").ChildNodes;
        foreach(XmlNode item in BagList)
        {
            XmlAttributeCollection idAttribute = item.Attributes;
            var List = new Bag();

            List.itemId = int.Parse(idAttribute["id"].InnerText);
            //Debug.Log(List.itemId);
            List.itemName = idAttribute["道具名"].InnerText;
            //Debug.Log(List.itemName);
            List.itemText = idAttribute["道具描述"].InnerText;
            //Debug.Log(List.itemText);
            List.itemSprite = Resources.Load<Sprite>("test/" + idAttribute["图片存放"].InnerText);

            itemList.Add(List);
        }

        return itemList;
    }
}

