using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CurrencyEnum
{
    expStar = 0,
    energy = 1,
    diamond = 2
}

public class UIManager : MonoBehaviour
{
    [Header("Level-And-Exp")]
    [SerializeField] Slider sExp;
    [SerializeField] int level;
    [SerializeField] TextMeshProUGUI lvlTxt;
    [SerializeField] ExpUI exp;
    [Header("Info")]
    [SerializeField] TextMeshProUGUI waveTxt;
    [SerializeField] TextMeshProUGUI enemiesKilledTxt;
    [Header("Currencies")]
    [SerializeField] List<MaterialGridContent> contents;
    [SerializeField] Currencies currencies;
    
    [Header("GAME OVER UI")]
    [SerializeField] GameOverUI gameOver;

    [Header("OpeningUI")]
    [SerializeField] OpeningUI opening;

    

    [Header("FULL UI EFFECT")]
    [SerializeField] Image flashGO;
    [SerializeField] Image fade;
    [SerializeField] CanvasGroup vignetteCG;
    [SerializeField] Image vignette;
    
    private void Update() {
        if(enemiesKilledTxt.transform.localScale.x > 1)
        {
            enemiesKilledTxt.transform.localScale = new Vector3(enemiesKilledTxt.transform.localScale.x - Time.deltaTime, enemiesKilledTxt.transform.localScale.y - Time.deltaTime, 0);
        }

        if(waveTxt.transform.localScale.x > 1)
        {
            waveTxt.transform.localScale = new Vector3(waveTxt.transform.localScale.x - Time.deltaTime, waveTxt.transform.localScale.y - Time.deltaTime, 0);
        }
    }


    public void SetCurrencyValue(CurrencyEnum cur, int val)
    {
        currencies.SetCurrency(cur, val);
    }

    public void AddCurrencyValue(CurrencyEnum cur, int val)
    {
        int curStatIndex = System.Enum.GetNames(typeof(CurrencyEnum)).Length;
        curStatIndex -= 3;
        curStatIndex += (int)cur;
        StatisticID sID = (StatisticID)curStatIndex;
        GameplayController.instance.AddStatistic(sID, val);
        int v = currencies.GetCurrency(cur);
        v += val;
        currencies.SetCurrency(cur, v);
    }

    public void UpdateCurrencyUI()
    {
        for(int i = 0 ; i < contents.Count ; i++)
        {
            int val = currencies.GetCurrency((CurrencyEnum)i);
            contents[i].UpdateValue(val);
        }
    }

    public void SetExp(float max, float val)
    {
        //sExp.maxValue = max;
        //sExp.value = val;
        exp.InitExp(max, val,0);
    }

    public void SetExp(float max, float val, int level, bool gradual = false)
    {
        //sExp.maxValue = max;
        //sExp.value = val;
        exp.InitExp(max, val,level, gradual);
    }

    public void Flash(System.Action during, System.Action after)
    {
        flashGO.color = Color.white;
        if(during != null)
        {
            during();
        }

        LeanTween.alpha(flashGO.GetComponent<RectTransform>(), .0f, 1.25f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            after();
        });
    }

    public void ShowVignette(Color c)
    {
        vignette.color = new Color(c.r,c.g,c.b,.0f);
        LeanTween.cancel(vignette.gameObject);
        LeanTween.alpha(vignette.GetComponent<RectTransform>(), 0.25f, 0.5f).setLoopType(LeanTweenType.pingPong);

        //LeanTween.cancel(vignetteCG.gameObject);
        LeanTween.value(vignetteCG.gameObject, .0f, 1.0f, 0.25f).setOnUpdate((float f)=>{
            vignetteCG.alpha = f;
        });
    }

    public void HideVignette()
    {
        StopCoroutine("ShowingVignette");
        LeanTween.cancel(vignetteCG.gameObject);
        LeanTween.value(vignetteCG.gameObject, vignetteCG.alpha, .0f, 0.25f).setOnUpdate((float f)=>{
            vignetteCG.alpha = f;
        }).setOnComplete(()=>{
            LeanTween.cancel(vignette.gameObject);
        });
    }

    public void ShowVignette(Color c, float duration)
    {
        StopCoroutine("ShowingVignette");
        StartCoroutine(ShowingVignette());
        IEnumerator ShowingVignette()
        {
            ShowVignette(c);
            yield return new WaitForSeconds(duration);
            HideVignette();
        }

    }
}

[System.Serializable]
public class Currencies
{
    public List<int> currencies;

    public Currencies()
    {
        currencies = new List<int>();
    }

    public int GetCurrency(CurrencyEnum id)
    {
        return currencies[(int)id];
    }

    public int SetCurrency(CurrencyEnum id, int val)
    {
        currencies[(int)id] = val;
        return currencies[(int)id];
    }
}

[System.Serializable]
public class ExpClass
{

}
