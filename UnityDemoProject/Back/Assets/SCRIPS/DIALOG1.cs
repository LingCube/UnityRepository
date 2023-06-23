using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DIALOG1 : MonoBehaviour
{
    public GameObject player;
    public Text T1;
    public TextAsset text1;
    public Image face;
    public Sprite P1, P2, X;
    public float textspeed;
    private float xtextspeed;
    public int index;
    private bool istextfinshed;
    List<string> textlist = new List<string>();
    void Awake()
    {
        xtextspeed = textspeed;
        player.GetComponent<PLAYER>().ismove = false;
        GetTextFromFlie(text1);
    }
    public void OnEnable()
    {
        StartCoroutine(SetTextUI());
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
        istextfinshed = false;
        T1.text = "";
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
            case "X\r":
                face.sprite = X;
                index++;
                break;
        }
        for(int i=0;i<textlist[index].Length;i++)
        {
            T1.text += textlist[index][i];
            yield return new WaitForSeconds(textspeed);
        }
        istextfinshed = true;
        index++;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)&&istextfinshed)
        {
            textspeed = xtextspeed;
            StartCoroutine(SetTextUI());
        }
        else if(Input.GetKeyDown(KeyCode.F)&&!istextfinshed)
        {
            textspeed = 0;
        }
        if(Input.GetKeyDown(KeyCode.F)&&index==textlist.Count)
        {
            gameObject.SetActive(false);
            player.GetComponent<PLAYER>().ismove = true;
        }
    }
}
