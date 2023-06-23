using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GAMESTART : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void gamestart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void quit()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
