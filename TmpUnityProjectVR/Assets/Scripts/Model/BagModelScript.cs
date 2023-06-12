using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStruct;
using BagMode.Model;
using BagMode.IBag;

public class Equip : BagItem, IBagItem
{

    public int GetLevel => Level;

    public int GetExp => Exp;

    public int GetEffectNum => EffectNum;

    public EquipType GetEquipType => EqType;

    public enum EquipType
    {
        ATK,
        DEF,
        HP,
        MP
    }

    public Equip(Transform _root, BagItemValue value, EquipType eqtype, int effect_num) : base(_root, value)
    {
        Type = BagItemType.Equip;
        EqType = eqtype;
        EffectNum = effect_num;
        Level = 0;
    }

    void IBagItem.Effect()
    {

    }

    private int Level, MaxLevel = 80;

    private int Exp, MaxExp;

    private int EffectNum;

    private EquipType EqType;

}

public class Drug : BagMode.Model.BagItem, IBagItem
{

    public DrugType GetDrugType => DgType;

    public int GetEffectNum => EffectNum;

    public int GetCnt => Cnt;

    public float GetEffectTime => EffectTime;

    public enum DrugType
    {
        ATK,
        DEF,
        HP,
        MP
    }

    public Drug()
    {
        Type = BagItemType.Drug;
    }

    public Drug(Transform _root, BagItemValue value, DrugType dgtype, int effect_num, float effect_time) : base(_root, value)
    {
        Type = BagItemType.Drug;
        DgType = dgtype;
        EffectNum = effect_num;
        EffectTime = effect_time;
    }

    void IBagItem.Effect()
    {
        throw new System.NotImplementedException();
    }

    private DrugType DgType;

    private int EffectNum, Cnt = 0;

    private float EffectTime;

}

public class Tool : BagMode.Model.BagItem, IBagItem
{

    public int GetCnt => Cnt;


    public enum ToolType
    {
        Exp,
        Task,
    }

    public Tool(Transform _root, BagItemValue value, ToolType _type) : base(_root, value)
    {
        Type = BagItemType.Tool;
        TlType = _type;
    }

    void IBagItem.Effect()
    {

    }

    private ToolType TlType;

    private int Cnt = 0;

}