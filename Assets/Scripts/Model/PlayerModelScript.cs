using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStruct;
using CharacterMode.Model;
using CharacterMode.ICharacter;
using Valve.VR;

public class Player : Character, ICharacterProp
{

    public static bool IsHpChanged = false;

    public static float DeHp = 0;

    public int GetLevel => Level;

    public int GetMaxLevel => MaxLevel;

    public int GetExp => Exp;

    public int GetAtk => Atk;

    public int GetOrgAtk => OrgAtk;

    public int GetOnAtk => OnAtk;

    public int GetDef => Def;

    public int GetOrgDef => OrgDef;

    public int GetOnDef => OnDef;

    public float GetHp => Hp;

    public float GetMaxHp => MaxHp;

    public float GetOrgMaxHp => OrgMaxHp;

    public float GetOnMaxHp => OnMaxHp;

    public float GetMp => Mp;

    public float GetMaxMp => MaxMp;

    public float GetOrgMaxMp => OrgMaxMp;

    public float GetOnMaxMp => OnMaxMp;

    public Equip GetAtkEquip => AtkEquip;

    public Equip GetDefEquip => DefEquip;

    public Equip GetHpEquip => HpEquip;

    public Equip GetMpEquip => MpEquip;

    public Player(Transform _root, Camera _camera) : base(_root, _camera)
    {
        PlayerAni = CharacterRoot.GetComponent<Animation>();
    }

    public override void Awake()
    {
        base.Awake();
        if (CharacterRoot.GetComponent<Animation>())
        {
            PlayerAni = CharacterRoot.GetComponent<Animation>();
            foreach(AnimationState aniState in PlayerAni)
            {
                PlayerAniClipList.Add(aniState.clip);
            }
        }
        if (!CharacterCamera.transform.GetComponent<SteamVR_Camera>())
        {
            CharacterCamera.gameObject.AddComponent<SteamVR_Camera>();
        }
        OrgMaxHp = OrgMaxMp = 100;
        OrgAtk = OrgDef = 100;
        OnAtk = OnDef = 0;
        Level = 0;
        Exp = 0;
        MaxExp = 500;
        AtkDrugTime = DefDrugTime = HpDrugTime = MpDrugTime = new Pair<Pair<float, float>, int>(new Pair<float, float>(0, 0), 0);
        PropControl();
        Hp = MaxHp;
        Mp = MaxMp;
    }

    public override void Updata()
    {
        if (IsHpChanged)
        {
            HpControl(DeHp);
            DeHp = 0;
            IsHpChanged = false;
        }

        if (Hp <= 0) return;
        base.Updata();
        PlayerAttack();
    }

    public void PropControl()
    {
        if (Exp >= MaxExp && Level < MaxLevel)
        {
            Level++;
            OrgMaxHp += OrgMaxHp / 10;
            OrgMaxMp += OrgMaxMp / 10;
            Hp = MaxHp = OrgMaxHp + OnMaxHp;
            Mp = MaxMp = OrgMaxMp + OnMaxMp;
            Exp %= MaxExp;
            MaxExp += MaxExp / 10;
        }
        Level = Mathf.Clamp(Level, 0, MaxLevel);
        MaxHp = OrgMaxHp + OnMaxHp;
        MaxMp = OrgMaxMp + OnMaxMp;
        Atk = OrgAtk + OnAtk;
        Def = OrgDef + OnDef;
        Hp = Mathf.Clamp(Hp, 0, MaxHp);
        Mp = Mathf.Clamp(Mp, 0, MaxMp);
    }

    public Equip AddEquip(Equip equip)
    {
        Equip Eq = null;
        switch (equip.GetEquipType)
        {
            case Equip.EquipType.ATK:
                OnAtk += equip.GetEffectNum - (AtkEquip == null ? 0 : AtkEquip.GetEffectNum);
                Eq = AtkEquip;
                AtkEquip = equip;
                break;
            case Equip.EquipType.DEF:
                OnDef += equip.GetEffectNum - (DefEquip == null ? 0 : DefEquip.GetEffectNum);
                Eq = DefEquip;
                DefEquip = equip;
                break;
            case Equip.EquipType.HP:
                OnMaxHp += equip.GetEffectNum - (HpEquip == null ? 0 : HpEquip.GetEffectNum);
                Eq = HpEquip;
                HpEquip = equip;
                break;
            case Equip.EquipType.MP:
                Eq = MpEquip;
                MpEquip = equip;
                OnMaxMp += equip.GetEffectNum;
                break;
            default:
                break;
        }
        PropControl();
        return Eq;
    }

    public Equip DelEquip(Equip.EquipType type)
    {
        Equip Eq = null;
        switch (type)
        {
            case Equip.EquipType.ATK:
                Eq = AtkEquip;
                OnAtk -= AtkEquip.GetEffectNum;
                break;
            case Equip.EquipType.DEF:
                Eq = DefEquip;
                OnDef -= DefEquip.GetEffectNum;
                break;
            case Equip.EquipType.HP:
                Eq = HpEquip;
                OnMaxHp -= HpEquip.GetEffectNum;
                break;
            case Equip.EquipType.MP:
                Eq = MpEquip;
                OnMaxMp -= MpEquip.GetEffectNum;
                break;
            default:
                break;
        }
        PropControl();
        return Eq;
    }

    public void EatDrug(Drug drug)
    {
        switch (drug.GetDrugType)
        {
            case Drug.DrugType.ATK:
                break;
            case Drug.DrugType.DEF:
                break;
            case Drug.DrugType.HP:
                break;
            case Drug.DrugType.MP:
                break;
            default:
                break;
        }
        PropControl();
    }

    public void HpControl(float dehp)
    {
        Hp -= dehp;
        PropControl();
    }

    protected Animation PlayerAni;

    protected List<AnimationClip> PlayerAniClipList = new List<AnimationClip>();

    protected Equip AtkEquip = null, DefEquip = null, HpEquip = null, MpEquip = null;

    protected Pair<Pair<float, float>, int> AtkDrugTime, DefDrugTime, HpDrugTime, MpDrugTime;

    protected SteamVR_Camera CharacterCameraVR;

    protected override void CharacterCameraControl()
    {
        base.CharacterCameraControl();
    }

    protected virtual void VR_CharacterCameraControl()
    {
        SteamVR_Action_Vector2 vec = SteamVR_Actions.default_TurnRotationVector2;
    }

    protected override void CharacterMove()
    {
        if (!IsMove) return;
        //base.CharacterMove();
        if (Input.GetKey(KeyCode.W) || SteamVR_Actions.default_OnClickMoveUp.GetState(SteamVR_Input_Sources.Any)) 
        {
            Vector3 wtar = Vector3.Cross(CharacterRoot.up, -CharacterCamera.transform.right);
            CharacterTar += wtar - new Vector3(0, wtar.y, 0);
        }
        if (Input.GetKey(KeyCode.S) || SteamVR_Actions.default_OnClickMoveDown.GetState(SteamVR_Input_Sources.Any)) 
        {
            Vector3 star = Vector3.Cross(CharacterRoot.up, CharacterCamera.transform.right);
            CharacterTar += star - new Vector3(0, star.y, 0);
        }
        if (Input.GetKey(KeyCode.A) || SteamVR_Actions.default_OnClickMoveLeft.GetState(SteamVR_Input_Sources.Any))
        {
            Vector3 atar = Vector3.Cross(CharacterRoot.up, -CharacterCamera.transform.forward);
            CharacterTar += atar - new Vector3(0, atar.y, 0);
        }
        if (Input.GetKey(KeyCode.D) || SteamVR_Actions.default_OnClickMoveRight.GetState(SteamVR_Input_Sources.Any))
        {
            Vector3 dtar = Vector3.Cross(CharacterRoot.up, CharacterCamera.transform.forward);
            CharacterTar += dtar - new Vector3(0, dtar.y, 0);
        }
        CharacterRoot.LookAt(CharacterTar / 2 + CharacterRoot.position);
        CharacterRoot.Translate((CharacterTar == Vector3.zero ? 0 : CharacterMoveSpeed) * Time.deltaTime * Vector3.forward);
        if (CharacterTar == Vector3.zero)
        {
            if (PlayerAniClipList.Count > 0)
            {
                PlayerAni.Play(PlayerAniClipList[0].name);
            }
        }
        else
        {
            if (PlayerAniClipList.Count > 2)
            {
                PlayerAni.Play(PlayerAniClipList[2].name);
            }
        }
        CharacterTar = Vector3.zero;
    }

    protected override void HideCharacterCameraTransformControl()
    {
        base.HideCharacterCameraTransformControl();
    }

    private bool IsMove = true;

    private int Level, MaxLevel = 80;

    private int Exp, MaxExp;

    private int Atk, OrgAtk, OnAtk, OrgDef, OnDef, Def;

    private float Hp, MaxHp, OrgMaxHp, OnMaxHp;

    private float Mp, MaxMp, OrgMaxMp, OnMaxMp;

    private void PlayerAttack()
    {
        if (PlayerAniClipList.Count < 5) return;
        if (PlayerAni.IsPlaying(PlayerAniClipList[3].name) || PlayerAni.IsPlaying(PlayerAniClipList[4].name))
        {
            return; 
        }
        IsMove = true;
        if (Input.GetMouseButtonDown(0))
        {
            IsMove = false;
            PlayerAni.Play(PlayerAniClipList[3].name);
        }
        if (Input.GetMouseButtonDown(1))
        {
            IsMove = false;
            PlayerAni.Play(PlayerAniClipList[4].name);
        }
    }

}