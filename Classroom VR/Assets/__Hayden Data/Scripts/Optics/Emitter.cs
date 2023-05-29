using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Valve.VR.InteractionSystem.Sample
{
    public class Emitter : MonoBehaviour
    {
        public Material lineMaterial;
        public Material other;
        public GameObject lineHolder;

        List<opticalRay> allRays = new List<opticalRay>();

        public float laserWidth = 0.1f;
        public float laserMaxLength = 5f;

        public int numRays;
        public float rayAngle;

        public LayerMask CheckLayerMask;

        public List<Vector3> imagePoints = new List<Vector3>();
        public List<float> imageScales = new List<float>();

        public GameObject candleImage;
        public GameObject imagePointHolder;

        public int numRayCasts = 0;

        void Start()
        {
            //Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            //laserLineRenderer.SetPositions(initLaserPositions);
            //laserLineRenderer.SetWidth(laserWidth, laserWidth);

            float dirY = (numRays/2)*rayAngle;

            for (int i = 0; i < numRays; i++)
            {
                opticalRay tempRay = new opticalRay();
                tempRay.dirY = dirY;
                tempRay.candle = (Emitter)this;

                GameObject myLine = new GameObject();
                myLine.transform.position = this.transform.position;
                myLine.AddComponent<LineRenderer>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();

                lr.positionCount = 2;
                lr.SetWidth(laserWidth, laserWidth);
                lr.material = lineMaterial;

                myLine.transform.parent = this.transform;

                tempRay.ray = lr;
                tempRay.lineHolder = myLine;

                dirY -= rayAngle;
                
                tempRay.dirY = dirY;

                allRays.Add(tempRay);
            }

            //quickSort(allRays, 0, allRays.Count - 1);

            /*foreach (opticalRay rayPart in allRays)
            {
                //Debug.Log(rayPart.dirY.ToString("F5"));
            }*/
        }

        void Update()
        {
            numRayCasts = 0;
            Vector3 minus = new Vector3();
            minus.x = this.transform.position.x;
            this.transform.position = this.transform.position - minus;

            imagePoints = new List<Vector3>();
            imageScales = new List<float>();
            foreach (opticalRay currentRay in allRays)
            {
                currentRay.runSimulation();
            }

            allRays[0].enableLineRenderer();
            allRays[allRays.Count - 1].enableLineRenderer();
            for (int i = 1; i <= 4; i++)
            {
                //Debug.Log("size: " + angleCount)
                allRays[((allRays.Count - 1) / 5) * i].enableLineRenderer();
            }

            //this is bad
            List<hitObject> foundObjects = new List<hitObject>();

            foreach (opticalRay shouldEnable in allRays)
            {
                int i = 0;
                foreach (GameObject individualHit in shouldEnable.objectHit)
                {
                    //Debug.Log("test2");
                    bool inAlready = false;
                    foreach (hitObject alreadyCreated in foundObjects)
                    {
                        if (alreadyCreated.hit == individualHit && alreadyCreated.depth == shouldEnable.depths[i]) {
                            alreadyCreated.anglesHit.Add(shouldEnable);
                            inAlready = true; 
                            break; 
                        }
                    }
                    //Debug.Log(inAlready);
                    if (!inAlready)
                    {
                        hitObject createHit = new hitObject();
                        createHit.depth = shouldEnable.depths[i];
                        createHit.hit = individualHit;
                        createHit.anglesHit.Add(shouldEnable);
                        Debug.Log("first: " + createHit.pointsHitY.Count.ToString());
                        createHit.pointsHitY.Add(shouldEnable.pointsHitY[i]);
                        Debug.Log("second: " + createHit.pointsHitY.Count.ToString());

                        foundObjects.Add(createHit);
                    }

                    i++;
                }
            }

            foreach (hitObject createdHit in foundObjects)
            {
                createdHit.onOff();
            }

            int numImageCandles = imagePointHolder.transform.childCount;
            int newCandles = imagePoints.Count - numImageCandles;
            if (newCandles > 0)
            {
                for (int i = 0; i < newCandles; i++)
                {
                    GameObject newCandlePreFab = Instantiate(candleImage, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(-90,0,0)));
                    newCandlePreFab.transform.SetParent(imagePointHolder.transform);

                }
            }else if (newCandles < 0 )
            {
                Transform[] children = imagePointHolder.GetComponentsInChildren<Transform>();
                
                for (int i = 0; i < (newCandles*-1); i++)
                {
                    Destroy(children[i+1].gameObject);
                }
            }
            Transform[] ts = imagePointHolder.GetComponentsInChildren<Transform>();
            int imageNum = 0;
            foreach (Vector3 imageLocation in imagePoints)
            {
                ts[imageNum+1].transform.position = imageLocation;
        
                ts[imageNum+1].localScale = new Vector3(imageScales[imageNum] * 10, imageScales[imageNum] * 10, imageScales[imageNum] * 10);

                imageNum++;
            }

        }

    }

    public class opticalRay
    {
        public Emitter candle;

        public LineRenderer ray;
        public GameObject lineHolder;

        public float dirY;

        public bool createRay = false;

        public List<GameObject> objectHit = new List<GameObject>();
        public List<int> depths = new List<int>();
        public List<float> pointsHitY = new List<float>();

        public void runSimulation()
        {
            Vector3 direction = Vector3.forward;
            direction.y += dirY;
            ray.material = candle.lineMaterial;
            ray.Reset();
            ray.positionCount = 2;  
            ray.SetPosition(0, candle.transform.position);

            objectHit = new List<GameObject>();
            pointsHitY = new List<float>();
            depths = new List<int>();

            try
            {
                //Debug.Log("number: " + candle.numRayCasts.ToString());
                ShootLaserFromTargetPosition(candle.transform.position, direction, candle.laserMaxLength, ray, 0, candle.transform.position);
                disableLineRenderer();
            }
            catch (Exception e)
            {
                //Debug.Log("number: " + candle.numRayCasts.ToString());
                throw e;
            }
           


        }


        void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length, LineRenderer laserLineRenderer, int depth, Vector3 startPosition)
        {
            if(depth > 100)
            {
                //Debug.Log("depth: " + depth.ToString());
            }
            
            
            Ray ray = new Ray(startPosition + (direction/100), direction);
            candle.numRayCasts++;
            RaycastHit raycastHit;

            Vector3 endPosition = new Vector3();
            //= targetPosition + (length * direction);

            float magnification;
            float imageDistance;
            bool imaginary;

            if (Physics.Raycast(ray, out raycastHit, length, ~(1 << 6)))
            {
                endPosition = raycastHit.point;
                laserLineRenderer.positionCount = depth + 2;

                laserLineRenderer.SetPosition(depth + 1, endPosition);

                

                if (raycastHit.collider.gameObject.tag == "Lens")
                {
                   

                    objectHit.Add(raycastHit.collider.gameObject);
                    pointsHitY.Add(endPosition.y);
                    depths.Add(depth);
                    //Debug.Log(objectHit.Count);
                    laserLineRenderer.positionCount = depth + 3;

                    (imageDistance, magnification, imaginary) = calculateImage(targetPosition, endPosition, raycastHit.collider.gameObject.GetComponent<Lens>());
                    if (depth > 0)
                    {
                        //Debug.Log("endpos: " + endPosition.ToString("F5") + " startpos: " + laserLineRenderer.GetPosition(depth).ToString("F5") + " dir: " + direction.ToString() + "  ");
                        //Debug.Log("image: " + imageDistance.ToString() + " mag: " + magnification.ToString());
                    }
                    Vector3 afterRefraction = new Vector3();
                    afterRefraction = raycastHit.collider.gameObject.transform.position;
                    afterRefraction.z += imageDistance;
                    afterRefraction.y += (candle.gameObject.transform.position.y - raycastHit.collider.gameObject.transform.position.y) * magnification;

                    Vector3 angleToImage = (afterRefraction - endPosition).normalized;
                    if (imaginary)
                    {
                        angleToImage = (endPosition - afterRefraction).normalized;
                    }

                    bool inList = false;
                    foreach (Vector3 imagePoint in candle.imagePoints)
                    {
                        if (afterRefraction == imagePoint)
                        {
                            inList = true;
                            break;
                        }
                    }

                    if (!inList){
                        candle.imagePoints.Add(afterRefraction);
                        candle.imageScales.Add(magnification);
                    }
                    
                    if (laserLineRenderer.GetPosition(depth) == endPosition && depth > 50)
                    {

                        laserLineRenderer.positionCount = depth + 3;
                        laserLineRenderer.SetPosition(depth + 2, endPosition + (angleToImage*length));
                        laserLineRenderer.material = candle.other;
                        //Debug.Log("this is a monkey: " + raycastHit.point.ToString("F6") + direction.ToString("F6") + angleToImage.ToString("F6"));
                        //Debug.Log(candle.CheckLayerMask);
                        return;

                    }
                   
                    

                    ShootLaserFromTargetPosition(afterRefraction, angleToImage, length, laserLineRenderer, depth + 1, endPosition);

                    //laserLineRenderer.SetPosition(2, angleToImage * candle.laserMaxLength);
                }
                else
                {
                    laserLineRenderer.positionCount = depth + 2;
                }

            }


        }

        Tuple<float, float, bool> calculateImage(Vector3 start, Vector3 hit, Lens lensHit)
        {
            float focal = lensHit.focalLength;
            float objectDist = hit.z - start.z;

            float imageDistance;
            float magnification;
            if (focal == objectDist)
            {
                imageDistance = 1000000.0f;
                magnification = -1000000.0f;
                return Tuple.Create(imageDistance, magnification, false);
            }

            //Debug.Log("focal: " + focal.ToString() + " objDist: " + objectDist.ToString());

            imageDistance = 1 / ((1 / focal) - (1 / objectDist));

            magnification = -(imageDistance / objectDist);

            bool imaginary = imageDistance < 0;


            return Tuple.Create(imageDistance, magnification, imaginary);
        }

        public void disableLineRenderer()
        {
            //ray.material = candle.lineMaterial;
            ray.enabled = false;
        }

        public void enableLineRenderer()
        {
            //ray.material = candle.other;
            ray.enabled = true;
        }
    }

    public class hitObject
    {
        public GameObject hit;
        public int depth;

        public float angleTop;
        public float angleBottom;

        public List<opticalRay> anglesHit = new List<opticalRay>();
        public List<float> pointsHitY = new List<float>();

        public void onOff()
        {
            //Debug.Log("1: " + anglesHit[0].dirY.ToString("F5") + " 2: " + anglesHit[anglesHit.Count - 1].dirY.ToString("F5"));
            //Debug.Log(anglesHit.Count);
            Debug.Log("sadf: " + anglesHit.Count.ToString() + " l: " + pointsHitY.Count.ToString());
            quickSort(pointsHitY, anglesHit, 0, anglesHit.Count - 1);
            
            

            anglesHit[0].enableLineRenderer();
            anglesHit[anglesHit.Count - 1].enableLineRenderer();

            int angleCount = anglesHit.Count;
            for (int i = 1; i <= 4; i++)
            {
                //Debug.Log("size: " + angleCount)
                anglesHit[((angleCount-1)/5)*i].enableLineRenderer();
            }
        }

        void quickSort(List<float> position, List<opticalRay> rayList, int low, int high)
        {
            //low and high are the range of the ray we want sorted, if low >= high or low < 0 it means no sorting is needed
            if (low >= high || low < 0) { return; }


            int pivot = partition(position, rayList, low, high);

            quickSort(position, rayList, low, pivot - 1);
            quickSort(position, rayList, pivot + 1, high);

        }

        int partition(List<float> position, List<opticalRay> rayPart, int low, int high)
        {
            float pivot = rayPart[high].pointsHitY[depth];

            int i = low - 1;
            Debug.Log(i);
            for (int j = low; j < high; j++)
            {
                if (rayPart[j].pointsHitY[depth] <= pivot)
                {
                    i++;

                    opticalRay lowPart = rayPart[i];
                    rayPart[i] = rayPart[j];
                    rayPart[j] = lowPart;

                    /*float lowPartFloat = position[i];
                    position[i] = position[j];
                    position[j] = lowPartFloat;*/

                }
            }

            i++;

            opticalRay iPart = rayPart[i];
            rayPart[i] = rayPart[high];
            rayPart[high] = iPart;

            /*float iPartFloat = position[i];
            position[i] = position[high];
            position[high] = iPartFloat;*/

            return i;
        }
    }
}