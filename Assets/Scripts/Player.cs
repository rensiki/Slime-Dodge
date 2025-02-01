using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    /// <summary>
    /// 이 게임의 핵심인 플레이어의 이동만을 담당하는 코드. 플레이어 오브젝트에 붙어있음.
    /// 나머지 일반공격이나 스킬과 같은 터치 조작들은 GameManager에서 처리함.
    /// 즉, 자이로스코프 기능은 이곳에서만 사용할 것이고, 나머지 터치 조작은 GameManager에서 처리할 것임.(플레이어 캐릭터와 터치 조작의 독립성 보장)
    /// </summary>



    // Start is called before the first frame update
    Rigidbody2D rb;
    Transform trans;



    //bool isMoving = false;//GameManager에서 총 관리함. 턴 개념과 깊게 연관된 변수이기 때문
    bool isTwoTouching = false;
    bool isAttackAble = false;
    bool callingMove = false;

    //디버거 변수
    bool DListouch1Ended = false;


    int xPos = 0;
    float time = 0;

    enum MovingState
    {
        LEFT = -1,
        RIGHT = 1
    }

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
        PCmoving();//키보드로 이동하는 디버그용 함수
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        LerpMoving();
    }

    void PlayerMove(int dir)//플레이어를 이동시키는 함수들을 총 관리하는 함수. 실질적으로 주어진 int값인 dir만큼 이동시켜줌
    {
        //StartCoroutine(MovingDelay(movingDelayTime));//calling moving로 대체 가능     //GameManager에서 총 관리함. 턴 개념과 깊게 연관된 변수이기 때문
        StartCoroutine(Moving(dir));//주어진dir에 따라 플레이어를 이동시킴
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
                if (Input.gyro.rotationRateUnbiased.y > 2 && !GameManager.Instance.isMoving)
                {
                    MoveCallingSetting(MovingState.RIGHT);
                }
                else if (Input.gyro.rotationRateUnbiased.y < -2 && !GameManager.Instance.isMoving)
                {
                    MoveCallingSetting(MovingState.LEFT);
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

    void MoveCallingSetting(MovingState state)
    {
        if (GameManager.Instance.isMoving)//이동 딜레이중에는 방향도 바꿀 수 없도록 이곳에다 빠져나가는 코드 추가
        {
            return;
        }  


        if (state == MovingState.LEFT)
        {
            Debug.Log("Left Dashed");
            PlayerMove(-1);//왼쪽으로 한칸 이동
            if (transform.rotation.y != 180)//회전해서 왼쪽을 보도록 설정
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            GameManager.Instance.addTurn();
        }
        else if(state == MovingState.RIGHT)
        {
            Debug.Log("Right Dashed");
            PlayerMove(1);//오른쪽으로 한칸 이동
            if (transform.rotation.y != 0)//회전해서 오른쪽을 보도록 설정
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            GameManager.Instance.addTurn();
        }
    }

    void PCmoving()//키보드로 이동하는 디버그용 함수
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveCallingSetting(MovingState.LEFT);
        }
    
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCallingSetting(MovingState.RIGHT);
        }
    }

    void TwoTouchDebuger()
    {
        isTwoTouching = false;

    }

    /*//GameManager에서 총 관리함. 턴 개념과 깊게 연관된 변수이기 때문
    //다른 코루틴으로 분리해야 디버그 가능
    IEnumerator MovingDelay(float time)
    {
        isMoving = true;
        yield return new WaitForSeconds(time);
        isMoving = false;
    }*/


    IEnumerator Moving(int Pdir)
    {
        float movingTime = 0.5f; 
        if(xPos>-8&&xPos<8){
            xPos += Pdir;
            //Debug.Log(xPos);
        }
        else{
            trans.position = new Vector2(0,0);
            xPos = 0;
        }
        callingMove = true;
        yield return new WaitForSeconds(movingTime);
        callingMove = false;
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
                        //GameManager.Instance.addTurn();//공격시 턴을 추가하지 않지만, 바라보는 방향에만 공격 가능한 한계부여하자.
                    }

                    time = 0;
                    isAttackAble = false;
                }
            }
        }
    }
}
