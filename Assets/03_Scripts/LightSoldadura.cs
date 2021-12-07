using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSoldadura : MonoBehaviour
{
    // Start is called before the first frame update
    Light light;
    Transform parentTrans;
    Vector3 oldEulerAngle;
    public float lightIntensity = 10;

    void Start()
    {
        light = GetComponent<Light>();
        parentTrans = GetComponentInParent<Transform>();
        oldEulerAngle = parentTrans.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentTrans.localScale.x > 0.9f )
        {
            light.intensity = lightIntensity;
        }
        else
        {            
            light.intensity = 0;
        }
    }
}
