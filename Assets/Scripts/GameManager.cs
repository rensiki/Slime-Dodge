using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI UITurn;

    public GameObject Slime1;


    int turn; //private로 설정해서 set,get으로 접근하는게 좋을듯
    int turnCounter = 0;
    float playerTurnTime = 0.5f;
    float enemyTurnTime = 0.5f;
    bool isPlayerTurn = false;
    bool isEnemyTurn = false;
    
    void Update()
    {
        UITurn.text = "Turn : " + turn;
        EnemyInstantiate();
    }
    public int getTurn()
    {
        return turn;
    }
    public void addTurn()
    {
        turn++;
        StartCoroutine(TurnChecker());
    }

    IEnumerator TurnChecker()
    {
        isPlayerTurn = true;
        yield return new WaitForSeconds(playerTurnTime);
        isPlayerTurn = false;
        isEnemyTurn = true;

        yield return new WaitForSeconds(enemyTurnTime);
        isEnemyTurn = false;
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
