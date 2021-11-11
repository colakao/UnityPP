using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public string myString;
    public Text myText;
    public float fadeTime = 10;
    public bool displayInfo;
    public MouseOrbitOld cameraScript;
    Renderer rend;
    public int camIndex;
    Color colorOrig;
    Color colorSelect = Color.white;
    Color colorMix;
    Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponentInChildren<Text>();
        
        //myText.color = Color.black;
        cameraScript = FindObjectOfType<MouseOrbitOld>();
        rend = GetComponent<Renderer>();
        colorOrig = rend.material.color;
        colorMix = Color.Lerp(colorOrig, colorSelect, 0.3f);
        //outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        FadeText();     
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("OnMouseEnter");
        displayInfo = true;
    }
    //public void OnPointerExit()
    public void OnPointerClick(PointerEventData eventData)
    {
		cameraScript.SetViewFront(camIndex);
		cameraScript.SetTraveling(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayInfo = false;
    }


    private void OnMouseOver()
    {
        displayInfo = true;
        
    }   

    private void OnMouseExit()
    {
        displayInfo = false;
        
    }
    
    private void OnMouseDown()
    {
		cameraScript.SetViewFront(camIndex);
		cameraScript.SetTraveling(true);
    }
    
    public void SelectPiece()
    {
		cameraScript.SetViewFront(camIndex);
		cameraScript.SetTraveling(true);
    }

    void FadeText()
    {
        if (displayInfo)
        {
            //myText.text = myString;
            myText.color = Color.Lerp(myText.color, Color.red, fadeTime * Time.deltaTime);
            //rend.material.color = Color.Lerp(rend.material.color, colorMix, fadeTime * Time.deltaTime);
            //outline.OutlineColor = Color.Lerp(outline.OutlineColor, Color.red, fadeTime * Time.deltaTime);
        }

        else
        {
            myText.color = Color.Lerp(myText.color, Color.black, fadeTime * Time.deltaTime);
            //rend.material.color = Color.Lerp(rend.material.color, colorOrig, fadeTime * Time.deltaTime);
            //outline.OutlineColor = Color.Lerp(outline.OutlineColor, Color.clear, fadeTime * Time.deltaTime);
            //outline.OutlineColor = Color.clear;
        }
    }

 
}
