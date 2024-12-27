using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    Transform trans;



    bool isMoving = false;
    bool isTwoTouching = false;
    bool isAttackAble = false;
    bool callingMove = false;

    //디버거 변수
    bool DListouch1Ended = false;


    int xPos = 0;
    float time = 0;
    float movingDelayTime = 1f;

    void Awake()
    {
        Input.gyro.enabled = true; // enable gyro
        trans = GetComponent<Transform>(); // get transform
        rb = GetComponent<Rigidbody2D>(); // get rigidbody
    }

    void Update()
    {
        //Debug.Log(Input.gyro.rotationRateUnbiased.y); // log rotation
        PlayerMoveCalling(); // 2번 이상 터치 후 자이로로 이동
        //TwoTouchDebuger(); // 2번 터치 후 때는 도중 1번 터치로 입력되어 공격하는 것 방지
        Attack(); //1번 터치시 공격

        //test
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        LerpMoving();

    }


    void PlayerMoveCalling()
    {
        if(Input.touchCount > 0){

            Touch touch0 = Input.GetTouch(0);
            if (Input.touchCount > 1)
            {

                isTwoTouching = true;//공격이 안되는 상태
                Touch touch1 = Input.GetTouch(1);

                //if문으로 2개 터치중 할 필요 없을듯 
                //왜냐면 2개 터치중이 아닐때는 아래의 코드가 실행되지 않기 때문
                if (Input.gyro.rotationRateUnbiased.y > 2 && !isMoving)
                {
                    Debug.Log("Right Dashed");
                    PlayerMove(1);
                    GameManager.Instance.addTurn();
                }
                else if (Input.gyro.rotationRateUnbiased.y < -2 && !isMoving)
                {
                    Debug.Log("Left Dashed");
                    PlayerMove(-1);
                    GameManager.Instance.addTurn();
                }

                //isTwoTouching을 false로 처리해주는 로직

                else if (touch1.phase == TouchPhase.Ended||touch0.phase == TouchPhase.Ended)
                {
                    DListouch1Ended = true;
                }
            }
            if(DListouch1Ended){
                if(touch0.phase == TouchPhase.Ended)
                    {
                        Debug.Log("둘다 땜");
                        //두번 터치 후 딜레이를 줘서 공격이 실수로 실행되는것 방지
                        //공격 딜레이랑 똑같긴 한데, 애니메이션 안넣었을때는 조금 어색함
                        //턴 개념이랑 엮어서 애니메,사운드로 잘 보안하면 디버깅 성공일듯
                        Invoke("TwoTouchDebuger", 0.5f);
                        DListouch1Ended = false;
                    }
            }
        }
    }

    void TwoTouchDebuger()
    {
        isTwoTouching = false;

    }

    void PlayerMove(int dir)
    {
        StartCoroutine(MovingDelay(movingDelayTime));//calling moving로 대체 가능
        StartCoroutine(Moving(dir));

        //주어진dir에 따라 플레이어를 이동시킴
    }


//다른 코루틴으로 분리해야 디버그 가능
    IEnumerator MovingDelay(float time)
    {
        isMoving = true;
        yield return new WaitForSeconds(time);
        isMoving = false;
    }

    IEnumerator Moving(int Pdir)
    {
        if(xPos>-8&&xPos<8){
            xPos += Pdir;
            Debug.Log(xPos);
        }
        else{
            trans.position = new Vector2(0,0);
            xPos = 0;
        }
        callingMove = true;
        yield return new WaitForSeconds(0.5f);
        callingMove = false;


        /*
        trans.position = Vector2.Lerp(trans.position
        , new Vector2(trans.position.x + Pdir, trans.position.y), Time.deltaTime * 10);
        yield return null;*/
    } 

    void LerpMoving(){
        if(callingMove){
            trans.position = Vector2.Lerp(trans.position, new Vector2(xPos, 0), 0.3f);
        }

    }


    void Attack()
    {

        if (Input.touchCount == 1 && !isTwoTouching)
        {
            Touch touch = Input.GetTouch(0);
            //Debug.Log(touch.phase);

            

            if (touch.phase == TouchPhase.Began){
                isAttackAble = true;
            }

            if(isAttackAble)
            {
                // 터치 시작 지점에서 0.1초 동안만 처리
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    time += Time.deltaTime;
                    //Debug.Log(time);

                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (time < 0.1f && time > 0.0001f)
                    {
                        Debug.Log("공격");
                        GameManager.Instance.addTurn();
                    }

                    time = 0;
                    isAttackAble = false;
                }
            }
        }
    }
}
