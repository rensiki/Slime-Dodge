using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI UITurn;
    public GameObject WaveManager;


    int turn; //private로 설정해서 set,get으로 접근하는게 좋을듯
    float playerTurnTime = 0.5f;
    float enemyTurnTime = 0.5f;
    bool isPlayerTurn = false;
    bool isEnemyTurn = false;
    
    void Update()
    {
        UITurn.text = "Turn : " + turn;
        if(Input.GetKeyDown(KeyCode.A))//a키 입력
        {
            addTurn();
        }
    }
    public int getTurn()
    {
        return turn;
    }
    public void addTurn()
    {
        turn++;
        WaveManager.GetComponent<WaveManager>().SpwanEnemy();
        StartCoroutine(TurnChecker());
    }

    IEnumerator TurnChecker()//플레이어와 적의 행동을 유발시키는 이벤트 관리
    {
        isPlayerTurn = true;
        yield return new WaitForSeconds(playerTurnTime);
        isPlayerTurn = false;
        isEnemyTurn = true;
        yield return new WaitForSeconds(enemyTurnTime);
        isEnemyTurn = false;
    }

}
