using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class DropDown : MonoBehaviour
    {
        public GameObject dropDownCanvas;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void clicked()
        {
            dropDownCanvas.SetActive(true);
        }
    }
}