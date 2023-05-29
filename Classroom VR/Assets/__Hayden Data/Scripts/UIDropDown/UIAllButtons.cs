using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Valve.VR.InteractionSystem.Sample
{
    public class UIAllButtons : MonoBehaviour
    {
        public GameObject canvas;
        public bool isSlider;
        public GameObject baseObject;
        public string scriptName;
        public string variableName;
        public string funcName;
        public List<GameObject> enableObjects = new List<GameObject>();
        public List<GameObject> disableObjects = new List<GameObject>();
        public List<string> boolStartFinish = new List<string>();
        public TextMeshProUGUI text;
        public bool isProperty = false;
        public ThrowableSlider grabObject;

        public bool isBoolButton = false;

        public void enable()
        {
            foreach (GameObject enObj in enableObjects)
            {
                enObj.SetActive(true);
            }

            foreach (GameObject disObj in disableObjects)
            {
                disObj.SetActive(false);
            }
          
            if (isSlider)
            {
                float value = (float)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
                
            }
        }

        void Update()
        {
            if (isBoolButton)
            {
                if (!isProperty)
                {
                    bool value = (bool)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
                    text.text = boolStartFinish[!value ? 1 : 0];
                }
                else
                {
                    bool value = (bool)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetProperty(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
                    text.text = boolStartFinish[!value ? 1 : 0];
                }
            }
        }

        public void updateValue()
        {
            float value = (float)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
            text.text = value.ToString();
            (baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).SetValue(baseObject.GetComponent(scriptName) as MonoBehaviour, this.GetComponent<Slider>().value);

        }

        public void back(Hand hand)
        {
            canvas.SetActive(false);
        }

        public void boolButton()
        {
            
            if (!isProperty)
            {
                bool value = (bool)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
                (baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetField(variableName).SetValue(baseObject.GetComponent(scriptName) as MonoBehaviour, !value);
                text.text = boolStartFinish[!value ? 1 : 0];
            }else
            {
                bool value = (bool)(baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetProperty(variableName).GetValue(baseObject.GetComponent(scriptName) as MonoBehaviour);
                (baseObject.GetComponent(scriptName) as MonoBehaviour).GetType().GetProperty(variableName).SetValue(baseObject.GetComponent(scriptName) as MonoBehaviour, !value);
                text.text = boolStartFinish[!value ? 1 : 0];
            }
            
        }

        public void runFunction()
        {
            GameManager.Instance.createPlanetSystem("SolarSystem.csv");
        }

        public void functionButtonNoParams()
        {
            (baseObject.GetComponent(scriptName) as MonoBehaviour).Invoke(funcName,0.0f);
        }

    }
}