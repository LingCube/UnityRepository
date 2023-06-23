using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ENEMYHP : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public Image Hp;
    public Image MaxHP;
    public int HP;
    public int maxHP;
    public bool islevelup = true;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        Hp.gameObject.SetActive(false);
        MaxHP.gameObject.SetActive(false);
    }
    public void deHP(int Edehp)
    {
        if (HP <= maxHP) HP -= Edehp;
        else Edehp = 0;
    }
    public void levelup()
    {
        if(enemy.activeSelf==false&&islevelup)
        {
            player.GetComponent<PLAYER>().playerlevelup();
            islevelup = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        Hp.fillAmount = (float)HP / (float)maxHP;
        if(HP<=0)
        {
            HP = 0;
            enemy.SetActive(false);
            MaxHP.gameObject.SetActive(false);
        }
        if (HP > maxHP) HP = maxHP;
        levelup();
    }
}
