using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using UI;

public class Global
{

}


public class ControlScript : MonoBehaviour
{

    protected SetWindow SetWin;

    protected HomePageWindow HomePageWin;

    protected ShopWindow ShopWin;

    protected GateWindow GateWin;

    private void Awake()
    {
        SetWin = new SetWindow(GameObject.Find("Canvas/SetWindow").transform, GameObject.Find("Canvas/SetWinBtn").GetComponent<Button>());
        HomePageWin = new HomePageWindow(GameObject.Find("Canvas/HomePageWin").transform,
            GameObject.Find("Canvas/HomePageWin/GameStartBtn").GetComponent<Button>(),
            GameObject.Find("Canvas/HomePageWin/SetWinBtn").GetComponent<Button>(), SetWin);
        ShopWin = new ShopWindow(GameObject.Find("Canvas/ShopWindow/ShopWin").transform,
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/ShopItemChoiceType").transform,
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/PlaneItemType").transform,
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/UnPlaneItemType").transform,
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/ShopItemPage").transform,
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/UnPlaneItemType/ChoiceImage").GetComponent<Image>(),
            GameObject.Find("Canvas/ShopWindow/ShopWinBtn").GetComponent<Button>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/UpgradeBtn").GetComponent<Button>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/StartBtn").GetComponent<Button>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/AdBtn").GetComponent<Button>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/PlaneItemType/PlaneItemText").GetComponent<Text>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/UnPlaneItemType/UnPlaneItemText").GetComponent<Text>(),
            GameObject.Find("Canvas/ShopWindow/ShopWin/ShopWinPage/ShopItemExplain/ShopItemExplainText").GetComponent<Text>());
        GateWin = new GateWindow(GameObject.Find("Canvas/GateWin").transform, GameObject.Find("Canvas/GateWin/GatePage").transform,
            GameObject.Find("Canvas/GateWin/GatePage/Viewport/Content").transform, new List<Sprite>(Resources.LoadAll<Sprite>("GateWin")));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShopWin.Updata();
        GateWin.Updata();
    }
}
