using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DataStruct;


namespace Model
{

    public class Character
    {
        public enum CharacterType
        {
            Player,
            Npc,
            Enemy
        }

        public Transform GetRoot => Root;

        public Character(Transform root)
        {
            if (!root) return;
            Root = root;
        }

        public virtual void Updata()
        {
            if (!Root) return;
        }

        protected Transform Root;

        protected CharacterType ChType;

        protected virtual void Move()
        {

        }
    }

    public class Equip
    {
        public enum EquipType
        {
            Head,
            Armor,
            Wingman,
            Bullet
        }

        public Equip()
        {

        }

        protected EquipType EpType;
    }
}

namespace Window
{
    public class BaseWin
    {
        public Transform GetRoot => Root;

        public BaseWin(Transform root)
        {
            if (!root) return;
            Root = root;
        }

        public virtual void Updata()
        {
            if (!Root) return;
            if (!Root.gameObject.activeSelf) return;
        }

        protected Transform Root;

        protected virtual void Open()
        {
            Root.gameObject.SetActive(true);
        }

        protected virtual void Close()
        {
            Root.gameObject.SetActive(false);
        }
    }
}

namespace UI
{
    public class Judge
    {

        public static bool IsMouseOnPos(RectTransform rect)
            => (Input.touchCount > 0 && Input.GetTouch(0).position.x <= rect.position.x + rect.rect.width / 2 || Input.mousePosition.x <= rect.position.x + rect.rect.width / 2)
            && (Input.touchCount > 0 && Input.GetTouch(0).position.x >= rect.position.x - rect.rect.width / 2 || Input.mousePosition.x >= rect.position.x - rect.rect.width / 2)
            && (Input.touchCount > 0 && Input.GetTouch(0).position.y <= rect.position.y + rect.rect.height / 2 || Input.mousePosition.y <= rect.position.y + rect.rect.height / 2)
            && (Input.touchCount > 0 && Input.GetTouch(0).position.y >= rect.position.y - rect.rect.height / 2 || Input.mousePosition.y >= rect.position.y - rect.rect.height / 2);

    }

    public class ScrollList
    {

        public bool Horizontal = true, Vertical = true;

        public ScrollList(Transform root, Transform content, bool horizontal = true, bool vertical = true)
        {
            if (!root || !content) return;
            Root = root;
            RectRoot = Root.GetComponent<RectTransform>();
            ScrollRoot = Root.GetComponent<ScrollRect>();
            HeightRoot = RectRoot.rect.height;
            Content = content;
            RectContent = Content.GetComponent<RectTransform>();
            ContentPos = RectContent.localPosition;
            for (int i = 0; i < Content.childCount; i++)
            {
                RectTransform tmp_child = Content.GetChild(i).GetComponent<RectTransform>();
                RectContentItemList.Add(tmp_child);
                ScrollItemDeque.push_back(tmp_child);
            }
            Horizontal = horizontal;
            Vertical = vertical;
            LItemIdx = 0;
            if (ScrollItemDeque.empty()) return;
            XFront = ScrollItemDeque.front().position.x;
            XBack = ScrollItemDeque.back().position.x;
            YFront = ScrollItemDeque.front().position.y;
            YBack = ScrollItemDeque.back().position.y;
            switch(Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                    PlatformTy = PlatformType.Window;
                    XDeltaPos = Input.GetAxis("Mouse X") * 100f;
                    YDeltaPos = Input.GetAxis("Mouse Y") * 100f;
                    break;
                case RuntimePlatform.OSXPlayer:
                    PlatformTy = PlatformType.IOS;
                    XDeltaPos = Input.GetAxis("Mouse X") * 100f;
                    YDeltaPos = Input.GetAxis("Mouse Y") * 100f;
                    break;
                case RuntimePlatform.Android:
                    PlatformTy = PlatformType.Android;
                    XDeltaPos = Input.touchCount > 0 ? Input.GetTouch(0).deltaPosition.x : 0;
                    YDeltaPos = Input.touchCount > 0 ? Input.GetTouch(0).deltaPosition.y : 0;
                    break;
                default:
                    break;
            }
        }

        public virtual void Updata()
        {
            if (!Root || !Content) return;
            if (!Root.gameObject.activeSelf) return;
            ScrollItemTurn();
            ScrollItemMove();
        }

        protected enum PlatformType
        {
            Window,
            IOS,
            Android
        }

        protected PlatformType PlatformTy;

        protected Transform Root;

        protected Transform Content;

        protected ScrollRect ScrollRoot;

        protected RectTransform RectRoot;

        protected RectTransform RectContent;

        protected List<RectTransform> RectContentItemList = new List<RectTransform>();

        protected Deque<RectTransform> ScrollItemDeque = new Deque<RectTransform>();

        protected Vector3 ContentPos;

        protected float XDeltaPos, YDeltaPos;

        protected float XFront = 0, XBack = 0, YFront = 0, YBack = 0, Hitem = 0, HeightRoot = 0;

        protected bool IsClickContent = false;

        protected bool IsCreate = true;

        protected int cnt = 0;

        protected int LItemIdx = 0, RItemIdx = int.MaxValue;

        protected virtual void ScrollItemChange(bool dir)
        {

        }

        protected virtual void ScrollItemTurn()
        {
            if (ScrollItemDeque.empty()) return;
            float w_item = ScrollItemDeque.back().rect.width, h_item = ScrollItemDeque.front().rect.height;

            // Up and Left Push
            if (ScrollItemDeque.front().position.y < YFront || ScrollItemDeque.front().position.x > XFront)
            {
                RectTransform rect = ScrollItemDeque.back();
                ScrollItemDeque.pop_back();
                rect.position = ScrollItemDeque.front().position + new Vector3(Horizontal ? -w_item : 0, Vertical ? h_item : 0, 0);
                ScrollItemDeque.push_front(rect);
                ScrollItemChange(true);
            }

            // Up and Left Pop
            if (ScrollItemDeque.front().position.y >= YFront + h_item || ScrollItemDeque.front().position.x <= XFront - w_item)
            {
                var item = ScrollItemDeque.front();
                item.position = ScrollItemDeque.back().position + new Vector3(Horizontal ? w_item : 0, Vertical ? -h_item : 0, 0);
                ScrollItemDeque.pop_front();
                ScrollItemDeque.push_back(item);
                ScrollItemChange(false);
            }

            // Back and Right Push
            if (ScrollItemDeque.back().position.y > YBack || ScrollItemDeque.back().position.x < XBack)
            {
                RectTransform rect = ScrollItemDeque.front();
                ScrollItemDeque.pop_front();
                rect.position = ScrollItemDeque.back().position + new Vector3(Horizontal ? w_item : 0, Vertical ? -h_item : 0, 0);
                ScrollItemDeque.push_back(rect);
                ScrollItemChange(false);
            }

            // Back and Right Pop
            if (ScrollItemDeque.back().position.y <= YBack - h_item || ScrollItemDeque.back().position.x >= XBack + w_item)
            {
                var item = ScrollItemDeque.back();
                item.position = ScrollItemDeque.front().position + new Vector3(Horizontal ? -w_item : 0, Vertical ? h_item : 0, 0);
                ScrollItemDeque.pop_back();
                ScrollItemDeque.push_front(item);
                ScrollItemChange(true);
            }
        }

        protected virtual void ScrollItemMove()
        {
            if (ScrollItemDeque.empty()) return;
            for (int i = 0; i < Content.childCount; i++)
            {
                if (!Content.GetChild(i).gameObject.activeSelf) return;
            }
            switch(PlatformTy)
            {
                case PlatformType.Window:
                case PlatformType.IOS:
                    if (Input.GetKeyDown(KeyCode.Mouse0) && Judge.IsMouseOnPos(RectContent)) IsClickContent = true;
                    if (Input.GetKeyUp(KeyCode.Mouse0)) IsClickContent = false;
                    XDeltaPos = Input.GetAxis("Mouse X") * 100f;
                    YDeltaPos = Input.GetAxis("Mouse Y") * 100f;
                    break;
                case PlatformType.Android:
                    IsClickContent = Judge.IsMouseOnPos(RectContent);
                    XDeltaPos = Input.touchCount > 0 ? Input.GetTouch(0).deltaPosition.x : 0;
                    YDeltaPos = Input.touchCount > 0 ? Input.GetTouch(0).deltaPosition.y : 0;
                    break;
            }
            
            foreach (var item in RectContentItemList)
            {
                if (!IsClickContent) break;
                item.position += new Vector3(Horizontal ? XDeltaPos : 0, Vertical ? YDeltaPos : 0, 0);
            }
        }
    }
}
