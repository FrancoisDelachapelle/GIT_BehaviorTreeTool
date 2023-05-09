using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PropertyNumeral
{
    public enum EComparisonSymbol
    {
        SUPERIOR,
        SUPERIOR_EQUAL,
        EQUAL,
        INFERIOR,
        INFERIOR_EQUAL
    }

    [SerializeField] private string firstComparer = "firstKey";
    [SerializeField] private string secondComparer = "secondKey";
    [SerializeField] private EComparisonSymbol comparisonSymbol = EComparisonSymbol.SUPERIOR;
    public string FirstComparer => firstComparer;
    public string SecondComparer => secondComparer;

    public PropertyNumeral()
    {
        
    }
}
