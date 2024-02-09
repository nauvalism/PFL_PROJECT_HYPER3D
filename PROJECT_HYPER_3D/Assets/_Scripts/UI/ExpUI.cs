using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{
    [SerializeField] Slider mainSlider;
    [SerializeField] TextMeshProUGUI levelTxt;

    public void InitExp(float max, float cur)
    {
        this.mainSlider.maxValue = max;
        this.mainSlider.value = cur;
    }

    public void UpdateSliderValue(float f)
    {
        this.mainSlider.value = f;
    }

    public void InitExp(float max, float cur, int lvl, bool gradual = false)
    {
        if(!gradual)
        {
            this.mainSlider.maxValue = max;
            this.mainSlider.value = cur;
            
        }
        else
        {
            this.mainSlider.maxValue = max;
            if(cur > this.mainSlider.value)
            {
                LeanTween.cancel(mainSlider.gameObject);
                LeanTween.value(mainSlider.gameObject, mainSlider.value, cur, 0.25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                    mainSlider.value = f;
                });
            }
            else
            {
                LeanTween.cancel(mainSlider.gameObject);
                LeanTween.value(mainSlider.gameObject, 0, cur, 0.125f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                    mainSlider.value = f;
                });
            }
        }
        this.levelTxt.text = "Lv."+(lvl+1).ToString();
    }
}


