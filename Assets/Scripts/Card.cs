using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] string name = "none";
    [SerializeField] int cost = 1;
    [SerializeField]int card_value = 1;
    [SerializeField] bool isOneOff = false;
    [SerializeField] int repeat = 0;
    [SerializeField] string enchant = "none";


}
/*
    public Card(string Name, int Cost, int Card_Value, int Repeat, bool IsOneOff = false)
    {
        name = Name; cost = Cost; card_value = Card_Value; repeat = Repeat;
        isOneOff = IsOneOff;
    }*/