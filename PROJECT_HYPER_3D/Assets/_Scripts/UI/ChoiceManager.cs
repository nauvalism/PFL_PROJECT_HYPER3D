using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] GameObject choiceMemberPrefab;
    [SerializeField] Transform choiceMemberParent;
    [SerializeField] List<ChoiceMember> spawnedChoiceMember;
    
    
}





[System.Serializable]
public class ChoiceAttribute
{
    public ChoiceMemberID choiceID;
    public MainCharacterAttribute attributes;
    public int value = 10;
    
}