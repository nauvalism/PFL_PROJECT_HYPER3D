using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    public static ChoiceManager instance {get;set;}

    private void Awake() {
        instance = this;
    }

    [SerializeField] int currentTier = 0;
    [SerializeField] List<Sprite> powerupImgs;
    [SerializeField] List<Color> tierColors;
    [SerializeField] GameObject choiceMemberPrefab;
    [SerializeField] Transform choiceMemberParent;
    [SerializeField] List<ChoiceMember> toBeFilledChoiceMembers;
    [SerializeField] List<ChoiceAttribute> choiceAttributes;
    [SerializeField] List<ChoicePowerups> powerupValueList;
    
    
    [Header("Countdown")]
    [SerializeField] TextMeshProUGUI countDownTxt;
    [SerializeField] Image countDownFiller;
    [SerializeField] float currentSecond = 0;
    [SerializeField] float defaultSecond = 30;

    [Header("GRAPHICS")]
    [SerializeField] CanvasGroup choiceCG;

    private void Update() {
        
        
        // if(Input.GetKeyDown(KeyCode.V))
        // {
        //     AssignChoices();
        // }

        // if(Input.GetKeyDown(KeyCode.B))
        // {
        //     ShowChoices(false);
        // }
        
        // if(Input.GetKeyDown(KeyCode.N))
        // {
        //     HideChoices(false);
        // }






        if(countDownTxt.transform.localScale.x > 1)
        {
            countDownTxt.transform.localScale = new Vector3(countDownTxt.transform.localScale.x - Time.deltaTime, countDownTxt.transform.localScale.y - Time.deltaTime, 0);
        }
    }

    public void ShowChoiceCG()
    {
        LeanTween.cancel(choiceCG.gameObject);
        LeanTween.value(choiceCG.gameObject, .0f, 1.0f, 0.25f).setOnUpdate((float f)=>{
            choiceCG.alpha = f;
        }).setOnComplete(()=>{
            choiceCG.interactable = true;
            choiceCG.blocksRaycasts = true;
        });
    }

    public void HideChoiceCG()
    {
        choiceCG.interactable = false;
        choiceCG.blocksRaycasts = false;
        LeanTween.cancel(choiceCG.gameObject);
        LeanTween.value(choiceCG.gameObject, 1.0f, .0f, 0.125f).setOnUpdate((float f)=>{
            choiceCG.alpha = f;
        }).setOnComplete(()=>{
            
        });
    }

    public void ShowChoices(bool instant)
    {
        AssignChoices();
        StartCoroutine(ShowingChoices());
        IEnumerator ShowingChoices()
        {

            ResetGraphic();
            ShowChoiceCG();
            
            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                toBeFilledChoiceMembers[i].ModifyCG(false);
            }
            yield return new WaitForEndOfFrame();
            float delay = (instant ? 0 : 0.125f);
            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                toBeFilledChoiceMembers[i].Show();
                yield return new WaitForSeconds(delay);
            }


            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                toBeFilledChoiceMembers[i].ModifyCG(true);
            }

            StartCountDown();
        }
    }

    public void HideChoices(bool instant)
    {
        StopCountingDown();
        StartCoroutine(HidingChoices());
        IEnumerator HidingChoices()
        {
            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                toBeFilledChoiceMembers[i].ModifyCG(false);
            }

            float delay = (instant ? 0 : 0.125f);
            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                toBeFilledChoiceMembers[i].Hide();
                yield return new WaitForSeconds(delay);
            }
    
            HideChoiceCG();
            GameplayController.instance.SetState(GameState.MidGame);
            
        }
        
    }


    public void AssignChoices()
    {
        GameplayController.instance.SetState(GameState.LevelUp);
        ResetChoices();
        RandomizeChoices();
    }

    public void ResetChoices()
    {
        for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
        {
            toBeFilledChoiceMembers[i].ResetMember();
        }
    }

    public void RandomizeChoices()
    {
        choiceAttributes = new List<ChoiceAttribute>();
        
        int _type = 0;
        
        for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
        {
            _type = Random.Range(0,System.Enum.GetNames(typeof(ChoiceMemberID)).Length);
            ChoiceMemberID __type = (ChoiceMemberID)_type;
            while(ContainsTypeIn(__type))
            {
                _type = Random.Range(0,System.Enum.GetNames(typeof(ChoiceMemberID)).Length);
                __type = (ChoiceMemberID)_type;
            }
            Debug.Log(_type);
            ChoiceAttribute c = new ChoiceAttribute();
            
            c.Generate(currentTier);
            choiceAttributes.Add(c);
            int value = powerupValueList[currentTier].GetValue(__type);
            choiceAttributes[i].FillValue(__type, value);


            toBeFilledChoiceMembers[i].FillMember(choiceAttributes[i]);
        }



        bool ContainsTypeIn(ChoiceMemberID ty)
        {
            for(int i = 0 ; i < toBeFilledChoiceMembers.Count ; i++)
            {
                if(toBeFilledChoiceMembers[i].GetChoiceAtrribute().choiceID == ty)
                {
                    return true;
                }
            }

            return false;
        }
    }
    

    public Sprite GetImages(ChoiceMemberID cMem)
    {
        Debug.Log("Requested : "+cMem);
        return powerupImgs[(int)cMem];
    }

    public void StartCountDown()
    {
        LeanTween.cancel(countDownFiller.gameObject);
        
        LeanTween.value(countDownFiller.gameObject, 1.0f, .0f, defaultSecond).setOnUpdate((float f)=>{
            countDownFiller.fillAmount = f;
        });

        StartCoroutine("CountingDown");
    }

    IEnumerator CountingDown()
    {
        currentSecond = defaultSecond;
        for(int i = (int)defaultSecond ; i > 0 ; i--)
        {
            currentSecond = i;
            UpdateCountDownText();
            yield return new WaitForSeconds(1);
        }

        currentSecond = 0;
        UpdateCountDownText();
        HideChoices(false);
    }

    void UpdateCountDownText()
    {
        countDownTxt.text = ((int)currentSecond).ToString();
        countDownTxt.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
    }


    public void Choose(int memberID)
    {
        HideChoices(false);
        ChoiceAttribute ca = choiceAttributes[memberID];
        GameplayController.instance.ApplyStat(ca);
    }

    public void AddTier()
    {

    }

    public Color GetTierBG()
    {
        return tierColors[currentTier];
    }

    public void StopCountingDown()
    {
        StopCoroutine("CountingDown");
        LeanTween.cancel(countDownFiller.gameObject);
    }

    public void ResetGraphic()
    {
        //StopAllCoroutines();
        StopCountingDown();
        currentSecond = defaultSecond;
        UpdateCountDownText();
        countDownFiller.fillAmount = 1;
        //LeanTween.cancel(choiceCG.gameObject);
    }
}





[System.Serializable]
public class ChoiceAttribute
{
    public int tier = 0;
    public ChoiceMemberID choiceID;
    public List<int> values;

    public ChoiceAttribute()
    {
        tier = 0;
        choiceID = ChoiceMemberID.Max_Health_Points;
    }

    public void Generate(int tier)
    {
        values = new List<int>();
        for(int i = 0 ; i < System.Enum.GetNames(typeof(ChoiceMemberID)).Length ; i++)
        {
            values.Add(0);
        }
    }

    public void FillValue(ChoiceMemberID cID, int v)
    {
        Debug.Log("CID : "+(int)cID+"--"+v);
        this.choiceID = cID;
        values[(int)choiceID] = v;
        //values[0] = v;
    }
    
    public int GetValue()
    {
        return values[(int)choiceID];
    }
}

[System.Serializable]
public class ChoicePowerups
{
    public List<int> values;

    public int GetValue(ChoiceMemberID id)
    {
        return values[(int)id];
    }
}