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
    float playerTurnTime = 0.5f;
    float enemyTurnTime = 0.5f;
    bool isPlayerTurn = false;
    bool isEnemyTurn = false;
    
    void Update()
    {
        UITurn.text = "Turn : " + turn;
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

}
