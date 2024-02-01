using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{
    [SerializeField] Slider mainSlider;
    [SerializeField] TextMeshProUGUI levelTxt;

    public void InitHP(float f)
    {
        this.mainSlider.maxValue = f;
        this.mainSlider.value = f;
    }

    public void UpdateSliderValue(float f)
    {
        this.mainSlider.value = f;
    }

    
}


