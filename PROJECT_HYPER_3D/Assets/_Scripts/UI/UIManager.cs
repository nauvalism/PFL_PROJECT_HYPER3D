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
    [Header("Currencies")]
    [SerializeField] List<MaterialGridContent> contents;
    [SerializeField] Currencies currencies;
    


    public void SetCurrencyValue(CurrencyEnum cur, int val)
    {
        currencies.SetCurrency(cur, val);
    }

    public void AddCurrencyValue(CurrencyEnum cur, int val)
    {
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
        sExp.maxValue = max;
        sExp.value = val;
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
