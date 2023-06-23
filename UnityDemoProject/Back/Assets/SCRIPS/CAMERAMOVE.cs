using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAMERAMOVE : MonoBehaviour
{
    public GameObject player;
    public float speed;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector3 = new Vector3(player.GetComponent<Transform>().position.x+offset.x,
            player.GetComponent<Transform>().position.y, transform.position.z);
        transform.position = vector3;
        transform.position = Vector3.Lerp(transform.position, vector3, speed * Time.deltaTime);
    }
}
