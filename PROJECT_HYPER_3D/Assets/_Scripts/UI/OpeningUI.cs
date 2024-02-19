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
    
    private void Start() {
        ShowOpening();
    }

    public void ShowOpening()
    {
        titleAnim.Play(titleAnimkey[0]);
        centerAnim.Play(centerAnimKey[0]);
        LeanTween.value(cg.gameObject, cg.alpha, 1.0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }).setOnComplete(()=>{
            cg.interactable = true;
            cg.blocksRaycasts = true;
            startBtn.interactable = true;
        });
    }

    public void DoHideOpening()
    {
        startBtn.interactable = false;
        GameplayController.instance.Flash(()=>{
            HideOpening(()=>{
                GameplayController.instance.StartGameCountDown();
            });
        }, null);
        
    }

    public void HideOpening(System.Action act)
    {
        
        startBtn.interactable = false;
        titleAnim.Play(titleAnimkey[1]);
        centerAnim.Play(centerAnimKey[1]);
        GameplayController.instance.CharSetup();
        LeanTween.value(cg.gameObject, 1.0f, .0f, .5f).setDelay(2).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }).setOnComplete(()=>{
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }).setOnComplete(()=>{
            
            GameplayController.instance.StartGameCountDown();
        });
    }
}
