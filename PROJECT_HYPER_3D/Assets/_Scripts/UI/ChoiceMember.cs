using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceMember : MonoBehaviour
{
    [SerializeField] int memberID = 0;
    [SerializeField] ChoiceAttribute attribute;
    [SerializeField] Image choiceMemberBG;
    [SerializeField] Image choiceMemberImg;
    [SerializeField] TextMeshProUGUI choiceDescription;
    [SerializeField] TextMeshProUGUI choiceValueTxt;
    [SerializeField] GameObject mover;
    [SerializeField] CanvasGroup cg;
    [SerializeField] SoundManager choiceSound;

    private void OnValidate() {
        attribute = new ChoiceAttribute();
    }

    public void ResetMember()
    {
        attribute = new ChoiceAttribute();
        choiceMemberImg.sprite = null;
        choiceDescription.text = "{reset}";
        choiceValueTxt.text = "-1";
    }

    public void FillMember(ChoiceAttribute attribute, int memberID = 0)
    {
        this.memberID = memberID;
        this.attribute = attribute;
        this.choiceDescription.text = System.Enum.GetName(typeof(ChoiceMemberID), attribute.choiceID).Replace("_"," "); 
        this.choiceValueTxt.text = "+"+attribute.GetValue().ToString();
        choiceMemberImg.sprite = ChoiceManager.instance.GetImages(attribute.choiceID);
    
    }

    public ChoiceAttribute GetChoiceAtrribute()
    {
        return attribute;
    }

    public void Hide()
    {
        LeanTween.cancel(cg.gameObject);
        LeanTween.cancel(mover.gameObject);

        cg.interactable = false;
        cg.blocksRaycasts = false;
        LeanTween.value(cg.gameObject, cg.alpha, .0f, 0.125f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
        }).setOnComplete(()=>{
            
        });

        LeanTween.moveLocalX(mover, 150.0f, 0.25f).setEase(LeanTweenType.easeOutQuad);


    }

    public void Show(int order = 0)
    {
        float pitchAddition = memberID / 5;
        choiceSound.PlayPitchAdd(0, pitchAddition);
        LeanTween.cancel(cg.gameObject);
        LeanTween.cancel(mover.gameObject);
        cg.alpha = 0;
        choiceMemberBG.color = ChoiceManager.instance.GetTierBG();
        mover.transform.localPosition = new Vector3(150.0f, .0f, .0f);

        cg.interactable = false;
        cg.blocksRaycasts = false;
        LeanTween.value(cg.gameObject, cg.alpha, 1.0f, 0.125f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
        }).setOnComplete(()=>{
            
        });

        LeanTween.moveLocalX(mover, .0f, 0.25f).setEase(LeanTweenType.easeOutQuad);
    }

    public void ModifyCG(bool mod)
    {
        cg.interactable = mod;
        cg.blocksRaycasts = mod;
    }

    public void Choose()
    {
        choiceSound.Play(1);
        ChoiceManager.instance.Choose(memberID);
    }
}
