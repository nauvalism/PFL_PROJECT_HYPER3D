using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Slider mainSlider;
    [SerializeField] TextMeshProUGUI hpTxt;

    public void InitHP(float f)
    {
        this.mainSlider.maxValue = f;
        this.mainSlider.value = f;
    }

    public void UpdateSliderValue(float f)
    {
        this.mainSlider.value = f;
    }

    public void UpdateText(float f)
    {
        hpTxt.text = Mathf.CeilToInt(f).ToString();
    }
}
