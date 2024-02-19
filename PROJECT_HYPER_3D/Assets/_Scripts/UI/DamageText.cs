using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] TextMeshPro text;
    [SerializeField] Transform mover;
    [SerializeField] Transform aScaler;

    private void OnValidate() {
        text.text = dmg.ToString();
    }

    public void DisplayText(int dmg, Transform tParent)
    {
        this.dmg = dmg;
        text.text = dmg.ToString();
        StartCoroutine(DisplayingText());

        IEnumerator DisplayingText()
        {
            LeanTween.cancel(text.gameObject);
            LeanTween.cancel(mover.gameObject);
            LeanTween.cancel(aScaler.gameObject);
            LeanTween.value(text.gameObject, .0f, 1.0f, .50f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                text.alpha = f;
            });
            LeanTween.moveLocalY(mover.gameObject, mover.transform.localPosition.y + .50f, 1.0f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.scale(aScaler.gameObject, new Vector3(1.25f,1.25f,1.25f), .750f).setEase(LeanTweenType.punch);
            yield return new WaitForSeconds(1.250f);
            LeanTween.value(text.gameObject, 1.0f, .0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                text.alpha = f;
            });
            LeanTween.moveLocalY(mover.gameObject, mover.transform.localPosition.y + 1.50f, 1.25f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
                Destroy(gameObject);
            });
        }

        
    }
}
