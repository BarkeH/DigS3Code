using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    private bool _onTrain = true;
    public bool onTrain
    {
        get { return _onTrain; }
        set { 
            _onTrain = value;
            changedOnTrain();    
        }
    }
    public bool moving = true;
    public float speed = 0.05f;

    public BallTracker throwingObject;
    public GameObject trainObject;

    public Vector3 playerStationPosition = new Vector3();
    public Vector3 playerTrainPosition = new Vector3();

    public GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        player.transform.position = playerTrainPosition;
    }

    void Update()
    {
        if (moving && onTrain)
        {
            this.transform.position = this.transform.position - new Vector3(0, 0, speed * Time.deltaTime);
        }
    }

    void changedOnTrain()
    {
        if (onTrain == false)
        {
            player.transform.position = playerStationPosition;
            this.transform.position = new Vector3();
            throwingObject.switchToStation();
        }else {
            player.transform.position = playerTrainPosition;
            moving = true;
            throwingObject.switchToTrain();
        }
    }
}
