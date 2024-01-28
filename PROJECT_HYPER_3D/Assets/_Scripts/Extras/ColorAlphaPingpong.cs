using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorAlphaPingpong : MonoBehaviour
{
    public GameObject tweenObject;
    public float alphaDuration = 1.0f;
    public LeanTweenType easeType = LeanTweenType.linear;

    private void Start()
    {
        ResetAlpha();    
    }

    public void ResetAlpha()
    {

        if (tweenObject.GetComponent<SpriteRenderer>())
        {
            SpriteRenderer sr = tweenObject.GetComponent<SpriteRenderer>();
            tweenObject.GetComponent<SpriteRenderer>().color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
            foreach (Transform t in tweenObject.transform)
            {
                SpriteRenderer srr = t.GetComponent<SpriteRenderer>();
                t.GetComponent<SpriteRenderer>().color = new Color(srr.color.r, srr.color.g, srr.color.b, 0);
            }
        }
        else
        {
            Image img = tweenObject.GetComponent<Image>();
            tweenObject.GetComponent<Image>().color = new Color(img.color.r, img.color.g, img.color.b, 0);
            foreach (Transform t in tweenObject.transform)
            {
                Image imgg = tweenObject.GetComponent<Image>();
                t.GetComponent<Image>().color = new Color(imgg.color.r, imgg.color.g, imgg.color.b, 0);
            }
        }
    }

    public void DoPingPong()
    {
        SpriteRenderer sr = tweenObject.GetComponent<SpriteRenderer>();
        LeanTween.cancel(tweenObject);
        tweenObject.GetComponent<SpriteRenderer>().color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        LeanTween.alpha(tweenObject, .0f, alphaDuration).setLoopType(LeanTweenType.pingPong).setEase(easeType);
    }

    public void DoPingPongUI()
    {
        Image img = tweenObject.GetComponent<Image>();
        LeanTween.cancel(tweenObject);
        tweenObject.GetComponent<Image>().color = new Color(img.color.r, img.color.g, img.color.b, 1);
        LeanTween.alpha(tweenObject.GetComponent<RectTransform>(), .0f, alphaDuration).setEase(easeType).setLoopType(LeanTweenType.pingPong);
    }

    public void StopAlpha()
    {
        LeanTween.cancel(tweenObject);
        foreach (Transform t in tweenObject.transform)
        {
            LeanTween.cancel(t.gameObject);
        }
        ResetAlpha();
    }
}
