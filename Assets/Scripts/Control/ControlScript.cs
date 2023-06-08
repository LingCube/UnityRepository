using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using BagMode.IBag;
using BagMode.Model;
using TaskMode;
using Valve.VR;

public enum GameType
{
    Play,
    Dialog,
    Win,
    Lose
}


public class Dialog
{

    public bool GetIsDialogFinished => isDialogFinished;

    public Dialog(TextAsset text_label, GameObject dialog_obj, float texting_time)
    {
        DialogObj = dialog_obj;
        DialogChoice = GameObject.Find("Canvas/DialogWindow/DialogChoice");
        for (int i = 0; i < DialogObj.transform.childCount; i++)
        {
            if (DialogObj.transform.GetChild(i).name == "DialogNameText")
            {
                DialogNameText = DialogObj.transform.GetChild(i).GetComponent<Text>();
            }
            if (DialogObj.transform.GetChild(i).name == "DialogText")
            {
                DialogText = DialogObj.transform.GetChild(i).GetComponent<Text>();
            }
        }
        DialogObj.SetActive(false);
        TextingStartTime = TextingTime = texting_time;
        TextLabelBuild(text_label);
    }

    public Dialog()
    {
        RandTextCreate();
    }

    public virtual void Updata()
    {
        DialogingText();
    }

    protected TextAsset TextLabel;

    protected GameObject DialogObj, DialogChoice;

    protected Text DialogNameText, DialogText;

    protected List<string> NameTextList = new List<string>();

    protected List<string> TextList = new List<string>();

    protected bool isTextListFinshed = false, isDialogFinished = true;

    protected int TextListIdx = 0, TextListItemIdx = 0;

    protected float InTextingTime = 0, TextingTime, TextingStartTime;

    public virtual void TextLabelBuild(TextAsset text_label)
    {
        TextLabel = text_label;
        DialogNameText.text = "";
        DialogText.text = "";
        bool isendl = false;
        string tmps = "";
        foreach (var item in TextLabel.text)
        {
            switch (item)
            {
                case '£º':
                    NameTextList.Add(tmps);
                    tmps = "";
                    break;
                case '\n':
                    if (!isendl) TextList.Add(tmps);
                    isendl = !isendl;
                    tmps = "";
                    break;
                default:
                    tmps += item;
                    break;
            }
        }
    }

    public void OpenDialog()
    {
        if (!isDialogFinished) return;
        isDialogFinished = false;
        TextListIdx = TextListItemIdx = 0;
        DialogObj.SetActive(true);
    }

    protected virtual void Open()
    {
        DialogObj.SetActive(true);
    }

    protected virtual void Close()
    {
        DialogObj.SetActive(false);
    }

    protected virtual void DialogingText()
    {
        if (!DialogObj.activeSelf) return;
        if (TextListIdx >= TextList.Count)
        {
            Close();
            isDialogFinished = true;
            return;
        }
        DialogNameText.text = NameTextList[TextListIdx];
        if (InTextingTime < TextingTime)
        {
            InTextingTime += 10f * Time.deltaTime;
        }
        else
        {
            InTextingTime = 0;
            if (TextListItemIdx < TextList[TextListIdx].Length)
            {
                DialogText.text += TextList[TextListIdx][TextListItemIdx];
                TextListItemIdx++;
            }

        }
        isTextListFinshed = TextListItemIdx >= TextList[TextListIdx].Length;
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            if (isTextListFinshed)
            {
                TextListIdx++;
                TextListItemIdx = 0;
                DialogText.text = "";
                TextingTime = TextingStartTime;
            }
            else
            {
                TextingTime = 0;
            }
        }
    }

    protected virtual void OnClickDialogTextBtn()
    {

    }

    private void RandTextCreate()
    {

    }

}

public class BagControl : BagView
{
    
    public void AddBagItem(BagMode.Model.BagItem item)
    {
        switch (item.GetBagItemType)
        {
            case BagMode.Model.BagItem.BagItemType.Equip:
                BagItemEquipList.Add(item as Equip);
                break;
            case BagMode.Model.BagItem.BagItemType.Drug:
                BagItemDrugList.Add(item as Drug);
                break;
            case BagMode.Model.BagItem.BagItemType.Tool:
                BagItemToolList.Add(item as Tool);
                break;
            default:
                break;
        }
        BagItemOnView();
    }

    
    public override void Updata()
    {
        if (SteamVR_Actions.default_OnClickOpenMenu.GetState(SteamVR_Input_Sources.Any) && SteamVR_Actions.default_OnClickMoveUp.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Open();
        }
        if (SteamVR_Actions.default_OnClickBackBtn.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Close();
        }
        base.Updata();
    }

    public BagControl()
    { 
        BagItemClickImage = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemClickImage").GetComponent<Image>();
        BagItemClickImage.gameObject.SetActive(false);
        BagItemOnPointerEnterImage = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemOnPointerEnterImage").GetComponent<Image>();
        BagItemOnPointerEnterImage.gameObject.SetActive(false);
        GameObject bag_item_list_obj = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemList");
        for (int i = 0; i < bag_item_list_obj.transform.childCount; i++)
        {
            GameObject childobj = bag_item_list_obj.transform.GetChild(i).gameObject;
            GameObject childobjtext = childobj.transform.GetChild(1).gameObject;
            BagItemObjList.Add(childobj);
            BagItemMainPropTextList.Add(childobj.transform.GetChild(1).GetComponent<Text>());
            BagItemObjToIndexDic.Add(childobj, i);
            EventTrigger eventTrigger = childobj.GetComponent<EventTrigger>() ? childobj.GetComponent<EventTrigger>() : childobj.AddComponent<EventTrigger>();
            EventTrigger _eventTrigger = childobjtext.GetComponent<EventTrigger>() ? childobjtext.GetComponent<EventTrigger>() : childobjtext.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            EventTrigger.Entry entryClick = new EventTrigger.Entry();
            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryClick.eventID = EventTriggerType.PointerClick;
            entryExit.eventID = EventTriggerType.PointerExit;
            entryEnter.callback.AddListener((data) => { OnPointerEnterBagItem((PointerEventData)data); });
            entryExit.callback.AddListener((data) => { OnPointerExitBagItem((PointerEventData)data); });
            entryClick.callback.AddListener((data) => { OnPointerClickBagItem((PointerEventData)data); });
            eventTrigger.triggers.Add(entryEnter);
            eventTrigger.triggers.Add(entryExit);
            eventTrigger.triggers.Add(entryClick);
            _eventTrigger.triggers.Add(entryEnter);
            _eventTrigger.triggers.Add(entryExit);
            _eventTrigger.triggers.Add(entryClick);
        }
        RootOpenBtn = GameObject.Find("Canvas/BagUI/BagUIOpenBtn").GetComponent<Button>();
        RootCloseBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagUICloseBtn").GetComponent<Button>();
        BagItemEquipTypeBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemType/BagItemEquipTypeBtn").GetComponent<Button>();
        BagItemDrugTypeBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemType/BagItemDrugTypeBtn").GetComponent<Button>();
        BagItemToolTypeBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagItemType/BagItemToolTypeBtn").GetComponent<Button>();
        BagLastPageBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagTurnPage/BagLastPageBtn").GetComponent<Button>();
        BagNextPageBtn = GameObject.Find("Canvas/BagUI/BagUIOpen/BagTurnPage/BagNextPageBtn").GetComponent<Button>();
        RootOpenBtn.onClick.AddListener(Open);
        RootCloseBtn.onClick.AddListener(Close);
        BagItemEquipTypeBtn.onClick.AddListener(OnClickBagItemEquipTypeBtn);
        BagItemDrugTypeBtn.onClick.AddListener(OnClickBagItemDrugTypeBtn);
        BagItemToolTypeBtn.onClick.AddListener(OnClickBagItemToolTypeBtn);
        BagLastPageBtn.onClick.AddListener(OnClickBagLastPage);
        BagNextPageBtn.onClick.AddListener(OnClickBagNextPage);
        BagItemOnView();
        Root.gameObject.SetActive(false);
    }

    protected Button BagItemEquipTypeBtn, BagItemDrugTypeBtn, BagItemToolTypeBtn;

    protected Button BagLastPageBtn, BagNextPageBtn;

    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {        
        base.Open();
        BagItemTypeIndex = 0;
        BagItemListIndex = 0;
        BagItemOnPointerEnterImage.rectTransform.position = BagItemObjList[BagItemListIndex].GetComponent<RectTransform>().position;
        BagItemClickImage.rectTransform.position = BagItemObjList[BagItemListIndex].GetComponent<RectTransform>().position;
        BagItemOnPointerEnterImage.gameObject.SetActive(true);
        BagItemOnView();
    }
    protected override void BagItemOnView()
    {
        base.BagItemOnView();
    }

    private Image BagItemClickImage = null, BagItemOnPointerEnterImage = null;

    private void OnClickBagItemEquipTypeBtn()
    {
        BagItemTypeIndex = 0;
        BagItemOnView();
    }

    private void OnClickBagItemDrugTypeBtn()
    {
        BagItemTypeIndex = 1;
        BagItemOnView();
    }

    private void OnClickBagItemToolTypeBtn()
    {
        BagItemTypeIndex = 2;
        BagItemOnView();
    }

    private void OnClickBagLastPage()
    {
        BagItemObjListPageIndex--;
        BagItemObjListPageIndex = Mathf.Max(0, BagItemObjListPageIndex);
        BagItemOnView();
    }

    private void OnClickBagNextPage()
    {
        BagItemObjListPageIndex++;
        int allpage = BagItemEquipList.Count / BagItemObjList.Count;
        if (BagItemEquipList.Count % BagItemObjList.Count > 0) allpage++;
        BagItemObjListPageIndex = Mathf.Min(allpage - 1, BagItemObjListPageIndex);
        BagItemOnView();
    }

    private void OnPointerEnterBagItem(PointerEventData eventData)
    {
        string tmpname = "BagItemList";
        if (eventData.pointerEnter.transform.parent.name == tmpname || eventData.pointerEnter.transform.parent.parent.name == tmpname)
        {
            GameObject pointerobj = eventData.pointerEnter.transform.parent.name == tmpname ? eventData.pointerEnter : eventData.pointerEnter.transform.parent.gameObject;
            BagItemOnPointerEnterImage.transform.position = pointerobj.transform.position;
            BagItemOnPointerEnterImage.gameObject.SetActive(true);
        }
    }

    private void OnPointerExitBagItem(PointerEventData eventData)
    {
        BagItemOnPointerEnterImage.gameObject.SetActive(false);
    }

    private void OnPointerClickBagItem(PointerEventData eventData)
    {
        string tmpname = "BagItemList";
        if (eventData.pointerEnter.transform.parent.name == tmpname || eventData.pointerEnter.transform.parent.parent.name == tmpname)
        {
            GameObject pointerobj = eventData.pointerEnter.transform.parent.name == tmpname ? eventData.pointerEnter : eventData.pointerEnter.transform.parent.gameObject;
            BagItemClickImage.gameObject.SetActive(true);
            BagItemClickImage.rectTransform.position = pointerobj.GetComponent<RectTransform>().position;
            BagItemListIndex = BagItemObjListPageIndex * BagItemObjList.Count + BagItemObjToIndexDic[pointerobj];
            BagItemOnView();
        }
        
    }

    
}

public class TaskControl : TaskView
{

    public static Task TaskGiven = null;

    public void AddTask()
    {
        TaskList.Add(TaskGiven);
        TaskGiven = null;
    }


    public TaskControl()
    {
        Root = GameObject.Find("Canvas/TaskUI/TaskUIOpen").transform;
        RootOpenBtn = GameObject.Find("Canvas/TaskUI/TaskUIOpenBtn").GetComponent<Button>();
        RootCloseBtn = GameObject.Find("Canvas/TaskUI/TaskUIOpen/TaskUICloseBtn").GetComponent<Button>();
        TaskAcceptBtn = GameObject.Find("Canvas/TaskUI/TaskUIOpen/TaskAcceptBtn").GetComponent<Button>();
        RootOpenBtn.onClick.AddListener(Open);
        RootCloseBtn.onClick.AddListener(Close);
        TaskAcceptBtn.onClick.AddListener(OnClickTackAcceptBtn);
        Root.gameObject.SetActive(false);
    }

    public override void Updata()
    {
        if (SteamVR_Actions.default_OnClickOpenMenu.GetState(SteamVR_Input_Sources.Any) && SteamVR_Actions.default_OnClickMoveLeft.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Open();
        }
        if (SteamVR_Actions.default_OnClickBackBtn.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Close();
        }
        base.Updata();
    }


    protected Dictionary<Task, bool> TaskToBoolDic = new Dictionary<Task, bool>();

    protected int TaskItemListIndex = 0, TaskListIndex = 0;

    protected Button TaskAcceptBtn;

    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {
        base.Open();
    }

    private void OnClickTackAcceptBtn()
    {
        if (TaskItemListIndex * TaskItemList.Count + TaskItemListIndex >= TaskList.Count) return;
        TaskListIndex = TaskItemListIndex * TaskItemList.Count + TaskItemListIndex;
        if (!TaskToBoolDic[TaskList[TaskListIndex]])
        {
            TaskItemList[TaskItemListIndex].text = "Accepted";
        }
    }


}

public class ControlScript : MonoBehaviour
{

    public Transform test_enemy_trm; 

    protected Dialog dialog;

    protected BagControl bagControl;

    protected TaskControl taskControl;

    protected Player player;

    protected NpcModel tmpNpc;

    protected Enemy testenemy;

    

    private void Awake()
    {
        bagControl = new BagControl();
        taskControl = new TaskControl();
        player = new Player(GameObject.Find("Player").transform, GameObject.Find("PlayerCamera").GetComponent<Camera>());
        testenemy = new Enemy(test_enemy_trm);
        dialog = new Dialog(Resources.Load<TextAsset>("Text/GameTesting3_Text"), GameObject.Find("Canvas/DialogWindow"), 0.5f);
        tmpNpc = new NpcModel(GameObject.Find("NPC/Bar_NPC").transform, dialog);
    }

    private void Start()
    {

        for (int i = 0; i < 55; i++)
        {
            foreach (BagItem.BagItemValue value in System.Enum.GetValues(typeof(BagItem.BagItemValue)))
            {
                foreach (Equip.EquipType type in System.Enum.GetValues(typeof(Equip.EquipType)))
                {
                    Equip equip = new Equip(null, value, type, Random.Range(10, 100));
                    bagControl.AddBagItem(equip);
                }
                foreach (Drug.DrugType type in System.Enum.GetValues(typeof(Drug.DrugType)))
                {
                    Drug drug = new Drug(null, value, type, Random.Range(10, 100), Random.Range(5, 15));
                    bagControl.AddBagItem(drug);
                }
                foreach (Tool.ToolType type in System.Enum.GetValues(typeof(Tool.ToolType)))
                {
                    Tool tool = new Tool(null, value, type);
                    bagControl.AddBagItem(tool);
                }
            }

        }
    }

    private void Update()
    {

        player.Updata();
        bagControl.Updata();
        taskControl.Updata();
        tmpNpc.Updata();
        testenemy.Updata();
        //testenemy.Updata();
    }

}