using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{

    //싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static GameManager _instance;//GameManager내부에서 자기 자신을 하나로 제한하고, 외부에 자기 자신을 전달하기 위한 변수인듯
    //인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance //GameManager.Instance로 접근 가능
    {//+)static이라 이 클래스에 종속되지 않음
        get
        {
            //인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        //인스턴스가 존재하는 경우 새로 생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        //아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }



    public TextMeshProUGUI UITurn;
    public WaveManager wave;
    public PoolManager pool;


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
        //wave.SpwanPointChange();
        wave.SpwanEnemy();
        pool.PoolsMoving();
        StartCoroutine(TurnChecker());
        Handheld.Vibrate();//진동은 배터리 엄청 소모하니까, 플레이어가 설정해서 켜고 끌 수 있도록 해야함
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
