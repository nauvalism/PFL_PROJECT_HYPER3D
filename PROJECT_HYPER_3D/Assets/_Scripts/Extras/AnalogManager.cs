using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnalogManager : MonoBehaviour
{
    public static AnalogManager instance { get; set; }
    public AnalogController theAnalog;
    public ColorAlphaPingpong alphaGuide;
    public Vector3 cppPos;
    public CanvasGroup cg;

    private void OnDisable()
    {
        //Loger.Log("im disabled by");
    }
    private void Awake()
    {
        instance = this;
        //theAnalog.DissapearJoystick();
        cppPos = alphaGuide.transform.localPosition;
    }

    private void Update()
    {
#if UNITY_EDITOR
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    DoGuide();
        //}
#endif
    }

    public void DoGuide()
    {
        theAnalog.ResetJoystick();
        Vector3 acPos = new Vector3(0, cppPos.y, cppPos.z);
        alphaGuide.DoPingPongUI();
        
    }

    public void UnGuide()
    {
        alphaGuide.StopAlpha();
    }

    public void DisableAnalog()
    {
        theAnalog.DissapearJoystick();
        UnGuide();
    }

    public void ResetAnalog()
    {
        UnGuide();
        theAnalog.ResetJoystick();
        //DoGuide();
    }

    public void CancelAlpha()
    {
        
    }

    public void MoveMode()
    {
        DisableAnalog();
        //.isAnaloging = false;
    }

    public void UnMoveMode()
    {
        theAnalog.enableAnalog = true;
        DoGuide();
    }
}
