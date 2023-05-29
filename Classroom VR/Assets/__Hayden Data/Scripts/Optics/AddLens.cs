using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class AddLens : MonoBehaviour
    {
        public GameObject lensPrefab;
        public GameObject lensParent;

        public void createLens()
        {
            GameObject newLens = Instantiate(lensPrefab, new Vector3(0,1,0), Quaternion.identity);
            newLens.gameObject.transform.parent = lensParent.transform;
        }

        public void deleteLens()
        {
            Destroy(lensParent);
        }
    }
}

