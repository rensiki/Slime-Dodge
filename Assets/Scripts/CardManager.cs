using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    class Card
    {
        public Card(string Name, int Cost, int Card_Value, int Repeat, bool IsOneOff = false)
        {
            name = Name; cost = Cost; card_value = Card_Value; repeat = Repeat;
            isOneOff = IsOneOff;
        }
        string name { get; set; }
        int cost
        {
            get { return cost; }
            set { Debug.Log(name + ": cost를" + value + "으로 변경"); cost = value; }
        }
        int card_value
        {
            get { return card_value; }
            set { Debug.Log(name + ": card_value" + value + "으로 변경"); card_value = value; }
        }
        bool isOneOff;
        int repeat;
        string enchant = "none";
    }
    class Hand
    {
        public Hand(int Hand_Num)
        {
            hand_Num = Hand_Num;
        }
        int hand_Num{get; set;}
        Card hand_card{get; set;}
    }

    Dictionary<string, Card> all_cards
    = new Dictionary<string, Card>();

    void Start()
    {
        //구체적인 마나 소모량은 직접 플레이해보며 수정해봐야할듯. 혹은 테스터 모집!
        all_cards.Add("OneAttack", new Card("OneAttack", 1, 1, 0));
        //all_cards.Add("TwoAttack", new Card("TwoAttack", 5, 2, 1));
        //all_cards.Add("ThreeAttack", new Card("ThreeAttack", 10, 3, 2));
        all_cards.Add("OneBothAttack", new Card("OneBothAttack", 10, 3, 0));
        //all_cards.Add("TwoBothAttack", new Card("TwoBothAttack", 20, 5, 1));
        //all_cards.Add("ThreeBothAttack", new Card("ThreeBothAttack", 30, 7, 2));
        all_cards.Add("ShootingOneAttack", new Card("ShootingOneAttack", 20, 5, 0));
        //all_cards.Add("ShootingTwoAttack", new Card("ShootingTwoAttack", 30, 5, 1));
        //all_cards.Add("ShootingThreeAttack", new Card("ShootingThreeAttack", 40, 5, 2));

        Hand[] hands = new Hand[4] {new Hand(0), new Hand(1), new Hand(2), new Hand(3)};

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("DrinkBubbleTea");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("PlayerNormalAttack");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PlayerTwoBlockAttack");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("PlayerBothAttack");
        }
    }
    

    
}