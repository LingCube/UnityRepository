using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tower_Script : MonoBehaviour
{

    public Transform gun;

    public Animator tower_anim;

    public GameObject bullet;

    public Image cd_Image;

    public int Tower_Type;


    public float de_cd_speed;


    int x_tower_pos;
    int y_tower_pos;

    // Start is called before the first frame update
    void Start()
    {

        tower_anim = transform.GetChild(1).GetComponent<Animator>();
        tower_anim.SetBool("Tower_1_anim", false);
        tower_anim.SetBool("Tower_2_anim", false);
        tower_anim.SetBool("Tower_3_anim", false);
        tower_anim.SetBool("Tower_4_anim", false);

        switch(Tower_Type)
        {
            case 1:
                tower_anim.SetBool("Tower_1_anim", true);
                break;
            case 2:
                tower_anim.SetBool("Tower_2_anim", true);
                break;
            case 3:
                tower_anim.SetBool("Tower_3_anim", true);
                break;
            case 4:
                tower_anim.SetBool("Tower_4_anim", true);
                break;
            default:
                break;
        }

        x_tower_pos = (int)transform.position.x;
        y_tower_pos = (int)transform.position.y;
        cd_Image.fillAmount = 0;
    }



    //Tower Bullet Create   防御塔发射子弹
    public void Tower_Bullet_Create(Vector3 create_pos)
    {
        if (cd_Image.fillAmount == 1) 
        {
            GameObject Atk_obj = Instantiate(bullet, create_pos, transform.rotation);
            Atk_obj.transform.parent = transform.GetChild(0);
            Atk_obj.GetComponent<Bullet_Script>().Bullet_Type = Tower_Type;
            cd_Image.fillAmount = 0;
        }
    }




    // Update is called once per frame
    void Update()
    {

        if (!GameControl_Scripts.Game_isStart)
        {
            return;
        }


        switch(Tower_Type)
        {
            //向前方一定距离发射子弹
            case 1:
                de_cd_speed = 0.5f;
                for(int i = x_tower_pos; i <= x_tower_pos + 7 && i < GameControl_Scripts.x_Terrain_Org + 2; i++)
                {
                    if (GameControl_Scripts.Terrain_Org[i, y_tower_pos] % 3 == 0)
                    {
                        Tower_Bullet_Create(gun.position);
                    }
                    
                }
                break;

            //在前方一段范围内埋下地雷
            case 2:
                de_cd_speed = 0.1f;
                for (int i = x_tower_pos + 1; i <= x_tower_pos + 3 && i < GameControl_Scripts.x_Terrain_Org + 2; i++)
                {
                    for(int j = y_tower_pos - 1; j <= y_tower_pos + 1 && j < GameControl_Scripts.y_Terrain_Org + 2; j++)
                    {
                        if (GameControl_Scripts.Terrain_Org[i, j] % 7 != 0 && GameControl_Scripts.Terrain_Org[i, j] % 2 != 0) 
                        {
                            Tower_Bullet_Create(new Vector3(i, j, 0));
                        }
                    }
                }
                break;

            //向前方一定距离发射减速子弹
            case 3:
                de_cd_speed = 0.35f;
                for(int i = x_tower_pos; i < x_tower_pos + 9 && i < GameControl_Scripts.x_Terrain_Org + 2; i++)
                {
                    if (GameControl_Scripts.Terrain_Org[i, y_tower_pos] % 3 == 0)
                    {
                        Tower_Bullet_Create(gun.position);
                    }
                }
                break;
                
            //向四周发射寒气使敌人停顿
            case 4:
                de_cd_speed = 0.2f;
                for (int i = x_tower_pos - 1; i <= x_tower_pos + 1 && i < GameControl_Scripts.x_Terrain_Org + 2; i++)  
                {
                    for (int j = y_tower_pos - 1; j <= y_tower_pos + 1 && j < GameControl_Scripts.y_Terrain_Org + 2; j++) 
                    {
                        //Enemy exist judgment   判断攻击范围内是否有敌人
                        if (GameControl_Scripts.Terrain_Org[i, j] % 3 == 0)
                        {
                            Tower_Bullet_Create(transform.position);
                        }

                        //Terrain return   地形还原
                        if (GameControl_Scripts.Terrain_Org[i, j] % 17 == 0)  
                        {
                            GameControl_Scripts.Terrain_Org[i, j] /= 17;
                        }
                        
                    }
                }
                break;
            default:
                break;
        }
        if (cd_Image.fillAmount >= 1) cd_Image.fillAmount = 1;
        if (cd_Image.fillAmount < 1) cd_Image.fillAmount += de_cd_speed * Time.deltaTime;
        if (GameControl_Scripts.Terrain_Org[x_tower_pos, y_tower_pos] % 3 == 0)
        {
            GameControl_Scripts.Terrain_Org[x_tower_pos, y_tower_pos] /= 2;
            for(int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).GetComponent<Bullet_Script>().isBulletDestroy = true;
            }
            Destroy(gameObject);
        }
    }
}
