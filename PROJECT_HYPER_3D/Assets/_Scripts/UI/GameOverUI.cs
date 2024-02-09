using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] CanvasGroup cg;
    [SerializeField] GameObject continueContainer;
    [SerializeField] Button nextBtn;
    [SerializeField] Button finishBtn;

    [Header("Statisics UI")]
    [SerializeField] GameObject statisticCarrier;
    [SerializeField] CanvasGroup statisticCarrierCG;
    [SerializeField] List<StatisticElement> statisticalElements;
    [SerializeField] List<StatisticElement> baseStatisticalElements;



    public void ShowGameOverNotifier()
    {
        
        StartCoroutine(ShowingGONotifier());
        

        IEnumerator ShowingGONotifier()
        {
            LeanTween.cancel(cg.gameObject);
            LeanTween.cancel(continueContainer.gameObject);
            LeanTween.cancel(statisticCarrier.gameObject);
            LeanTween.cancel(nextBtn.gameObject);
            LeanTween.cancel(finishBtn.gameObject);

            nextBtn.interactable = false;
            finishBtn.interactable = false;

            statisticCarrier.transform.localPosition = new Vector3(-500.0f, .0f, .0f);

            cg.alpha = 0;
            cg.interactable = true;
            cg.blocksRaycasts = true;

            continueContainer.transform.localPosition = new Vector3(-500.0f, .0f, .0f);

            LeanTween.value(cg.gameObject, .0f, 1.0f, 1.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                cg.alpha = f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            });

            yield return new WaitForSeconds(1.5f);

            LeanTween.moveLocalX(continueContainer.gameObject, .0f, 1.0f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(1.0f);


            for(int i = 0 ; i < statisticalElements.Count ; i++)
            {
                statisticalElements[i].Show(i, statisticalElements.Count);
                yield return new WaitForSeconds(.5f);
            }

            LeanTween.scale(nextBtn.gameObject, Vector3.one, .5f).setEase(LeanTweenType.easeOutBack);
        }
    }

    public void HideStatistic()
    {
        LeanTween.cancel(continueContainer.gameObject);
        LeanTween.cancel(statisticCarrier.gameObject);
        LeanTween.cancel(nextBtn.gameObject);
        LeanTween.cancel(finishBtn.gameObject);

        nextBtn.interactable = false;
        finishBtn.interactable = false;

        LeanTween.moveLocalX(continueContainer.gameObject, 100.0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            statisticCarrier.transform.localPosition = new Vector3(-500.0f, .0f, .0f);
        });
        LeanTween.value(statisticCarrierCG.gameObject, 1.0f, .0f, .75f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            statisticCarrierCG.alpha = f;
            statisticCarrierCG.interactable = true;
            statisticCarrierCG.blocksRaycasts = true;
        }).setOnComplete(()=>{
            statisticCarrierCG.interactable = false;
            statisticCarrierCG.blocksRaycasts = false;
            LeanTween.value(cg.gameObject, 1.0f, .0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                cg.alpha = f; 
            }).setOnComplete(()=>{
                cg.interactable = false;
                cg.blocksRaycasts = false;
            });
        });
        

        


    }

    public void ShowCompleteGameOver()
    {
        
    
        StartCoroutine(ShowingGONotifier());
        IEnumerator ShowingGONotifier()
        {
            LeanTween.cancel(continueContainer.gameObject);
            LeanTween.cancel(statisticCarrier.gameObject);
            LeanTween.cancel(nextBtn.gameObject);
            LeanTween.cancel(finishBtn.gameObject);

            nextBtn.interactable = false;
            finishBtn.interactable = false;

            LeanTween.scale(nextBtn.gameObject, Vector3.zero, 0.25f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(0.25f);

            statisticCarrier.transform.localPosition = new Vector3(-500.0f, .0f, .0f);

            LeanTween.moveLocalX(continueContainer.gameObject, 500.0f, 1.0f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(1.0f);


            for(int i = 0 ; i < baseStatisticalElements.Count ; i++)
            {
                baseStatisticalElements[i].Show(i, baseStatisticalElements.Count);
                yield return new WaitForSeconds(.5f);
            }

            LeanTween.scale(finishBtn.gameObject, Vector3.one, .5f).setEase(LeanTweenType.easeOutBack);
        }
    }
}
