using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetObject : MonoBehaviour
{
    GameObject holder;
    GameManager gameManager;
    List<PlanetObject> planetObjects = new List<PlanetObject>();

    public float mass;
    public float radius;

    public bool isPlanet = true;
    public bool isFieldLine = false;

    public Vector3 scalePosition = new Vector3();
    public Vector3 velocity = new Vector3();
    public Vector3 acceleration = new Vector3();

    public Vector3 startPosition = new Vector3();

    void Start()
    {
        
        
        gameManager = GameManager.Instance;
        
        holder = gameManager.planetHolder;


        //add the other children of the holder object to the List planetObjects
        Transform[] transform = holder.GetComponentsInChildren<Transform>();
        foreach (Transform child in transform)
        {
            PlanetObject addition = child.gameObject.GetComponent<PlanetObject>();
            if (child.gameObject == this.gameObject || addition == null) { continue; }
            planetObjects.Add(addition);

        }

        startPosition = this.transform.position;
    }

    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(new Vector3());

        if (isPlanet && gameManager.gravityPaused == false)
        {
            updatePositionVectors();
            updateVelocityVectors();
            updateAccelerationVectors();


        }
        
    }

    void updatePositionVectors()
    {
        float addX = velocity.x * 0.08f * (1f/ 75000000f);
        float addY = velocity.y * 0.08f * (1f / 75000000f);
        float addZ = velocity.z * 0.08f * (1f / 50000000f);

        this.transform.position += new Vector3(addX, addY, addZ);

        scalePosition = this.transform.position * 75000000f;
    }

    void updateVelocityVectors()
    {
        velocity.x += acceleration.x * 0.08f;
        velocity.y += acceleration.y * 0.08f;
        velocity.z += acceleration.z * 0.08f;
    }

    bool updateAccelerationVectors()
    {
        acceleration.x = 0;
        acceleration.y = 0;
        acceleration.z = 0;

        foreach (PlanetObject massJ in planetObjects)
        {
            float dx = (massJ.scalePosition.x - scalePosition.x);
            float dy = (massJ.scalePosition.y - scalePosition.y);
            float dz = (massJ.scalePosition.z - scalePosition.z);

            float g = 6.67384e-11f;

            float distSq = (dx * dx) + (dy * dy) + (dz * dz);
            
            if (!isPlanet && distSq < 1E+14) {
                return true; }
            float force = (g * massJ.mass) / (distSq * ((float)Math.Sqrt(distSq) + 0.15f));

            acceleration.x += dx * force;
            acceleration.y += dy * force;
            acceleration.z += dz * force;
        }

        return false;

    }

    public void moveFieldPoint()
    {
        transform.position += startPosition - transform.position;
        acceleration = new Vector3();
        velocity = new Vector3(); 
        scalePosition = new Vector3();
        for (int i = 0; i < 300; i++)
        {
           

            updatePositionVectors();
            updateVelocityVectors();
            bool toClose = updateAccelerationVectors();
            if (toClose) {
               
                break; }
        }
    }

    public void updateObjectList()
    {
        Transform[] transform = holder.GetComponentsInChildren<Transform>();
        planetObjects = new List<PlanetObject>();
        foreach (Transform child in transform)
        {
            PlanetObject addition = child.gameObject.GetComponent<PlanetObject>();
            if (child.gameObject == this.gameObject || addition == null) { continue; }
            planetObjects.Add(addition);

        }
    }
}
