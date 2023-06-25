using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{

    public int Bullet_Type;

    int x_start_bullet_pos;
    int y_start_bullet_pos;

    public bool isBulletDestroy = false;

    // Start is called before the first frame update
    void Start()
    {
        x_start_bullet_pos = (int)transform.position.x;
        y_start_bullet_pos = (int)transform.position.y;
        switch (Bullet_Type)
        {
            case 1:
                Destroy(gameObject, 15);
                break;
            case 2:
                GameControl_Scripts.Terrain_Org[(int)transform.position.x, (int)transform.position.y] *= 7;
                break;
            case 3:
                Destroy(gameObject, 20);
                break;
            case 4:
                Destroy(gameObject, 2);
                break;
            default:
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameControl_Scripts.Game_isStart)
        {
            return;
        }

        int Bullet_eStart_xPos = (int)transform.position.x;
        int Bullet_eStart_yPos = (int)transform.position.y;
        switch(Bullet_Type)
        {
            case 1:
                transform.position += new Vector3(5f * Time.deltaTime, 0, 0);
                if (transform.position.x - Bullet_eStart_xPos > 0.3f
                    && GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] % 3 == 0)  
                {
                    GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] *= 5;
                    Destroy(gameObject);
                }
                if (transform.position.x - x_start_bullet_pos > 7 || transform.position.x > GameControl_Scripts.x_Terrain_Org) 
                {
                    Destroy(gameObject);
                }
                
                break;
            case 2:
                if (GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] % 3 == 0) 
                {
                    GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] *= 11;
                    Destroy(gameObject);
                }
                break;
            case 3:
                transform.position += new Vector3(7f * Time.deltaTime, 0, 0);
                if (transform.position.x - Bullet_eStart_xPos > 0.3f
                    && GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] % 3 == 0) 
                {
                    GameControl_Scripts.Terrain_Org[Bullet_eStart_xPos, Bullet_eStart_yPos] *= 13;
                    Destroy(gameObject);
                }
                if (transform.position.x - x_start_bullet_pos > 9 || transform.position.x > GameControl_Scripts.x_Terrain_Org)
                {
                    Destroy(gameObject);
                }
                break;
            case 4:
                for (int i = Bullet_eStart_xPos - 1; i <= Bullet_eStart_xPos + 1 && i < GameControl_Scripts.x_Terrain_Org + 2; i++)   
                {
                    for (int j = Bullet_eStart_yPos - 1; j <= Bullet_eStart_yPos + 1 && j < GameControl_Scripts.y_Terrain_Org + 2; j++) 
                    {
                        GameControl_Scripts.Terrain_Org[i, j] *= 17;
                        Destroy(gameObject);
                    }
                }
                break;
            default:
                break;
        }
        if(isBulletDestroy)
        {
            Destroy(gameObject);
        }
    }
}
