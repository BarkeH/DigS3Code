using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public GameObject planetHolder;
    public FieldLines FieldHolder;

    public PlanetObject planetPreFab;

    public bool gravityPaused;
    public bool fieldLinesEnabled
    {
        get { return _fieldLinesEnabled; }
        set
        {
            _fieldLinesEnabled = value;
            FieldHolder.gameObject.SetActive(value);
        }
    }

    private bool _fieldLinesEnabled = true;
    public List<string> planetCsvNames = new List<string>();
    
    void Awake()
    {
        fieldLinesEnabled = true;
        Debug.Log(Application.dataPath);
        Instance = this;
    }

    public void planetsChanged()
    {
        Transform[] transform = planetHolder.GetComponentsInChildren<Transform>();
        
        foreach (Transform child in transform)
        {
            if (child.gameObject == planetHolder) { continue; }
            child.gameObject.GetComponent<PlanetObject>().updateObjectList();
            

        }
    }

    public void createPlanetSystem(string csvName)
    {
        Transform[] transform = planetHolder.GetComponentsInChildren<Transform>();

        foreach (Transform child in transform)
        {
            if (child.gameObject == planetHolder) { continue; }
            Destroy(child.gameObject);
        }
        using (var reader = new StreamReader(Application.dataPath + "/__Hayden Data/DefaultSystems/" + csvName))
        {
            while (!reader.EndOfStream)
            {
                
                var line = reader.ReadLine();
                var values = line.Split(',');
                Debug.Log(line);
                float x;
                float y;
                float z;
                float mass;
                float radius;
                float velx;
                float vely;
                float velz;
                float.TryParse(values[1], out x);
                float.TryParse(values[2], out y);
                float.TryParse(values[3], out z);
                float.TryParse(values[4], out mass);
                float.TryParse(values[5], out radius);
                float.TryParse(values[6], out velx);
                float.TryParse(values[7], out vely);
                float.TryParse(values[8], out velz);

                PlanetObject newPlanet = Instantiate(planetPreFab, planetHolder.transform.position + new Vector3(x,y,z), Quaternion.Euler(new Vector3(0, 0, 0)));
                
                newPlanet.transform.parent = planetHolder.transform;
                newPlanet.mass = mass;
                newPlanet.radius = radius;
                newPlanet.velocity = new Vector3(velx, vely, velz);
            }
        }
        planetsChanged();
    }
}

