using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GAMEOVER : MonoBehaviour
{
    public GameObject gameover;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void restart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
        gameover.SetActive(false);
    }
    public void backmeun()
    {
        SceneManager.LoadScene(0);
        gameover.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
