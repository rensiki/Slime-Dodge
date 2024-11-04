using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{


    public int turn = 0;
    int turnCounter = 0;

    public TextMeshProUGUI UITurn;

    public GameObject Slime1;
    
    void Update()
    {
        UITurn.text = "Turn : " + turn;
        EnemyInstantiate();
    }

    void EnemyInstantiate()
    {
        if (turn > turnCounter)
        {
            turnCounter = turn;
            Invoke("InvokeSlime", 0.5f);

        }
        
        
        
    }

    void InvokeSlime()
    {
        if (turn == 3)
        {
            Debug.Log("s1생성3");
            Instantiate(Slime1, new Vector2(0, 0), Slime1.transform.rotation);
        }
        if (turn == 5)
        {
            Debug.Log("s1생성5");
            Instantiate(Slime1, new Vector2(2, 0), Slime1.transform.rotation);
        }
    }

}
