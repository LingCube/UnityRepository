using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PLAYERUI : MonoBehaviour
{
    public GameObject playerUI;
    public Transform player;
    public Transform playerui;
    public Text LEVEL;
    public Text HP;
    public Text MAXHP;
    public Text MP;
    public Text MAXMP;
    public Text ATK;
    public Text DEF;
    public Text EXP;
    public Text MAXEXP;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void playerUIshow()
    {
        Time.timeScale = 0;
        playerUI.SetActive(true);
    }
    public void playerUIfalse()
    {
        Time.timeScale = 1;
        playerUI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        LEVEL.text = player.GetComponent<PLAYER>().LEVEL.ToString();
        HP.text = playerui.GetComponent<PLAYERHP>().HP.ToString();
        MAXHP.text = playerui.GetComponent<PLAYERHP>().maxHP.ToString();
        MP.text = playerui.GetComponent<PLAYERMP>().MP.ToString();
        MAXMP.text = playerui.GetComponent<PLAYERMP>().maxMP.ToString();
        ATK.text = player.GetComponent<PLAYER>().ATK.ToString();
        DEF.text = player.GetComponent<PLAYER>().DEF.ToString();
        EXP.text = player.GetComponent<PLAYER>().EXP.ToString();
        MAXEXP.text = player.GetComponent<PLAYER>().maxEXP.ToString();
    }
}
