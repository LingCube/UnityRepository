using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DIALOG2 : MonoBehaviour
{
    public GameObject player;
    public GameObject dialog2;
    public Text T2;
    public TextAsset text2;
    public Image face;
    public Sprite P1, P2, P3, E, X;
    public float textspeed;
    private float xtextspeed;
    public int index;
    bool istalk = true;
    bool istextfinished;
    List<string> textlist = new List<string>();
    private void Awake()
    {
        xtextspeed = textspeed;
        GetTextFromFlie(text2);
    }
    public void GetTextFromFlie(TextAsset Flie)
    {
        textlist.Clear();
        index = 0;
        var linedata = Flie.text.Split('\n');
        foreach (var line in linedata) textlist.Add(line);
    }
    IEnumerator SetTextUI()
    {
        istextfinished = false;
        T2.text = "";
        switch(textlist[index])
        {
            case "P1\r":
                face.sprite = P1;
                index++;
                break;
            case "P2\r":
                face.sprite = P2;
                index++;
                break;
            case "P3\r":
                face.sprite = P3;
                index++;
                break;
            case "E\r":
                face.sprite = E;
                index++;
                break;
            case "X\r":
                face.sprite = X;
                index++;
                break;
        }
        for(int i=0;i<textlist[index].Length;i++)
        {
            T2.text += textlist[index][i];
            yield return new WaitForSeconds(textspeed);
        }
        istextfinished = true;
        index++;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"&&istalk)
        {
            collision.gameObject.GetComponent<PLAYER>().ismove = false;
            dialog2.SetActive(true);
            StartCoroutine(SetTextUI());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(istalk==true)
        {
            if(Input.GetKeyDown(KeyCode.F)&&istextfinished)
            {
                textspeed = xtextspeed;
                StartCoroutine(SetTextUI());
            }
            else if(Input.GetKeyDown(KeyCode.F)&&!istextfinished)
            {
                textspeed = 0;
            }
            if(Input.GetKeyDown(KeyCode.F)&&index==textlist.Count)
            {
                dialog2.SetActive(false);
                player.GetComponent<PLAYER>().ismove = true;
                istalk = false;
            }
        }
    }
}
