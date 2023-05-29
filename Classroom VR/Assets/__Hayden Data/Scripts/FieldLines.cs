using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldLines : MonoBehaviour
{
    public CurvedLineRenderer fieldDrawPreFab;
    public PlanetObject fieldPreFab;
    public int numPoints;
    public float distanceBetween;
    
    public List<PlanetObject> fieldPointList = new List<PlanetObject>();

    public GameObject lineHolder;
    public Material lineMaterial;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    public GameObject centre;

    List<LineRenderer> lineList = new List<LineRenderer>();

    public GameObject focus;

    long framesSinceUpdate = 20;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 startPointPosition = new Vector3((numPoints.x - 1) * (-distanceBetween / 2), (numPoints.y - 1) * (-distanceBetween / 2), (numPoints.z - 1) * (-distanceBetween / 2));
        //Vector3 fieldPointPosition = startPointPosition;

        for (float i = 0.0f; i < numPoints; i++)
        {
            PlanetObject newFieldPoint = Instantiate(fieldPreFab, this.gameObject.transform);
            Vector3 pos = new Vector3();
            pos.x = Mathf.Sin(i/3+1) * ((i + 20)/100);
            pos.z = Mathf.Cos(i/3+1) * ((i + 20)/100);
            pos.y = 0;
            newFieldPoint.transform.position += pos + focus.transform.position;
            fieldPointList.Add(newFieldPoint);
            
            GameObject lineHolderY = new GameObject();

            lineHolderY.transform.position = lineHolder.transform.position;
            lineHolderY.AddComponent<LineRenderer>();
            LineRenderer lr = lineHolderY.GetComponent<LineRenderer>();

           
            lr.SetWidth(laserWidth, laserWidth);
            lr.material = lineMaterial;

            lineHolderY.transform.parent = lineHolder.transform;
            lr.SetPosition(0, newFieldPoint.transform.position);

            lineList.Add(lr);

        }


        /*for (int x = 0; x < numPoints.x; x++)
        {

            List<List<PlanetObject>> yLine = new List<List<PlanetObject>>();
            for (int y = 0; y < numPoints.y; y++)
            {
                List<PlanetObject> zLine = new List<PlanetObject>();
                for (int z = 0; z < numPoints.z; z++)
                {
                    PlanetObject newFieldPoint = Instantiate(fieldPreFab, this.gameObject.transform);
                    newFieldPoint.transform.position += fieldPointPosition;
                    zLine.Add(newFieldPoint);

                    fieldPointPosition.z += distanceBetween;
                }
                yLine.Add(zLine);

                fieldPointPosition.z = startPointPosition.z;
                fieldPointPosition.y += distanceBetween;
            }
            fieldPointList.Add(yLine);

            fieldPointPosition.y = startPointPosition.y;
            fieldPointPosition.x += distanceBetween;
        }   

        for (int x = 0; x < numPoints.x; x++)
        {
            if (numPoints.y > 1)
            {
                for (int z = 0; z < numPoints.z; z++)
                {
                    GameObject lineHolderY = new GameObject();

                    lineHolderY.transform.position = lineHolder.transform.position;
                    lineHolderY.AddComponent<LineRenderer>();
                    LineRenderer lr = lineHolderY.GetComponent<LineRenderer>();

                    lr.positionCount = (int)numPoints.y;
                    lr.SetWidth(laserWidth, laserWidth);
                    lr.material = lineMaterial;

                    lineHolderY.transform.parent = lineHolder.transform;

                    lineListY.Add(lr);
                }
            }

            if (numPoints.z > 1)
            {
                for (int y = 0; y < numPoints.y; y++)
                {
                    GameObject lineHolderZ = new GameObject();

                    lineHolderZ.transform.position = lineHolder.transform.position;
                    lineHolderZ.AddComponent<LineRenderer>();
                    LineRenderer lr = lineHolderZ.GetComponent<LineRenderer>();

                    lr.positionCount = (int)numPoints.z;
                    lr.SetWidth(laserWidth, laserWidth);
                    lr.material = lineMaterial;

                    lineHolderZ.transform.parent = lineHolder.transform;

                    lineListZ.Add(lr);
                }
            }

        }

        if (numPoints.x > 1)
        {
            for (int z = 0; z < numPoints.z; z++)
            {
                for (int y = 0; y < numPoints.y; y++)
                {
                    GameObject lineHolderX = new GameObject();

                    lineHolderX.transform.position = lineHolder.transform.position;
                    lineHolderX.AddComponent<LineRenderer>();
                    LineRenderer lr = lineHolderX.GetComponent<LineRenderer>();

                    lr.positionCount = (int)numPoints.z;
                    lr.SetWidth(laserWidth, laserWidth);
                    lr.material = lineMaterial;

                    lineHolderX.transform.parent = lineHolder.transform;

                    lineListX.Add(lr);
                }
            }
        }
        Debug.Log("X" + fieldPointList.Count.ToString());
        Debug.Log("Y" + fieldPointList[0].Count.ToString());
        Debug.Log("Z" + fieldPointList[0][0].Count.ToString());

        Debug.Log("combined");
        Debug.Log("X" + lineListX.Count.ToString());
        Debug.Log("Z" + lineListZ.Count.ToString());
        Debug.Log("Y" + lineListY.Count.ToString());*/
    }

    // Update is called once per frame
    void Update()
    {
        /*framesSinceUpdate++;
        if (framesSinceUpdate < 20) { return; }

      
        int x = 0;
        int y = 0;
        int z = 0;
        int i = 0;
        int j = 0;
        int l = 0;
        foreach (List<List<PlanetObject>> yLine in fieldPointList)
        {
            
            j = 0;
            Debug.Log("z" + z.ToString());
            foreach (List<PlanetObject> zLine in yLine)
            {
                Debug.Log("j" + j.ToString());
                Debug.Log("x" + x.ToString());
                i = 0;
                foreach (PlanetObject point in zLine)
                {
                    Debug.Log("i" + i.ToString());
                    Debug.Log("y" + y.ToString());
                    point.moveFieldPoint();

                    lineListZ[x].SetPosition(i, point.transform.position);
                    lineListY[i+(z*zLine.Count)].SetPosition(j, point.transform.position);
                    //lineListX[j + (z * .Count)].SetPosition(z, point.transform.position);
                    i++;
                    y++;
                }
                j++;
                x++;
            }
            z++;
        }

        framesSinceUpdate = 0;*/
        framesSinceUpdate++;
        if (framesSinceUpdate < 20) { return; }
        int i = 0;
        foreach (PlanetObject fieldPoint in fieldPointList)
        {
            fieldPoint.moveFieldPoint();
            lineList[i].SetPosition(1, fieldPoint.transform.position);
            i++;
        }
    }
}
