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
    [SerializeField] TextMeshProUGUI waveNotifier;
    [SerializeField] CanvasGroup waveNotifierCG;
    [SerializeField] TextMeshProUGUI waveCountDown;
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
            enemiesKilledTxt.transform.localScale = new Vector3(enemiesKilledTxt.transform.localScale.x - (Time.deltaTime * 2.5f), enemiesKilledTxt.transform.localScale.y - (Time.deltaTime * 5), 0);
        }

        if(waveTxt.transform.localScale.x > 1)
        {
            waveTxt.transform.localScale = new Vector3(waveTxt.transform.localScale.x - (Time.deltaTime * 2.5f), waveTxt.transform.localScale.y - (Time.deltaTime * 5), 0);
        }

        // if(Input.GetKeyDown(KeyCode.T))
        // {
        //     UpdateWave(2);
        // }

        // if(Input.GetKeyDown(KeyCode.Y))
        // {
        //     WaveComplete(3, null);
        // }
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
            if(after != null)
            {
                after();
            }
            
        });
    }

    public void ShowVignette(Color c)
    {
        Debug.Log("Showing Vignette");
        
        LeanTween.cancel(vignette.gameObject);
        vignette.color = new Color(c.r,c.g,c.b,.0f);
        LeanTween.alpha(vignette.GetComponent<RectTransform>(), 1.0f, 0.5f).setLoopType(LeanTweenType.pingPong);

        LeanTween.cancel(vignetteCG.gameObject);
        vignetteCG.alpha = 0;
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
        //StopCoroutine("ShowingVignette");
        StartCoroutine(ShowingVignette(c, duration));
        

    }

    IEnumerator ShowingVignette(Color c, float duration)
    {
        ShowVignette(c);
        yield return new WaitForSeconds(duration);
        HideVignette();
    }

    public void UpdateEnemyKilled(int value)
    {
        enemiesKilledTxt.text = value.ToString();
        enemiesKilledTxt.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
    }

    public void UpdateWave(int value)
    {
        waveTxt.text = value.ToString();
        waveTxt.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
        
        string _meessage = "Wave "+value+" Start!!";
        StartCoroutine(UpdatingWave(value, _meessage, null));
    }

    public void WaveComplete(int value, System.Action after)
    {
        waveTxt.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        string _message = "Wave "+value+" Complete";
        StartCoroutine(UpdatingWave(value, _message, after));
    }

    IEnumerator UpdatingWave(int value, string message, System.Action then)
    {
        LeanTween.cancel(waveNotifier.gameObject);
        LeanTween.cancel(waveNotifierCG.gameObject);
        
        waveNotifierCG.alpha = 0;
        waveNotifier.transform.localPosition = new Vector3(0, -50, .0f);

        waveNotifier.text = message;

        LeanTween.value(waveNotifierCG.gameObject, .0f, 1.0f, .25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            waveNotifierCG.alpha = f;
        });
        LeanTween.moveLocalY(waveNotifier.gameObject, .0f, .5f).setEase(LeanTweenType.easeOutQuad);

        yield return new WaitForSeconds(1.5f);

        LeanTween.value(waveNotifierCG.gameObject, 1.0f, .0f, .25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            waveNotifierCG.alpha = f;
        });
        LeanTween.moveLocalY(waveNotifier.gameObject, 50.0f, .5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            if(then != null)
            {
                then();
            }
        });
    }

    public void StartCountdownWave(System.Action after)
    {

        StartCoroutine(CountingDown());
        IEnumerator CountingDown()
        {
            for(int i = 3 ; i > 0 ; i--)
            {
                LeanTween.cancel(waveCountDown.gameObject);
                waveCountDown.transform.localScale = Vector3.zero;
                waveCountDown.transform.localPosition = new Vector3(0, waveCountDown.transform.localPosition.y, 0);
                waveCountDown.text = "WAVE STARTS IN "+i.ToString()+"...";
                waveCountDown.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                LeanTween.scale(waveCountDown.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
                waveCountDown.GetComponent<SoundManager>().Play(0);
                yield return new WaitForSeconds(1.0f);
            }

            LeanTween.cancel(waveCountDown.gameObject);
            waveCountDown.transform.localScale = Vector3.zero;
            waveCountDown.text = "WAVE START";
            waveCountDown.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
            LeanTween.scale(waveCountDown.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutExpo);
            waveCountDown.GetComponent<SoundManager>().Play(1);

            Flash(()=>{
                after();
            }, ()=>{
                LeanTween.moveLocalX(waveCountDown.gameObject, 1250.0f, 0.75f).setEase(LeanTweenType.easeOutQuad);
            });


            
            
        }
    }

    public void SyncGameOverStats(List<float> f)
    {
        gameOver.SyncValuesToUI(f);
    }

    public void ShowGameOver()
    {
        gameOver.ShowGameOverNotifier();
    }

    public void ShowOpening()
    {
        opening.ShowOpening();
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
