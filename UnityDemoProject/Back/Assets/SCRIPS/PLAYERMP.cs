using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PLAYERMP : MonoBehaviour
{
    public Image mpfill;
    public Image Mp;
    public Sprite Mp1;
    public Sprite Mp2;
    public int MP;
    public int maxMP;
    // Start is called before the first frame update
    void Start()
    {
        MP = maxMP;
    }
    public void MpSpriteChange()
    {
        if (MP <= maxMP / 2) Mp.sprite = Mp1;
        else Mp.sprite = Mp2;
    }
    public void inMP(int Pinmp)
    {
        if (MP <= maxMP) MP += Pinmp;
        else Pinmp = 0;
    }
    public void deMP(int Pdemp)
    {
        if (MP <= maxMP) MP -= Pdemp;
        else Pdemp = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (MP < 0) MP = 0;
        if (MP > maxMP) MP = maxMP;
        mpfill.fillAmount = (float)MP / (float)maxMP;
        MpSpriteChange();
    }
}
