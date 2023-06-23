using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PLAYERHP : MonoBehaviour
{
    public GameObject Player;
    public GameObject gameover;
    public Image hpfill;
    public Image Hp;
    public Sprite Hp1;
    public Sprite Hp2;
    public int HP;
    public int maxHP;
    // Start is called before the first frame update
    void Start()
    {
        HP=maxHP;
        Hp.sprite = Hp2;
    }
    public void HpSpriteChange()
    {
        if (HP <= maxHP / 2) Hp.sprite = Hp1;
        else Hp.sprite = Hp2;
    }
    public void inHP(int Pinhp)
    {
        if (HP <= maxHP) HP += Pinhp;
        else Pinhp = 0;
    }
    public void deHP(int Pdehp)
    {
        if (HP <= maxHP) HP -= Pdehp;
        else Pdehp = 0;
    }
    public void Playerdie()
    {
        if(HP<=0)
        {
            Player.SetActive(false);
            gameover.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (HP < 0) HP = 0;
        if (HP > maxHP) HP = maxHP;
        hpfill.fillAmount = (float)HP / (float)maxHP;
        HpSpriteChange();
        Playerdie();
    }
}
