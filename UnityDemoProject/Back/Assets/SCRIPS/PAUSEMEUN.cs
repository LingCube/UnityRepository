using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PAUSEMEUN : MonoBehaviour
{
    public GameObject pausemeun;
    public GameObject sure1;
    public GameObject sure2;
    private bool pausemeuntrue = true;
    
    // Start is called before the first frame update
    void Start()
    {
        pausemeun.SetActive(false);
    }
    public void pause()
    {
        pausemeun.SetActive(true);
        Time.timeScale = 0;
    }
    public void back()
    {
        if (pausemeun.activeSelf == true)
        {
            sure1.SetActive(false);
            sure2.SetActive(false);
        }
        else pausemeun.SetActive(false);
        Time.timeScale = 1;
    }
    public void back1()
    {
        if (sure1.activeSelf == false && sure2.activeSelf == false) pausemeun.SetActive(false);
        Time.timeScale = 1;
    }
    public void restart()
    {
        sure1.SetActive(true);
        sure2.SetActive(false);
    }
    public void giveup()
    {
        sure2.SetActive(true);
        sure1.SetActive(false);
    }
    public void sure()
    {
        if(sure1.activeSelf==true)
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
            sure1.SetActive(false);
            pausemeun.SetActive(false);
        }
        if (sure2.activeSelf == true)
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
            sure2.SetActive(false);
            pausemeun.SetActive(false);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(pausemeuntrue)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pausemeun.SetActive(true);
                sure1.SetActive(false);
                sure2.SetActive(false);
                Time.timeScale = 0;
                pausemeuntrue = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pausemeun.SetActive(false);
                sure1.SetActive(false);
                sure2.SetActive(false);
                Time.timeScale = 1;
                pausemeuntrue = true;
            }
        }
    }
}
