using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialGridContent : MonoBehaviour
{
    [SerializeField] Transform rootJumpScaler;
    [SerializeField] Image img;
    [SerializeField] Sprite spr;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int value = 0;

    private void OnValidate() {
        if(spr != null)
        {
            img.sprite = spr;
        }

        if(text != null)
        {
            text.text = value.ToString();
        }
    }

    private void Update() {
        if(rootJumpScaler.transform.localScale.x > 1)
        {
            rootJumpScaler.transform.localScale = new Vector3(rootJumpScaler.transform.localScale.x - Time.deltaTime, rootJumpScaler.transform.localScale.y - Time.deltaTime, 0);
        }

    }

    public void RefreshText()
    {
        
        text.text = value.ToString();
        rootJumpScaler.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

    }

    public void UpdateValue(int _value)
    {
        this.value = _value;
        RefreshText();
    }
}
