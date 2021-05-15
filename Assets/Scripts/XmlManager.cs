using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

public class XmlManager : MonoBehaviour
{
    public static XmlManager instance;

    List<Bag> itemList = new List<Bag>();
    List<EnemyData> EnemyList = new List<EnemyData>();

    private void Awake()
    {
        instance = this;
        LoadXML();
    }

    public string Data = "Assets/XML/Data.xml";

    public void LoadXML()
    {
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        //读取xml
        xml.Load(Data);
        //读取xxx节点下的所有子节点
        XmlNodeList XmlList = xml.SelectSingleNode("数据").ChildNodes;
        foreach (XmlNode xxx in XmlList)
        {
            if (xxx.Name == "道具")
            {
                foreach (XmlNode item in xxx.ChildNodes)
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


            }
            if (xxx.Name == "魔物")
            {
                foreach (XmlNode enemy in xxx.ChildNodes)
                {

                    XmlAttributeCollection idAttribute = enemy.Attributes;
                    var List = new EnemyData();

                    List.EnemyName = idAttribute["魔物名"].InnerText;
                    List.EnemyId = int.Parse(idAttribute["id"].InnerText);
                    List.isBigEnemy = bool.Parse(idAttribute["精英"].InnerText);
                    List.EnemyHp = int.Parse(idAttribute["血量"].InnerText);
                    List.EnemyDamage = int.Parse(idAttribute["伤害"].InnerText);
                    List.EnemySkill = idAttribute["技能"].InnerText;
                    List.EnemySpeed = float.Parse(idAttribute["移速"].InnerText);
                    List.EnemyKind = idAttribute["种族"].InnerText;
                    List.KindText = idAttribute["魔物描述"].InnerText;

                    EnemyList.Add(List);

                }
            }
        }
    }

    public List<Bag> CopyItemList() { return itemList; }
    public List<EnemyData> CopyEnemyList() { return EnemyList; }
}

