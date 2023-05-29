using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnSystemButtons : MonoBehaviour
{
    public LoadDefaultSystem buttonPreFab;
    string fileLocation;

    List<string[]> items = new List<string[]>();

    int numX = 0;
    int numY = 0;

    public Vector3 topRight;

    void Start()
    {
        fileLocation = Application.dataPath + "/__Hayden Data/DefaultSystems/list.csv";

        Debug.Log(fileLocation);

        using (var reader = new StreamReader(fileLocation))
        {
            while (!reader.EndOfStream)
            {

                var line = reader.ReadLine();
                var values = line.Split(',');

                Debug.Log(values[0]);

                items.Add(values);

                Vector3 currentLocation = topRight + new Vector3(numX * 0.4f, -(numY * 0.3f), 0);

                LoadDefaultSystem newButton = Instantiate(buttonPreFab, new Vector3(), Quaternion.identity);
                newButton.transform.parent = this.transform;
                newButton.transform.localPosition = currentLocation;
                newButton.transform.localRotation = Quaternion.identity;
                newButton.file = values[0];
                newButton.name = values[1];

                numX++;
                if (numX == 3)
                {
                    numX = 0;
                    numY++;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
