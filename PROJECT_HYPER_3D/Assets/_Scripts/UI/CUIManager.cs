using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI charNameTxt;
    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] Slider hpSlider;
    [SerializeField] HealthUI hpUI;



    public void InitHPSlider(float f)
    {
        //this.hpSlider.maxValue = f;
        //this.hpSlider.value = f;

        hpUI.InitHP(f);
    }

    public void UpdateSlider(float f)
    {
        //this.hpSlider.value = f;
        hpUI.UpdateSliderValue(f);
        hpUI.UpdateText(f);
    }

    public void UpdateText(float f)
    {
        hpTxt.text = Mathf.CeilToInt(f).ToString();
    }
}
