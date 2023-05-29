using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
