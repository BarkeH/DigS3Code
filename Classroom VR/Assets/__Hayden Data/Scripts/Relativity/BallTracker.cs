using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;


public class BallTracker : MonoBehaviour
{
    public TrainMovement world;

    bool isThrowing = false;
    Vector3 startPosition = new Vector3();
    List<Vector3> trackPosition = new List<Vector3>();
    public LineRenderer line;

    float timeSinceTrack = 0;
    public float timeSinceTrackThreshold = 0.01f;

    float distToGround = 0.15f;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrowing)
        {

            updateThrowingPosition();  
            if (IsGrounded())
            {
                pickUp();
            }
        }
    }

    void updateThrowingPosition()
    {
        timeSinceTrack += Time.deltaTime;

        if (timeSinceTrack < timeSinceTrackThreshold) { return; }

        timeSinceTrack = 0.0f;

        //trackPosition.Add(this.transform.position);
    }

    public void startThrowing()
    {
        if (trackPosition.Count > 0) { return; }

        isThrowing = true;
        //startPosition = this.transform.position;

        StartCoroutine(trackPositionRoutine());

        timeSinceTrack = 0;
    }

    public void pickUp()
    {
        if (isThrowing)
        {
            world.moving = false;
            isThrowing = false;

            setLineRenderer(trackPosition, startPosition);
        }
    }

    public void setLineRenderer(List<Vector3> trackedPositions, Vector3 startingPosition)
    {
        line.positionCount = trackedPositions.Count + 1;

        line.SetPosition(0, startingPosition);

        int i = 0;
        foreach (Vector3 pos in trackedPositions)
        {
            i++;
            line.SetPosition(i, trackedPositions[i - 1]);
        }
    }

    public void switchToStation()
    {
        List<Vector3> stationPositionList = new List<Vector3>();
        Vector3 stationStartPosition = new Vector3();
        int lineCount = trackPosition.Count + 1;
       
        stationStartPosition = startPosition - new Vector3(0, 0, 0.3f * lineCount);

        for (int i = 1; i < lineCount; i++)
        {
            Vector3 newPosition = trackPosition[i - 1] - new Vector3(0, 0, 0.3f * (lineCount - i));
            stationPositionList.Add(newPosition);
        }

        setLineRenderer(stationPositionList, stationStartPosition);

        
    }

    public void switchToTrain()
    {
        setLineRenderer(trackPosition, startPosition);
    }

    public void resetSimulation()
    {
        isThrowing = false;
        startPosition = new Vector3();
        trackPosition = new List<Vector3>();

        line.Reset();
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    IEnumerator trackPositionRoutine()
    {
        startPosition = this.transform.position;
        yield return new WaitForSeconds(timeSinceTrackThreshold);

        while (isThrowing)
        {
            trackPosition.Add(this.transform.position);
            yield return new WaitForSeconds(timeSinceTrackThreshold);
        }
    }
}


