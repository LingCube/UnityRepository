using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using UI;
using Window;
using DataStruct;


public class HomePageWindow : BaseWin
{

    public HomePageWindow(Transform root, Button game_start_btn, Button set_win_btn, SetWindow set_win) : base(root)
    {
        if (!game_start_btn) return;
        if (!set_win_btn || !set_win.GetRoot) return;
        GameStartBtn = game_start_btn;
        GameStartBtn.onClick.AddListener(OnClickGameStart);
        SetWinBtn = set_win_btn;
        SetWin = set_win;
        SetWinBtn.onClick.AddListener(OnClickSetWinOpen);
    }

    public override void Updata()
    {
        base.Updata();
    }

    protected SetWindow SetWin;

    protected Button GameStartBtn, SetWinBtn;

    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {
        base.Open();
        Root.SetAsLastSibling();
    }

    protected void OnClickGameStart()
    {
        Close();
        Time.timeScale = 1;
    }

    protected void OnClickSetWinOpen()
    {
        if (!Root.gameObject.activeSelf) return;
        if (!SetWin.GetRoot) return;
        SetWin.GetRoot.gameObject.SetActive(true);
        SetWin.GetRoot.SetAsLastSibling();
        Time.timeScale = 0;
    }

}

public class SetWindow : BaseWin
{
    public SetWindow(Transform root, Button open_btn = null) : base(root)
    {
        MusicBtn = Root.GetChild(0).GetComponent<Button>();
        BackgroundMusicBtn = Root.GetChild(1).GetComponent<Button>();
        CloseBtn = Root.GetChild(2).GetComponent<Button>();
        BtnSprites[0] = CloseBtn.image.sprite;
        BtnSprites[1] = MusicBtn.image.sprite;
        MusicBtn.onClick.AddListener(OnClickMusicBtn);
        BackgroundMusicBtn.onClick.AddListener(OnClickBackgroundMusic);
        CloseBtn.onClick.AddListener(Close);
        if (open_btn)
        {
            OpenBtn = open_btn;
            OpenBtn.onClick.AddListener(Open);
        }
        Close();
    }

    public override void Updata()
    {
        base.Updata();
    }


    protected Button MusicBtn;

    protected Button BackgroundMusicBtn;

    protected Button OpenBtn, CloseBtn;

    protected Sprite[] BtnSprites = new Sprite[2];

    protected override void Open()
    {
        base.Open();
        Root.SetAsLastSibling();
        Time.timeScale = 0;
    }

    protected override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }

    protected void OnClickMusicBtn()
    {
        if (!Root.gameObject.activeSelf) return;
        int idx = MusicBtn.image.sprite == BtnSprites[0] ? 0 : 1;
        MusicBtn.image.sprite = BtnSprites[Mathf.Abs(idx - 1)];
        //±‡–¥“Ù¿÷¬ﬂº≠        
    }

    protected void OnClickBackgroundMusic()
    {
        if (!Root.gameObject.activeSelf) return;
        int idx = BackgroundMusicBtn.image.sprite == BtnSprites[0] ? 0 : 1;
        BackgroundMusicBtn.image.sprite = BtnSprites[Mathf.Abs(idx - 1)];
        //±‡–¥“Ù¿÷¬ﬂº≠
    }

    protected void OnClickShareGame()
    {
        if (!Root.gameObject.activeSelf) return;
        //±‡–¥∑÷œÌ¬ﬂº≠
    }

    protected void OnClickGameCircle()
    {
        if (!Root.gameObject.activeSelf) return;
        //±‡–¥”Œœ∑»¶¬ﬂº≠
    }

    protected void OnClickGameCirclePicture()
    {
        if (!Root.gameObject.activeSelf) return;
        //±‡–¥”Œœ∑»¶Õº∆¨¬ﬂº≠
    }

}

public class ShopWindow : BaseWin
{
    public ShopWindow(Transform root, Transform choice_type_trm, Transform choice_plane_trm, Transform un_choice_plane_trm,
        Transform choice_plane_page_trm, Image choice_img, Button shop_open_btn, Button upgrade_btn, Button start_btn,
        Button ad_btn, Text plane_choice_text, Text un_plane_choice_text, Text shop_item_explain_text) : base(root)
    {
        if (!choice_type_trm || !choice_plane_trm || !un_choice_plane_trm || !choice_plane_page_trm) return;
        if (!choice_img || !shop_open_btn || !upgrade_btn || !start_btn) return;
        if (!plane_choice_text || !un_plane_choice_text || !shop_item_explain_text) return;
        for (int i = 0; i < choice_type_trm.childCount; i++)
        {
            Button btn = choice_type_trm.GetChild(i).GetComponent<Button>();
            ShopChoiceRectList.Add(btn.image.rectTransform);
            ShopChoiceTextList.Add(btn.GetComponentInChildren<Text>());
            btn.onClick.AddListener(OnClickChoiceShopEquipTypeBtn);
        }
        ChoiceTypeColor = ShopChoiceTextList[0].color;
        UnChoiceTypeColor = ShopChoiceTextList[^1].color;
        for (int i = 0; i < choice_plane_trm.childCount - 1; i++)
        {
            PlaneChoiceRectList.Add(choice_plane_trm.GetChild(i).GetComponent<RectTransform>());
        }
        PlaneChoiceText = choice_plane_trm.GetChild(choice_plane_trm.childCount - 1).GetComponent<Text>();
        for (int i = 0; i < un_choice_plane_trm.childCount - 2; i++)
        {
            UnPlaneChoiceImageList.Add(un_choice_plane_trm.GetChild(i).GetChild(0).GetComponent<Image>());
        }
        UnPlaneChoiceText = un_choice_plane_trm.GetChild(un_choice_plane_trm.childCount - 2).GetComponent<Text>();
        ChoicePlaneLastPageBtn = choice_plane_page_trm.GetChild(0).GetComponent<Button>();
        ChoicePlaneLastPageBtn.onClick.AddListener(OnClickChoiceLastPage);
        ChoicePlaneNextPageBtn = choice_plane_page_trm.GetChild(1).GetComponent<Button>();
        ChoicePlaneNextPageBtn.onClick.AddListener(OnClickChoiceNextPage);
        ChoiceImage = choice_img;
        shop_open_btn.onClick.AddListener(Open);
        UpgradeBtn = upgrade_btn;
        UpgradeBtn.onClick.AddListener(OnClickEquipUpgrade);
        StartBtn = start_btn;
        StartBtn.onClick.AddListener(Close);
        PlaneChoiceText = plane_choice_text;
        UnPlaneChoiceText = un_plane_choice_text;
        ShopItemExplainText = shop_item_explain_text;
        ArmorSpriteList = new List<Sprite>(Resources.LoadAll<Sprite>("ShopWin/ArmorItemType"));
        WingmanSpriteList = new List<Sprite>(Resources.LoadAll<Sprite>("ShopWin/WingmanItemType"));
        BulletSpriteList = new List<Sprite>(Resources.LoadAll<Sprite>("ShopWin/BulletItemType"));
        ChoiceSpriteList = new List<Sprite>(Resources.LoadAll<Sprite>("ShopWin/ChoiceSprite"));
        TextAsset[] plane_text_label = Resources.LoadAll<TextAsset>("ShopWin/PlaneItemType");
        for (int i = 0; i < plane_text_label.Length; i++) PlaneTextList.Add(plane_text_label[i].text);
        TextAsset[] armor_text_label = Resources.LoadAll<TextAsset>("ShopWin/ArmorItemType");
        for (int i = 0; i < armor_text_label.Length; i++) ArmorTextList.Add(armor_text_label[i].text);
        TextAsset[] wingman_text_label = Resources.LoadAll<TextAsset>("ShopWin/WingmanItemType");
        for (int i = 0; i < wingman_text_label.Length; i++) WingmanTextList.Add(wingman_text_label[i].text);
        TextAsset[] bullet_text_label = Resources.LoadAll<TextAsset>("ShopWin/BulletItemType");
        for (int i = 0; i < bullet_text_label.Length; i++) BulletTextList.Add(bullet_text_label[i].text);
        ChoiceType = ShopEquipChoiceType.PlaneUpgrade;
        ShopChoiceTypeBegin();
    }

    public override void Updata()
    {
        base.Updata();
        ShopChoiceTypeUpdata();
    }

    protected enum ShopEquipChoiceType
    {
        PlaneUpgrade,
        ArmorUpgrade,
        WingmanUpgrade,
        BulletUpgrade
    }

    protected ShopEquipChoiceType ChoiceType;

    protected int ShopChoiceTypeIdx = 0;

    protected Color ChoiceTypeColor, UnChoiceTypeColor;

    protected Image ChoiceImage;

    protected Button ChoicePlaneLastPageBtn, ChoicePlaneNextPageBtn;

    protected Button UpgradeBtn;

    protected Button StartBtn;

    protected Button AdBtn;

    protected Text PlaneChoiceText, UnPlaneChoiceText;

    protected Text ShopItemExplainText;

    protected List<RectTransform> ShopChoiceRectList = new List<RectTransform>();

    protected List<Text> ShopChoiceTextList = new List<Text>();

    protected List<RectTransform> PlaneChoiceRectList = new List<RectTransform>();

    protected List<Image> UnPlaneChoiceImageList = new List<Image>();

    protected List<Sprite> ArmorSpriteList = new List<Sprite>();

    protected List<Sprite> WingmanSpriteList = new List<Sprite>();

    protected List<Sprite> BulletSpriteList = new List<Sprite>();

    protected List<Sprite> ChoiceSpriteList = new List<Sprite>();

    protected List<string> PlaneTextList = new List<string>();

    protected List<string> ArmorTextList = new List<string>();

    protected List<string> WingmanTextList = new List<string>();

    protected List<string> BulletTextList = new List<string>();

    protected override void Open()
    {
        base.Open();
        Root.parent.SetAsLastSibling();
    }

    protected override void Close()
    {
        base.Close();
    }

    protected void ShopChoiceTypeBegin()
    {
        if (!Root.gameObject.activeSelf) return;
        ShopChoiceTypeIdx = 0;
        for (int i = 0; i < UnPlaneChoiceImageList.Count; i++)
        {
            UnPlaneChoiceImageList[i].gameObject.SetActive(true);
            UnPlaneChoiceImageList[i].transform.parent.gameObject.SetActive(true);
            UnPlaneChoiceText.gameObject.SetActive(true);
            PlaneChoiceRectList[i].gameObject.SetActive(false);
            PlaneChoiceText.gameObject.SetActive(false);
            switch (ChoiceType)
            {
                case ShopEquipChoiceType.PlaneUpgrade:
                    PlaneChoiceRectList[i].gameObject.SetActive(true);
                    PlaneChoiceText.gameObject.SetActive(true);
                    UnPlaneChoiceImageList[i].gameObject.SetActive(false);
                    UnPlaneChoiceImageList[i].transform.parent.gameObject.SetActive(false);
                    UnPlaneChoiceText.gameObject.SetActive(false);
                    break;
                case ShopEquipChoiceType.ArmorUpgrade:
                    UnPlaneChoiceImageList[i].sprite = ArmorSpriteList[i];
                    break;
                case ShopEquipChoiceType.WingmanUpgrade:
                    UnPlaneChoiceImageList[i].sprite = WingmanSpriteList[i];
                    break;
                case ShopEquipChoiceType.BulletUpgrade:
                    UnPlaneChoiceImageList[i].sprite = BulletSpriteList[i];
                    break;
                default:
                    break;
            }
        }
        if (ChoiceType == ShopEquipChoiceType.PlaneUpgrade)
        {
            ChoicePlaneLastPageBtn.gameObject.SetActive(true);
            ChoicePlaneNextPageBtn.gameObject.SetActive(true);
            ChoiceImage.gameObject.SetActive(false);
            return;
        }
        ChoicePlaneLastPageBtn.gameObject.SetActive(false);
        ChoicePlaneNextPageBtn.gameObject.SetActive(false);
        ChoiceImage.gameObject.SetActive(true);
        ChoiceImage.rectTransform.position = UnPlaneChoiceImageList[0].rectTransform.position;
        ChoiceImage.sprite = ChoiceType == ShopEquipChoiceType.ArmorUpgrade ? ChoiceSpriteList[1] : ChoiceSpriteList[0];
    }

    protected void ShopChoiceTypeUpdata()
    {
        if (!Root.gameObject.activeSelf) return;
        switch (ChoiceType)
        {
            case ShopEquipChoiceType.PlaneUpgrade:
                ShopItemExplainText.text = PlaneTextList[ShopChoiceTypeIdx];
                break;
            case ShopEquipChoiceType.ArmorUpgrade:
                ShopItemExplainText.text = ArmorTextList[ShopChoiceTypeIdx];
                break;
            case ShopEquipChoiceType.WingmanUpgrade:
                ShopItemExplainText.text = WingmanTextList[ShopChoiceTypeIdx];
                break;
            case ShopEquipChoiceType.BulletUpgrade:
                ShopItemExplainText.text = BulletTextList[ShopChoiceTypeIdx];
                break;
            default:
                break;
        }
        if (ChoiceType == ShopEquipChoiceType.PlaneUpgrade)
        {
            foreach (RectTransform rect in PlaneChoiceRectList)
            {
                rect.gameObject.SetActive(rect == PlaneChoiceRectList[ShopChoiceTypeIdx]);
            }
            return;
        }
        for (int i = 0; i < UnPlaneChoiceImageList.Count; i++)
        {
            if (!Judge.IsMouseOnPos(UnPlaneChoiceImageList[i].rectTransform)) continue;
            ShopChoiceTypeIdx = i;
            ChoiceImage.rectTransform.position = UnPlaneChoiceImageList[i].rectTransform.position;
            break;
        }
    }

    protected void OnClickChoiceShopEquipTypeBtn()
    {
        if (!Root.gameObject.activeSelf) return;
        for (int i = 0; i < ShopChoiceRectList.Count; i++)
        {
            ShopChoiceTextList[i].color = UnChoiceTypeColor;
            if (!Judge.IsMouseOnPos(ShopChoiceRectList[i])) continue;
            switch (i)
            {
                case 0:
                    ChoiceType = ShopEquipChoiceType.PlaneUpgrade;
                    break;
                case 1:
                    ChoiceType = ShopEquipChoiceType.ArmorUpgrade;
                    break;
                case 2:
                    ChoiceType = ShopEquipChoiceType.WingmanUpgrade;
                    break;
                case 3:
                    ChoiceType = ShopEquipChoiceType.BulletUpgrade;
                    break;
                default:
                    break;
            }
            ShopChoiceTextList[i].color = ChoiceTypeColor;
        }
        ShopChoiceTypeBegin();
    }

    protected void OnClickChoiceLastPage() => ShopChoiceTypeIdx = Root.gameObject.activeSelf ? Mathf.Max(0, --ShopChoiceTypeIdx) : 0;

    protected void OnClickChoiceNextPage() => ShopChoiceTypeIdx = Root.gameObject.activeSelf ? Mathf.Min(++ShopChoiceTypeIdx, 2) : 0;

    protected void OnClickEquipUpgrade()
    {
        if (!Root.gameObject.activeSelf) return;
    }

    protected void OnClickAdBtn()
    {
        //±‡–¥π„∏Ê¬ﬂº≠
    }
}

public class GateWindow : BaseWin
{
    public GateWindow(Transform root, Transform gate_scroll_root, Transform gate_scroll_content, List<Sprite> planet_sprite_list) : base(root)
    {
        if (!gate_scroll_root || !gate_scroll_content) return;
        Deque<List<Image>> planet_image_list_deque = new Deque<List<Image>>();
        for (int i = 0; i < gate_scroll_content.childCount; i++)
        {
            List<Image> planet_image_list = new List<Image>();
            for (int j = 0; j < gate_scroll_content.GetChild(i).childCount; j++)
            {
                Image image = gate_scroll_content.GetChild(i).GetChild(j).GetComponent<Image>();
                if (!image) continue;
                planet_image_list.Add(image);
            }
            planet_image_list_deque.push_back(planet_image_list);
        }
        GateScroll = new GateScrollList(gate_scroll_root, gate_scroll_content, planet_image_list_deque, planet_sprite_list, false, true);
    }

    public override void Updata()
    {
        base.Updata();
        GateScroll.Updata();
    }

    protected GateScrollList GateScroll;

    protected override void Close()
    {
        base.Close();
    }

    protected override void Open()
    {
        base.Open();
    }

    protected class GateScrollList : ScrollList
    {

        public GateScrollList(Transform root, Transform content,Deque<List<Image>> planet_image_list_deque, List<Sprite> planet_sprite_list,
            bool horizontal = true, bool vertical = true) : base(root, content, horizontal, vertical)
        {
            if (planet_image_list_deque == null || planet_sprite_list == null) return;
            PlanetImageListDeque = planet_image_list_deque;
            PlanetSpriteList = planet_sprite_list;
            LItemIdx = 0;
            RItemIdx = planet_image_list_deque.size() * planet_image_list_deque.front().Count - 1;
            //PlanetSpriteList = new List<Sprite>(Resources.LoadAll<Sprite>("GateWin"));
        }

        public override void Updata()
        {
            base.Updata();
        }

        protected Deque<List<Image>> PlanetImageListDeque = new Deque<List<Image>>();

        protected List<Sprite> PlanetSpriteList = new List<Sprite>();

        protected Dictionary<Sprite, int> PlanetSpriteToIndexDic = new Dictionary<Sprite, int>();
        
        protected override void ScrollItemChange(bool dir)
        {
            base.ScrollItemChange(dir);
            if (PlanetImageListDeque == null || PlanetSpriteList == null) return;
            List<Image> planet_image_list = dir ? PlanetImageListDeque.back() : PlanetImageListDeque.front();
            for (int i = 0; i < planet_image_list.Count; i++)
            {
                planet_image_list[i].sprite = PlanetSpriteList[LItemIdx];
            }
            switch(dir)
            {
                case true:
                    PlanetImageListDeque.pop_back();
                    PlanetImageListDeque.push_front(planet_image_list);
                    break;
                case false:
                    PlanetImageListDeque.pop_front();
                    PlanetImageListDeque.push_back(planet_image_list);
                    break;
            }
        }
        
        protected override void ScrollItemMove()
        {
            base.ScrollItemMove();
        }

        protected override void ScrollItemTurn()
        {
            base.ScrollItemTurn();
        }

        
    }
}
