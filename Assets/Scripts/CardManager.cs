using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    class Hand
    {
        public Hand(int Hand_Num, Transform MyPos)
        {
            hand_Num = Hand_Num;
            myPos = MyPos;
        }
        public string getCardName() {
            return hand_card.name;
        }
        public Card getCard()
        {
            if (hasCard == false)
            {
                return null;
            }
            if (hand_card == null)
            {
                Debug.LogWarning(hand_Num + ": hand is null!");
            }
            hasCard = false;
            return hand_card;
        }
        int hand_Num { get; set; }
        bool hasCard = false;
        Transform myPos;
        Card hand_card;
    }
    public List<Card> all_casds = new List<Card>();
    public List<Transform> hand_Pos = new List<Transform>();
    public List<Card> myCards = new List<Card>();
    Hand[] hands;

    void Start()
    {
        hands = new Hand[4] {new Hand(0, hand_Pos[0])
        , new Hand(1, hand_Pos[1]), new Hand(2, hand_Pos[2]), new Hand(3, hand_Pos[3])};

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            activateCard(hands[0].hand_card.name);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
        }
    }
    void activateCard(string name) {
        if (name == null)
        {
            Debug.LogWarning("card's name is null!");
        }

        if (name == "OneAttackCard")
        {
            Debug.Log("OneAttackCard activated!");
        }
        else if (name == "BothAttackCard")
        {
            Debug.Log("BothAttackCard activated!");
        }
        else if (name == "ShootingAttackCard")
        {
            Debug.Log("ShootingAttackCard activated!");
        }
    }
    

    
}