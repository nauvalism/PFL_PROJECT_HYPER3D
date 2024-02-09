using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpeningUI : MonoBehaviour
{
    [SerializeField] Animator titleAnim;
    [SerializeField] List<string> titleAnimkey;
    [SerializeField] Animator centerAnim;
    [SerializeField] List<string> centerAnimKey;
    [SerializeField] CanvasGroup cg;
    [SerializeField] Button startBtn;
    

    public void ShowOpening()
    {
        LeanTween.value(cg.gameObject, 1.0f, .0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }).setOnComplete(()=>{
            cg.interactable = true;
            cg.blocksRaycasts = true;
            startBtn.interactable = true;
        });
    }

    public void HideOpening(System.Action act)
    {
        startBtn.interactable = false;
        LeanTween.value(cg.gameObject, 1.0f, .0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }).setOnComplete(()=>{
            cg.interactable = false;
            cg.blocksRaycasts = false;
        });
    }
}
