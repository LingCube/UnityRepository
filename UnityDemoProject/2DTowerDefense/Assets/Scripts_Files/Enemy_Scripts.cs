using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scripts : MonoBehaviour
{

    public int Enemy_type = 1;

    public int Enemy_maxHp = 1000;
    public int Enemy_Hp;



    public bool Enemy_isMove = true;
    public int Enemy_index;


    float Enemy_Stop_cd = 2f;

    public float Enemy_Speed = 1f;
    private float Enemy_xSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Enemy_Hp = Enemy_maxHp;
        Enemy_xSpeed = Enemy_Speed;
        Enemy_type = Random.Range(1, 5);
        Enemy_isMove = false;
    }


    //Enemy position initialize   敌人位置初始化
    public void Enemy_Pos_Initialize(int enemy_xpos, int enemy_ypos, int enemy_initialize_num)
    {
        while (GameControl_Scripts.Terrain_Org[enemy_xpos, enemy_ypos] % enemy_initialize_num == 0)
        {
            GameControl_Scripts.Terrain_Org[enemy_xpos, enemy_ypos] /= enemy_initialize_num;
        }
    }



    // Update is called once per frame
    void Update()
    {

        if (!GameControl_Scripts.Game_isStart)
        {
            return;
        }

        if(!GameControl_Scripts.Game_Enemies_Array[Enemy_index])
        {
            return;
        }


        if (Enemy_Hp >= Enemy_maxHp) Enemy_Hp = Enemy_maxHp;        

        int Start_exPos = (int)transform.position.x;
        transform.position -= new Vector3(Enemy_Speed * Time.deltaTime, 0, 0);
        int End_exPos = (int)transform.position.x;
        int End_eyPos = (int)transform.position.y;
        if (Start_exPos != End_exPos)  
        {
            GameControl_Scripts.Terrain_Org[Start_exPos, (int)transform.position.y] /= 3;
            GameControl_Scripts.Terrain_Org[End_exPos, (int)transform.position.y] *= 3;
        }

        //Enemy Hp change 
        // 3为敌人存在未被攻击   5为被 防御塔1 攻击   7和11为被 防御塔2 攻击  13为被 防御塔3 攻击  17为被 防御塔4 攻击

        // 被防御塔1攻击
        if (GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 5 == 0)  
        {
            Enemy_Hp -= Player_Script.Player_ATK;
            GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 5;
        }

        // 被防御塔2攻击
        if (GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 7 == 0 && GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 11 == 0) 
        {
            Enemy_Hp -= Player_Script.Player_ATK * 5;
            GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 7;
            GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 11;
        }

        // 被防御塔3攻击
        if (GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 13 == 0)
        {
            Enemy_Speed /= 2;
            Enemy_Stop_cd = 1.5f;
            Enemy_Hp -= Player_Script.Player_ATK / 10 * 8;
            GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 13;
        }

        // 被防御塔4攻击
        if (GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 17 == 0)
        {
            Enemy_Speed = 0;
            Enemy_Stop_cd = 2.5f;
            Enemy_Hp -= Player_Script.Player_ATK / 10;
            GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 17;
        }


        // 敌人速度变化
        if (Enemy_Speed != Enemy_xSpeed && Enemy_Stop_cd > 0)  
        {
            Enemy_Stop_cd -= 1f * Time.deltaTime;
        }
        else
        {
            Enemy_Speed=Enemy_xSpeed;
        }



        // 敌人死亡
        if (Enemy_Hp <= 0)
        {
            Enemy_Hp = 0;
            Player_Script.Player_Sp_Point++;
            Player_Script.Player_Sp += 10;
            if (GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] % 3 == 0)  
            {
                GameControl_Scripts.Terrain_Org[End_exPos, End_eyPos] /= 3;
            }
            GameControl_Scripts.Game_Enemies_Cnt--;
            GameControl_Scripts.Game_Enemies_lastCnt--;
            Destroy(gameObject);
            return;
        }


        // 敌人进入防御点
        if (End_exPos == 0) 
        {
            GameControl_Scripts.Game_Enemies_Cnt--;
            GameControl_Scripts.Game_Enemies_lastCnt--;
            Destroy(gameObject);
            return;
        }

    }
}
