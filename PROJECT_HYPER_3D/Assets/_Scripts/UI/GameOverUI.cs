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

    public void RefreshValues(StatisticID id, float value)
    {
        string v = Mathf.RoundToInt(value).ToString();
        statisticalElements[(int)id].SetValue(v);
    }

    public void RefreshValues(StatisticID id, string value)
    {
        statisticalElements[(int)id].SetValue(value);
    }

    public void SyncValuesToUI(List<float> values)
    {
        for(int i = 0 ; i < values.Count; i++)
        {
            if(i == 0)
                continue;

            RefreshValues((StatisticID)i, values[i]);
        }
    }

    public void RefreshTimeValue()
    {
        string _t;
        float _s = GameplayController.instance.GetSessionSecond();
        System.TimeSpan t = System.TimeSpan.FromSeconds(_s);
        _t = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        statisticalElements[(int)StatisticID.timeSurvive].SetValue(_t);
    }

    public void ShowGameOverNotifier()
    {
        RefreshTimeValue();
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

            continueContainer.transform.localPosition = new Vector3(-1500.0f, .0f, .0f);

            LeanTween.value(cg.gameObject, .0f, 1.0f, 1.5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                cg.alpha = f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            });

            for(int i = 0 ; i < statisticalElements.Count ; i++)
            {
                statisticalElements[i].ResetPos();
                
            }

            yield return new WaitForSeconds(1.5f);

            LeanTween.moveLocalX(continueContainer.gameObject, .0f, .50f).setEase(LeanTweenType.easeOutQuad);

            yield return new WaitForSeconds(1.0f);




            for(int i = 0 ; i < statisticalElements.Count ; i++)
            {
                statisticalElements[i].Show(i, statisticalElements.Count);
                yield return new WaitForSeconds(.15f);
            }

            LeanTween.scale(nextBtn.gameObject, Vector3.one, .5f).setEase(LeanTweenType.easeOutBack);
        }
        nextBtn.interactable = true;
        finishBtn.interactable = true;
    }

    public void HideStatistic()
    {
        LeanTween.cancel(continueContainer.gameObject);
        LeanTween.cancel(statisticCarrier.gameObject);
        LeanTween.cancel(nextBtn.gameObject);
        LeanTween.cancel(finishBtn.gameObject);

        nextBtn.interactable = false;
        finishBtn.interactable = false;

        LeanTween.moveLocalX(continueContainer.gameObject, 500.0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            statisticCarrier.transform.localPosition = new Vector3(-500.0f, .0f, .0f);
        });
        LeanTween.value(cg.gameObject, 1.0f, .0f, .75f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }).setOnComplete(()=>{
            cg.interactable = false;
            cg.blocksRaycasts = false;
            GameplayController.instance.ShowOpeningUI();
            // LeanTween.value(cg.gameObject, 1.0f, .0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            //     cg.alpha = f; 
            // }).setOnComplete(()=>{
            //     cg.interactable = false;
            //     cg.blocksRaycasts = false;
            // }).setOnComplete(()=>{
                
            // });
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
