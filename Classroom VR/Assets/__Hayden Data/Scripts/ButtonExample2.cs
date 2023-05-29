using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ButtonExample2 : MonoBehaviour
    {

        int num = 0;
        public void pressMe(Hand hand)
        {
            num++;      
            Debug.Log("Pressed me " + num + " times!");
        }
    }
}