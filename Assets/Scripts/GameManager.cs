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


    public TextMeshProUGUI UITurn;
    public TextMeshProUGUI UIJellyStone;
    public TextMeshProUGUI UIbubbleMana;
    public TextMeshProUGUI UIbubbleTeaLevel;
    public WaveManager wave;
    public PoolManager pool;
    public GameObject player;
    public CameraMoving camera;
    bool isMoving = false;//플레이어가 이동할 때 접근해서 판단할 수 있어야함//private로 설정해서 set,get으로 접근하는게 좋을듯
    float movingDelayTime = 0.5f;//플레이어가 이동하는 시간. 즉, 적이 이동하는 enemy turn time이라고 보면 됨. 매우 세심하게 조정해야함.
    Transform playerTrans;
    int turn; //private로 설정해서 set,get으로 접근하는게 좋을듯
    int bubbleTeaLevel = 1;//초기상태에서 사용해도 마나 얻을 수 있도록 1로 설정
    int bubbleTeaEnhancement = 0;
    int bubbleMana = 0;
    int PlayerTwoBlockAttackMana = 10;
    int PlayerBothAttackMana = 20;
    public int ToalJellyStone = 0;

    public bool getIsMoving(){return isMoving;}
    public void InitializeStatus(){ bubbleTeaLevel = 0; bubbleMana = 0; }


    private void Awake()
    {
        playerTrans = player.GetComponent<Transform>();



        //------------------------싱글톤 패턴------------------------
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



    void Update()
    {
        UITurn.text = "Turn : " + turn;
        UIJellyStone.text = "JellyStone : " + ToalJellyStone;
        UIbubbleMana.text = "BubbleMana : " + bubbleMana;
        UIbubbleTeaLevel.text = "BubbleTeaLevel : " + bubbleTeaLevel;

        if (Input.GetKeyDown(KeyCode.W))
        {
            DrinkBubbleTea();
            Debug.Log("DrinkBubbleTea");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerNormalAttack();
            Debug.Log("PlayerNormalAttack");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (bubbleMana < PlayerTwoBlockAttackMana){return;}
            bubbleMana -= PlayerTwoBlockAttackMana;
            PlayerTwoBlockAttack();
            Debug.Log("PlayerTwoBlockAttack");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bubbleMana < PlayerBothAttackMana) { return; }
            bubbleMana -= PlayerBothAttackMana;
            PlayerBothAttack();
            Debug.Log("PlayerBothAttack");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameSave();
            Debug.Log("GameSaved");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameLoad();
            Debug.Log("GameLoaded");
        }
    }
    public int getTurn()
    {
        return turn;
    }
    public void addTurn()
    {
        if (!isMoving)
        {
            turn++;
            isMoving = true;
            StartCoroutine(MovingDelay());//플레이어 코드 속에 있던 함수에서 이쪽으로 이동시킴. 오류 없도록 주의
            //wave.SpwanPointChange();
            wave.SpwanEnemy();
            pool.PoolsMoving();
            camera.setCameraMoving(true);
            //Handheld.Vibrate();//진동은 배터리 엄청 소모하니까, 플레이어가 설정해서 켜고 끌 수 있도록 해야함
        }
    }
    public void addJellyStone(int stoneValue)
    {
        ToalJellyStone += stoneValue;
    }

    public void GameSave()
    {
        PlayerPrefs.SetInt("JellyStone", ToalJellyStone);
        PlayerPrefs.Save();
    }
    public void GameLoad()
    {
        ToalJellyStone = PlayerPrefs.GetInt("JellyStone");
    }

    IEnumerator MovingDelay()
    {
        if (isMoving)
        {
            Debug.Log("now moving");
            yield return new WaitForSeconds(movingDelayTime);
            PlayerHitCheck();
            camera.setCameraMoving(false);
            isMoving = false;
            Debug.Log("end moving");
        }
    }

    void PlayerHitCheck()
    {
        //Debug.Log("PlayerHitCheck");
        Vector3 rayPos = new Vector3(playerTrans.position.x, playerTrans.position.y, playerTrans.position.z+1);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, new Vector3(0, 0, -1), 3);
        Debug.DrawRay(rayPos, new Vector3(0, 0, -1)*3, Color.blue, 0.5f);
        if (hit.collider.gameObject.tag == "Enemy")
        {
            Debug.Log("player attacked!");
        }
    }
    
    void EnemyAttackingFunc(Vector3 RayPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(RayPos, new Vector3(0, 0, -1), 3);
        Debug.DrawRay(RayPos, new Vector3(0, 0, -1) * 3, Color.yellow, 0.5f);
        while (hit != false)
        {//enemy 외의 대상과 접촉하면 오류뜨는 구조. 레이어로 디버그 가능?
            hit.collider.gameObject.GetComponent<Enemy>().EnemyActiveFalse();
            hit = Physics2D.Raycast(RayPos, new Vector3(0, 0, -1), 3);
            Debug.DrawRay(RayPos, new Vector3(0, 0, -1) * 3, Color.yellow, 0.5f);
        }
    }

    public void PlayerNormalAttack()
    //
    {
        if (isMoving)
        {
            return;
        }

        if (playerTrans.rotation.y == 0)
        {
            Vector3 rayPos = new Vector3(playerTrans.position.x + 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
        }
        else
        {
            Vector3 rayPos = new Vector3(playerTrans.position.x - 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
        }
        addTurn();
    }

    void PlayerTwoBlockAttack()
    {
        if (isMoving)
        {
            return;
        }

        if (playerTrans.rotation.y == 0)
        {
            Vector3 rayPos = new Vector3(playerTrans.position.x + 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
            rayPos = new Vector3(playerTrans.position.x + 2, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
        }
        else
        {
            Vector3 rayPos = new Vector3(playerTrans.position.x - 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
            rayPos = new Vector3(playerTrans.position.x - 2, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
        }
        addTurn();
    }
    void PlayerBothAttack()
    {
        if (isMoving)
        {
            return;
        }
        {
            Vector3 rayPos = new Vector3(playerTrans.position.x + 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
            rayPos = new Vector3(playerTrans.position.x - 1, playerTrans.position.y, playerTrans.position.z + 1);
            EnemyAttackingFunc(rayPos);
        }
        addTurn();
    }

    public void BubbleTeaLevelUp()
    {
        Debug.Log("BubbleTeaLevelUp");
        bubbleTeaLevel++;
    }
    void DrinkBubbleTea()
    {
        if(isMoving)
        {
            return;
        }
        Debug.Log("DrinkBubbleTea");
        bubbleMana += bubbleTeaLevel;
        if (bubbleTeaLevel >= 5)
        {
            Debug.Log("버블티 레벨업->5+bubbleTeaEnhancement추가!");
            bubbleMana += 5 + bubbleTeaEnhancement;
        }
        bubbleTeaLevel = 1;
        addTurn();
    }

}
