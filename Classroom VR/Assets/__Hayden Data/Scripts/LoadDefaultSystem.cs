using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadDefaultSystem : MonoBehaviour
{
    public string name;
    public string file;
    public TextMeshProUGUI text;

    void Start()
    {

        
        text.text = name;
    }

    public void loadSystem()
    {
        GameManager.Instance.createPlanetSystem(file + ".csv");
    }
}
