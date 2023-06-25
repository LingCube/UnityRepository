using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameControl_Scripts : MonoBehaviour
{

    //地图大小
    public static int[,] Terrain_Org = new int[105,105];
    public static int x_Terrain_Org = 15;
    public static int y_Terrain_Org = 5;
    public GameObject Terrain;
    public GameObject Terrains;
    public Sprite[] Terrain_Sprites = new Sprite[2];
    bool Terrain_Sprite_Change = true;


    //玩家设置
    public GameObject Game_Player;
    public Player_Script Game_Player_Script;


    //怪物设置
    public static bool[] Game_Enemies_Array = new bool[1005];
    public static int Game_Enemies_Cnt;
    public static int Game_Enemies_everyCnt;
    public static int Game_Enemies_lastCnt;
    public static int Game_Enemy_index;
    float Game_Enemy_everyCd_Time = 1f;
    float Game_Enemy_everyCd_Speed = 0.1f;
    float Game_Enemy_Cd_Time = 0;
    float Game_Enemy_maxCd_Time = 3f;

    public GameObject Enemies;
    public GameObject Enemy;


    //游戏状态查看
    public static bool Game_isStart = false;
    public static int Game_Playing_Index = 0;
    public Text Game_Array_Text;


    //游戏界面UI
    public Sprite[] Game_UI_Sprite = new Sprite[6];
    public GameObject Game_Start_Obj;
    public GameObject Game_Pause_Meun;


    //游戏结果
    public static int Game_Res_Cnt = 3;
    public static int Game_Res = 0;
    public static bool Game_isPause = false;
    public GameObject Game_Res_Obj;
    public Text Game_Res_Text;

    


    // Start is called before the first frame update
    void Start()
    {

        //初始化避免额外开销
        Game_Player_Script = Game_Player.GetComponent<Player_Script>();

        Time.timeScale = 0;

        Game_Start_Obj.SetActive(true);
        

    }



    //控制游戏开始状态
    public void Game_Start_Control(int game_index)
    {

        //初始化游戏结果
        Game_Res = 0;
        Game_Res_Cnt = game_index == 0 ? 3 : 1;
        Game_Res_Obj.SetActive(false);

        //Create Terrain   重置地形

        for(int i = 0; i < Terrains.transform.childCount; i++)
        {
            Destroy(Terrains.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < x_Terrain_Org + 2; i++)
        {
            for (int j = 0; j < y_Terrain_Org + 2; j++)
            {
                GameObject terrain = Instantiate(Terrain, Terrains.transform);
                Terrain_Org[i, j] = 1;
                terrain.transform.position = new Vector3(i, j + 0.2f, 2f);
                terrain.transform.localScale = new Vector3(0.65f, 0.65f, 1);
                terrain.GetComponent<SpriteRenderer>().sprite = Terrain_Sprite_Change ? Terrain_Sprites[0] : Terrain_Sprites[1];
                Terrain_Sprite_Change = Terrain_Sprite_Change == true ? false : true;
            }
        }


        //重置玩家状态

        Game_Player_Script.Player_Playing_Start();


        //Create Enemy   重置敌人

        for(int i = 0; i < Enemies.transform.childCount; i++)
        {
            Destroy(Enemies.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < 1005; i++)
        {
            GameControl_Scripts.Game_Enemies_Array[i] = false;
        }

        Game_Enemies_everyCnt = 4;
        Game_Enemies_lastCnt = 0;
        Game_Enemy_index = 1;
        Game_Enemy_everyCd_Time = 0;
        Game_Enemy_Cd_Time = 0;
        Game_Enemies_Array[0] = true;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject enemy = Instantiate(Enemy, Enemies.transform);
                int yenemy_pos = Random.Range(1, 6);
                enemy.transform.position = new Vector3(x_Terrain_Org + 2, yenemy_pos, 0);
                Terrain_Org[x_Terrain_Org + 2, yenemy_pos] *= 3;
                enemy.GetComponent<Enemy_Scripts>().Enemy_index = (i + 1) * (j + 1);
                enemy.GetComponent<Enemy_Scripts>().Enemy_isMove = false;
                Game_Enemies_Cnt++;
            }
        }

        Game_isStart = true;

    }



    //控制游戏开始、重开、暂停、返回菜单、退出
    public void Game_Playing_Control_Btn(int btn_index)
    {
        //0为开始游戏、1为重新开始、2为暂停、3为返回菜单、4为退出游戏
        switch(btn_index)
        {
            case 0:
                Time.timeScale = 1;
                Game_Start_Obj.SetActive(false);
                Game_Start_Control(Game_Playing_Index);
                break;
            case 1:
                Time.timeScale = 1;
                Game_Start_Control(Game_Playing_Index);
                Game_Pause_Meun.SetActive(false);
                break;
            case 2:
                if (!Game_isPause)
                {
                    Game_Pause_Meun.GetComponentInParent<Image>().sprite = Game_UI_Sprite[3];
                    Game_Pause_Meun.SetActive(true);
                    Time.timeScale = 0;
                    Game_isPause = true;
                }
                else
                {
                    Game_Pause_Meun.GetComponentInParent<Image>().sprite = Game_UI_Sprite[0];
                    Game_Pause_Meun.SetActive(false);
                    Time.timeScale = 1;
                    Game_isPause = false;
                }
                break;
            case 3:
                Time.timeScale = 0;
                SceneManager.LoadScene(0);
                break;
            case 4:
                Application.Quit();
                break;
        }
    }

    

    //Enemies Pool   创建怪物池
    public void Game_Enemies_Pool()
    {
        //上一波次怪结束
        if (Game_Enemies_lastCnt == 0)
        {
            Game_Enemies_everyCnt++;
            Game_Enemies_lastCnt = Game_Enemies_everyCnt;
            if (Game_Enemy_Cd_Time == 0)
            {
                Game_Enemy_Cd_Time = Game_Enemy_maxCd_Time;
                Game_Enemy_everyCd_Speed += 0.05f;  
                Game_Enemy_maxCd_Time -= 0.5f;
            }
        }
        //计时
        if (Game_Enemy_Cd_Time > 0)
        {
            Game_Enemy_Cd_Time -= 0.5f * Time.deltaTime;
        }
        //计时结束，当前波次怪开始
        if (Game_Enemy_Cd_Time <= 0)
        {
            Game_Enemy_Cd_Time = 0;
            //每个怪出怪间隔
            if (Game_Enemy_everyCd_Time > 0)
            {
                Game_Enemy_everyCd_Time -= Game_Enemy_everyCd_Speed * Time.deltaTime;
            }
            if (Game_Enemies_Array[Game_Enemy_index - 1] && Game_Enemy_everyCd_Time <= 0)
            {
                Game_Enemies_Array[Game_Enemy_index] = true;
                Game_Enemy_index++;
                Game_Enemy_everyCd_Time = 1f;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        //判断游戏是否开始
        if(!Game_isStart)
        {
            return;
        }


        //让整个游戏状态以数组形式显示
        Game_Array_Text.text = "";
        for (int i = y_Terrain_Org + 1; i >= 0; i--)     
        {
            for (int j = 0; j < x_Terrain_Org + 3; j++)  
            {
                Game_Array_Text.text += Terrain_Org[j, i].ToString();
                Game_Array_Text.text += " ";
            }
            Game_Array_Text.text += '\n';
        }
        Game_Array_Text.text += "Game Enemy Count: " + Game_Enemies_Cnt;

        //Game Enemies Control
        Game_Enemies_Pool();


        
        //Game Pause   按Esc暂停
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Game_Playing_Control_Btn(2);
        }


        //Game result judgment   判断游戏是否失败
        if (Game_Enemies_Cnt <= 0)
        {
            Game_Enemies_Cnt = 0;
            Game_Res = 1;
        }
        else
        {
            for (int i = 1; i <= 5; i++)
            {
                if (Game_Res_Cnt <= 0)
                {
                    Game_Res_Cnt = 0;
                    Game_Res = -1;
                    break;
                }
                if (Terrain_Org[0, i] % 3 == 0)
                {
                    Game_Res_Cnt--;
                    Terrain_Org[0, i] /= 3;
                }
            }
        }
        


        //Game Result Control   游戏结果控制，1为游戏胜利，0为游戏正在进行，-1为游戏失败
        switch(Game_Res)
        {
            case 0:     
                Game_Res_Obj.SetActive(false);
                break;
            case 1:
                Time.timeScale = 0;
                Game_Res_Obj.SetActive(true);
                Game_Res_Text.text = "Game Success";
                Game_Playing_Index++;
                Game_isStart = false;
                break;
            case -1:
                Time.timeScale = 0;
                Game_Res_Obj.SetActive(true);
                Game_Res_Text.text = "Game Fail.";
                Game_isStart = false;
                break;
            default:
                break;
        }
    }
}
