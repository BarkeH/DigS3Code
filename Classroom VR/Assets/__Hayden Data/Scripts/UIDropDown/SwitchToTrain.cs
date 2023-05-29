using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchToTrain : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    public void switchScene()
    {
        //Destroy(player);
        Debug.Log("monkey");
        SceneManager.LoadScene(sceneName: "Hayden_TrainRelativity");
    }
    public void backToClassroom()
    {
        //Destroy(player);
        SceneManager.LoadScene(sceneName: "Hayden_Classroom");
    }
}
