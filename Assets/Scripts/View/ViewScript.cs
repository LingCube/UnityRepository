using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TaskMode;
using DataStruct;
using BagMode;
using CharacterMode;

namespace UIManger
{
    namespace Window
    {
        public class BaseWin
        {
            public BaseWin()
            {

            }

            public virtual void Updata()
            {

            }

            protected Transform Root;

            protected Button RootOpenBtn, RootCloseBtn;

            protected virtual void Open()
            {
                Root.parent.SetAsLastSibling();
                Root.gameObject.SetActive(true);
            }

            protected virtual void Close()
            {
                Root.gameObject.SetActive(false);
            }
        }
    }
}

public class CharacterView : UIManger.Window.BaseWin
{

    public CharacterView(Transform _root)
    {
        Root = _root;
    }

    public override void Updata()
    {
        base.Updata();
    }

    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {
        base.Open();
    }

}

public class BagView : UIManger.Window.BaseWin
{

    public BagView()
    {
        Root = GameObject.Find("Canvas/BagUI/BagUIOpen").transform;
        BagItemImage = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemExplain/BagItemImage").GetComponent<Image>();
        BagItemNameText = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemExplain/BagItemNameText").GetComponent<Text>();
        BagPageText = GameObject.Find("Canvas/BagUI/BagUIOpen/BagTurnPage/BagPageText").GetComponent<Text>();
    }

    public override void Updata()
    {
        base.Updata();
    }

    protected List<GameObject> BagItemObjList = new List<GameObject>();

    protected List<Text> BagItemMainPropTextList = new List<Text>();

    protected Dictionary<GameObject, int> BagItemObjToIndexDic = new Dictionary<GameObject, int>();

    protected Image BagItemImage;

    protected Text BagItemNameText, BagPageText;


    // BagItemTypeIndex: 0 Equip,  1 Drug,  2 Tool
    protected int BagItemListIndex = 0, BagItemTypeIndex = 0, BagItemObjListPageIndex = 0;

    protected List<Equip> BagItemEquipList = new List<Equip>();

    protected List<Drug> BagItemDrugList = new List<Drug>();

    protected List<Tool> BagItemToolList = new List<Tool>();
    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {
        base.Open();
    }

    

    protected virtual void BagItemOnView()
    {
        for (int i = 0; i < BagItemObjList.Count; i++)
        {
            switch (BagItemTypeIndex)
            {
                case 0:
                    if (BagItemObjListPageIndex * BagItemObjList.Count + i < BagItemEquipList.Count)
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(true);
                        }
                        if (BagItemObjListPageIndex * BagItemObjList.Count + i == BagItemListIndex)
                        {
                            BagItemNameText.text = BagItemEquipList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemName;
                            BagItemImage.sprite = BagItemEquipList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemSprite;
                        }
                        BagItemMainPropTextList[i].text = "Lv." + BagItemEquipList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetLevel.ToString();
                    }
                    else
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    break;
                case 1:
                    if (BagItemObjListPageIndex * BagItemObjList.Count + i < BagItemDrugList.Count)
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(true);
                        }
                        if (BagItemObjListPageIndex * BagItemObjList.Count + i == BagItemListIndex)
                        {
                            BagItemNameText.text = BagItemDrugList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemName;
                            BagItemImage.sprite = BagItemDrugList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemSprite;
                        }
                        BagItemMainPropTextList[i].text = BagItemDrugList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetCnt.ToString();
                    }
                    else
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    break;
                case 2:
                    if (BagItemObjListPageIndex * BagItemObjList.Count + i < BagItemToolList.Count)
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(true);
                        }
                        if (BagItemObjListPageIndex * BagItemObjList.Count + i == BagItemListIndex)
                        {
                            BagItemNameText.text = BagItemToolList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemName;
                            BagItemImage.sprite = BagItemToolList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetBagItemSprite;
                        }
                        BagItemMainPropTextList[i].text = BagItemToolList[BagItemObjListPageIndex * BagItemObjList.Count + i].GetCnt.ToString();
                    }
                    else
                    {
                        for (int j = 0; j < BagItemObjList[i].transform.childCount; j++)
                        {
                            BagItemObjList[i].transform.GetChild(j).gameObject.SetActive(false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        int allpage = BagItemEquipList.Count / BagItemObjList.Count;
        if (BagItemEquipList.Count % BagItemObjList.Count > 0) allpage++;
        BagPageText.text = (BagItemObjListPageIndex + 1).ToString() + " / " + allpage.ToString();
    }

}

public class TaskView : UIManger.Window.BaseWin
{
    public TaskView()
    {

    }

    protected List<Task> TaskList = new List<Task>();

    protected List<Text> TaskItemList = new List<Text>();

}