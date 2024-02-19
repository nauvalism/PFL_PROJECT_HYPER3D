using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticElement : MonoBehaviour
{
    [SerializeField] ChoiceMemberID statisticCorrespondent;
    [SerializeField] Transform mover;
    [SerializeField] CanvasGroup moverCG;
    [SerializeField] string statisticTitle;
    [SerializeField] Sprite statisticSprite;
    [SerializeField] Image statisticImg;
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] TextMeshProUGUI valueTxt;
    [SerializeField] SoundManager sound;

    private void OnValidate() {
        if(statisticTitle != string.Empty)
        {
            if(titleTxt != null)
            {
                titleTxt.text = statisticTitle;
            }
            
        }

        if(statisticSprite != null)
        {
            statisticImg.sprite = statisticSprite;
        }
    }

    

    public void SetValue(string title, string value)
    {
        titleTxt.text = title;
        valueTxt.text = value;
    }

    public void SetValue( string value)
    {
        valueTxt.text = value;
    }

    public void ResetPos()
    {
        LeanTween.cancel(mover.gameObject);
        LeanTween.cancel(moverCG.gameObject);
        moverCG.alpha = 0;
        mover.localPosition = new Vector3(-150.0f, .0f, .0f);

    }

    public void Show(float index, float maxIndex)
    {
        LeanTween.cancel(mover.gameObject);
        LeanTween.cancel(moverCG.gameObject);

        LeanTween.moveLocalX(mover.gameObject, .0f, .75f).setEase(LeanTweenType.easeOutQuad);
        LeanTween.value(moverCG.gameObject, .0f, 1.0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            moverCG.alpha = f;
        });
        float add = index / maxIndex;
        sound.PlayPitchAdd(0,add);
    }
}
