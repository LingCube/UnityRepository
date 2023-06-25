using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Player_Script : MonoBehaviour
{
    public Sprite[] Player_Static_Sprite = new Sprite[4];
    public Sprite[] Game_Tower_Sprite = new Sprite[4];
    public Image Player_Static_Image;
    public Image Player_Static_Change_Image;
    public bool player_st = true;
    public static int Player_ATK;
    public Animator Player_anim;


    //判断方向键是否按下
    bool[] Key_Btn = new bool[4];



    public GameObject Tower_Par;
    public GameObject Tower;


    //Player Sp Point Control
    public static int Player_Sp_Point = 0;



    //玩家技能控制
    float Player_maxSp = 100;
    public static float Player_Sp = 100;
    int Player_de_Sp;
    float Player_Sp_cd;
    float Player_Sp_cd_speed;
    float Player_Sp_cd_decnt;
    public Text Player_maxSp_text;
    public Text Player_Sp_text;
    public Image Player_maxSp_image;
    public Image Player_Sp_image;
    public Text[] Player_Sp_cd_text = new Text[4];
    public Image[] Player_Sp_cd_image = new Image[4];



    // Start is called before the first frame update
    void Start()
    {
        Player_Playing_Start();

    }


    //玩家状态初始化
    public void Player_Playing_Start()
    {

        Player_anim = GetComponent<Animator>();

        //初始化玩家位置
        transform.position = new Vector2(1, 3);

        //初始化玩家属性
        player_st = true;
        Player_Static_Image.sprite = Player_Static_Sprite[0];
        Player_Sp_image.sprite = Player_Static_Sprite[1];
        Player_Static_Change_Image.fillAmount = 0;
        Player_ATK = 100;
        Player_Sp_Point = 0;
        Player_de_Sp = 25;
        Player_Sp_cd_decnt = 1;
        Player_maxSp = 100;
        Player_Sp = Player_maxSp;
        Player_maxSp_image.fillAmount = 1;
        Player_Sp_image.fillAmount = 1;
        Player_maxSp_text.text = Player_maxSp.ToString();
        Player_Sp_text.text = Player_Sp.ToString();

        //初始化玩家移动状态
        for (int i = 0; i < 4; i++)
        {
            Key_Btn[i] = false;
            Player_Sp_cd_image[i].fillAmount = 0;
        }

        //初始化所有防御塔状态
        for (int i = 0; i < Tower_Par.transform.childCount; i++)
        {
            Destroy(Tower_Par.transform.GetChild(i).gameObject);
        }

    }


    //Player Move   玩家移动
    public void Player_Move(Vector2 Start_Pos)
    {
        //Keycode to control move   玩家按键移动的控制
        if (Input.GetKey(KeyCode.A) && Key_Btn[1] == false && Key_Btn[2] == false && Key_Btn[3] == false)
        {
            Key_Btn[0] = true;
            transform.position -= new Vector3(2 * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(-0.1f, 0.1f, 1);
            Player_anim.SetBool("Game_Player_isRun", true);
        }
        else if (Input.GetKey(KeyCode.D) && Key_Btn[0] == false && Key_Btn[2] == false && Key_Btn[3] == false)
        {
            Key_Btn[1] = true;
            transform.position += new Vector3(2 * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(0.1f, 0.1f, 1);
            Player_anim.SetBool("Game_Player_isRun", true);
        }
        else if (Input.GetKey(KeyCode.W) && Key_Btn[0] == false && Key_Btn[1] == false && Key_Btn[3] == false)
        {
            Key_Btn[2] = true;
            transform.position += new Vector3(0, 2 * Time.deltaTime, 0);
            Player_anim.SetBool("Game_Player_isRun", true);
        }
        else if (Input.GetKey(KeyCode.S) && Key_Btn[0] == false && Key_Btn[1] == false && Key_Btn[2] == false)
        {
            Key_Btn[3] = true;
            transform.position -= new Vector3(0, 2 * Time.deltaTime, 0);
            Player_anim.SetBool("Game_Player_isRun", true);
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {

            Player_anim.SetBool("Game_Player_isRun", false);

            //Position to int     将当前位置转化为int
            int xpos = (int)transform.position.x;
            int ypos = (int)transform.position.y;

            //Move Direction     判断当前移动方向
            bool xtar = transform.position.x - Start_Pos.x >= 0 ? true : false;
            bool ytar = transform.position.y - Start_Pos.y >= 0 ? true : false;

            //Distance between now pos and int_pos     判断转化后的位置与当前位置的距离
            float xlen = transform.position.x - xpos >= 0 ? transform.position.x - xpos : xpos - transform.position.x;
            float ylen = transform.position.y - ypos >= 0 ? transform.position.y - ypos : ypos - transform.position.y;

            //Position Control     控制玩家位置
            if (xlen > 0.6f)
            {
                if (xtar) xpos++;
                else xpos--;
            }
            if (ylen > 0.6f)
            {
                if (ytar) ypos++;
                else ypos--;
            }

            transform.position = new Vector2(xpos, ypos);

            //return to start static     返回按键初始状态
            for (int i = 0; i < 4; i++)
            {
                Key_Btn[i] = false;
            }
        }

        //Position limit     玩家移动限制
        if (transform.position.x <= 1) transform.position = new Vector2(1, transform.position.y);
        if (transform.position.x >= GameControl_Scripts.x_Terrain_Org)
        {
            transform.position = new Vector2(GameControl_Scripts.x_Terrain_Org, transform.position.y);
        }
        if (transform.position.y <= 1) transform.position = new Vector2(transform.position.x, 1);
        if (transform.position.y >= GameControl_Scripts.y_Terrain_Org)
        {
            transform.position = new Vector2(transform.position.x, GameControl_Scripts.y_Terrain_Org);
        }
    }



    //Tower Build 防御塔建造
    public void Sp_Create(int xtower_pos, int ytower_pos, int de_sp, int sp_index)
    {
        if (GameControl_Scripts.Terrain_Org[xtower_pos, ytower_pos] <= 1
            && Player_Sp_cd_image[sp_index - 1].fillAmount == 0 && Player_Sp >= Player_de_Sp)
        {
            Player_Sp -= de_sp;
            Player_Sp_cd_image[sp_index - 1].fillAmount = 1;
            GameObject tower = Instantiate(Tower, Tower_Par.transform);
            tower.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = Game_Tower_Sprite[sp_index - 1];
            tower.GetComponent<Tower_Script>().Tower_Type = sp_index;
            tower.transform.position = new Vector2(xtower_pos, ytower_pos);
            GameControl_Scripts.Terrain_Org[xtower_pos, ytower_pos] = 2;
            Player_Sp_image.fillAmount = Player_Sp / Player_maxSp;
        }

    }


    //Player Sp Point Change   玩家技能点变化
    //1，2 maxSp变化；3，4 技能消耗变化；5，6 技能cd变化
    public void Player_Sp_Point_Change(int sp_point_index)
    {
        if (Player_Sp_Point <= 0 && sp_point_index % 2 != 0)
        {
            return;
        }

        

        if (sp_point_index % 2 != 0)
        {
            Player_Sp_Point--;
        }
        else
        {
            Player_Sp_Point++;
        }


        switch (sp_point_index)
        {
            case 1:
                Player_maxSp += (int)Player_maxSp / 20;
                break;
            case 2:
                if (Player_maxSp > 100)
                {
                    Player_maxSp -= (int)Player_maxSp / 20;
                }
                break;
            case 3:
                if (Player_de_Sp > 15)
                {
                    Player_de_Sp -= 2;
                }
                break;
            case 4:
                if (Player_de_Sp < 25)
                {
                    Player_de_Sp += 2;
                }
                break;
            case 5:
                if (Player_Sp_cd_decnt > 0)
                {
                    Player_Sp_cd_decnt -= 0.1f;
                }
                break;
            case 6:
                if (Player_Sp_cd_decnt < 1)
                {
                    Player_Sp_cd_decnt += 0.1f;
                }
                break;
            default:
                break;
        }
        return;
    }



    // Update is called once per frame
    void Update()
    {

        if (!GameControl_Scripts.Game_isStart)
        {
            return;
        }


        if (Player_Sp >= Player_maxSp)
        {
            Player_Sp = Player_maxSp;
        }
        if (Player_Sp <= 0)
        {
            Player_Sp = 0;
        }
        if (Player_Sp_image.fillAmount < 1)
        {
            Player_Sp += 0.02f * Time.deltaTime;
            Player_Sp_image.fillAmount = Player_Sp / Player_maxSp;
        }
        Player_maxSp_text.text = Player_maxSp.ToString();
        Player_Sp_text.text = (Player_Sp_image.fillAmount * Player_maxSp).ToString("#0");

        //Player to move   玩家移动，并获取移动前的位置
        Vector2 Start_Pos = transform.position;
        Player_Move(Start_Pos);
        

        //st to control   玩家状态变换
        if (Input.GetKeyDown(KeyCode.Space) && Player_Static_Change_Image.fillAmount == 0)
        {
            player_st = player_st == true ? false : true;
            Player_Static_Image.sprite = player_st == true ? Player_Static_Sprite[0] : Player_Static_Sprite[2];
            Player_Sp_image.sprite = player_st == true ? Player_Static_Sprite[1] : Player_Static_Sprite[3];
            Player_Static_Change_Image.fillAmount = 1;
        }
        if (Player_Static_Change_Image.fillAmount > 0)
        {
            Player_Static_Change_Image.fillAmount -= 0.5f * Time.deltaTime;
        }


        //sp to control   玩家技能控制
        int xtower_pos = (int)transform.position.x;
        int ytower_pos = (int)transform.position.y;
        if (player_st)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Sp_Create(xtower_pos, ytower_pos, Player_de_Sp, 1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Sp_Create(xtower_pos, ytower_pos, Player_de_Sp, 2);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Sp_Create(xtower_pos, ytower_pos, Player_de_Sp, 3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Sp_Create(xtower_pos, ytower_pos, Player_de_Sp, 4);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (Player_Sp_cd_image[i].fillAmount > 0)
            {
                switch (i)
                {
                    case 0:
                        Player_Sp_cd = 5f * Player_Sp_cd_decnt;
                        break;
                    case 1:
                        Player_Sp_cd = 10f * Player_Sp_cd_decnt;
                        break;
                    case 2:
                        Player_Sp_cd = 7f * Player_Sp_cd_decnt;
                        break;
                    case 3:
                        Player_Sp_cd = 10f * Player_Sp_cd_decnt;
                        break;
                }
                Player_Sp_cd_speed = 1 / Player_Sp_cd;
                Player_Sp_cd_image[i].fillAmount -= Player_Sp_cd_speed * Time.deltaTime;
                Player_Sp_cd_text[i].text = (Player_Sp_cd_image[i].fillAmount * Player_Sp_cd).ToString("#0.0");
            }
            else
            {
                Player_Sp_cd_text[i].text = " ";
            }
        }

    }
}
