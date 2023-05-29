using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTrack : MonoBehaviour
{
    public Vector3 teleportTo = new Vector3();
    public GameObject worldHolder;

    void Update()
    {   

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Train"))
        {
            worldHolder.transform.position = teleportTo;
        }
    }
}
