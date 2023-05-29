using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class Lens : MonoBehaviour
    {
        public float max = 5.0f;
        public float min = -5.0f;

        public float focalLength = 2;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 minus = new Vector3();
            minus.x = this.transform.parent.position.x;
            this.transform.parent.position = this.transform.parent.position - minus;

            this.transform.parent.rotation = Quaternion.Euler(new Vector3());


        }
    }
}