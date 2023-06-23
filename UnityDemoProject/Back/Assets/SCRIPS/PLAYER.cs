using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PLAYER : MonoBehaviour
{
    public GameObject Player;
    public GameObject shield;
    public GameObject bullet;
    public GameObject sbullet;
    public Transform gun;
    public Slider EXPUI;
    public float speed;
    public float force;
    public int ATK;
    public int DEF;
    private int XDEF;
    public int EXP = 0;
    public int maxEXP;
    public int LEVEL;
    public int junmpcount = 0;
    public int demp;
    public bool ismove = true;
    public bool isshield = false;
    public bool ishurted = false;
    // Start is called before the first frame update
    void Start()
    {
        ATK = LEVEL * 100;
        DEF = LEVEL * 100;
        Player.GetComponent<PLAYERHP>().maxHP = LEVEL * 100;
        Player.GetComponent<PLAYERMP>().maxMP = LEVEL * 100;
    }
    public void playermove()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            transform.localScale = new Vector3(-1, 1, 1);
            GetComponent<Animator>().SetBool("ISRUN", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            transform.localScale = new Vector3(1, 1, 1);
            GetComponent<Animator>().SetBool("ISRUN", true);
        }
        if (Input.GetKeyDown(KeyCode.K)&&(junmpcount<=1&&junmpcount>=0))
        {
            shield.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, force));
            GetComponent<Animator>().SetBool("ISGRASSRUN", true);
            junmpcount++;
        }
    }
    public void playerattack()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            shield.SetActive(true);
            XDEF = DEF;
            DEF = 10 * DEF;
            isshield = true;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            shield.SetActive(false);
            DEF = XDEF;
            isshield = false;
        }
        if(isshield==false)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (transform.localScale == new Vector3(1, 1, 1))
                {
                    bullet.GetComponent<bulletmove>().isright = true;
                }
                if (transform.localScale == new Vector3(-1, 1, 1))
                {
                    bullet.GetComponent<bulletmove>().isright = false;
                }
                GameObject mbullet = Instantiate(bullet, gun.position, gun.rotation);
                bullet.GetComponent<bulletmove>().ATK = ATK;
            }
            if(Player.GetComponent<PLAYERMP>().MP>0)
            {
                if(Input.GetKeyDown(KeyCode.I))
                {
                    if(transform.localScale==new Vector3(1,1,1))
                    {
                        sbullet.GetComponent<BULLETMOVE2>().isright = true;
                    }
                    if(transform.localScale==new Vector3(-1,1,1))
                    {
                        sbullet.GetComponent<BULLETMOVE2>().isright = false;
                    }
                    Player.GetComponent<PLAYERMP>().deMP(demp);
                    GameObject msbullet = Instantiate(sbullet, gun.position, gun.rotation);
                    sbullet.GetComponent<BULLETMOVE2>().ATK = 2 * ATK;
                }
            }
        }
    }
    public void playerlevelup()
    {
        if (EXP < maxEXP) EXP += LEVEL * 100;
        if(EXP>=maxEXP)
        {
            LEVEL += 1;
            maxEXP += maxEXP / 2;
            Player.GetComponent<PLAYERHP>().maxHP += Player.GetComponent<PLAYERHP>().maxHP / 5;
            Player.GetComponent<PLAYERHP>().HP = Player.GetComponent<PLAYERHP>().maxHP;
            Player.GetComponent<PLAYERMP>().maxMP += Player.GetComponent<PLAYERMP>().maxMP / 5;
            Player.GetComponent<PLAYERMP>().MP = Player.GetComponent<PLAYERMP>().maxMP;
            ATK = ATK + (LEVEL + ATK) / 4;
            DEF = DEF + (LEVEL + DEF) / 6;
            XDEF = DEF;
            EXP = 0;
        }
    }
    public void playerhurted()
    {
        if(ishurted==true)
        {
            if(transform.localScale==new Vector3(1,1,1))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-force * 0.6f, force));
                GetComponent<Animator>().SetBool("ISHURTED", true);
            }
            if (transform.localScale == new Vector3(-1, 1, 1))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(force * 0.6f, force));
                GetComponent<Animator>().SetBool("ISHURTED", true);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GRASS") 
        { 
            junmpcount = 0;
            GetComponent<Animator>().SetBool("ISHURTED", false);
        }
        if(collision.gameObject.tag=="ENEMY")
        {
            int Pdehp = collision.gameObject.GetComponent<ENEMY>().ATK - DEF;
            if (Pdehp < 0) Pdehp = 0;
            if (isshield == true)
            {
                if (Pdehp > 0)
                {
                    ishurted = true;
                    playerhurted();
                }
                else ishurted = false;
            }
            if (isshield == false)
            {
                ishurted = true;
                playerhurted();
            }
            Player.GetComponent<PLAYERHP>().deHP(Pdehp);

        }
    }
    // Update is called once per frame
    void Update()
    {
        //GetComponent<Animator>().SetBool("ISHURTED", false);
        GetComponent<Animator>().SetBool("ISRUN", false);
        GetComponent<Animator>().SetBool("ISGRASSRUN", false);
        EXPUI.value = (float)EXP / (float)maxEXP;
        if(ismove==true)
        {
            playermove();
            playerattack();
        }
        ishurted = false;
        //if (Input.GetKeyDown(KeyCode.Space)) playerlevelup();
    }
}
