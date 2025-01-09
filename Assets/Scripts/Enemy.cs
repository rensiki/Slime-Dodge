using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    Rigidbody2D rigid;

    //변수들이 public이라 안정성이 조금 떨어질 것 같긴 함..
    public int xMoveValue = 1;//x축으로 이동하는 거리
    public float speed = 0.1f;//이동하는 속도
    public float a = 5f;//2차함수의 a값. 즉 곡선의 높이
    public float movingDelayTime = 0.01f;

    int endx = 8; //이동이 끝나는 x값. 맵 크기 자체는 -8.5부터 8.5까지임

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //StartCoroutine(Test());
        a = getAbyxMove(a);

    }
    /*테스트용
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            StartCoroutine(MovingFunc());
        }
    }*/

    public void reverse_xMoveValue()//근데 변수가 public이라 큰 의미 없음
    {
        xMoveValue *= -1;
    }
    public void set_xMoveValue(int value)
    {
        xMoveValue = value;
    }

    float getAbyxMove(float a){
        if (xMoveValue == 1 || xMoveValue == -1)
        {
            return a;
        }
        else if (xMoveValue == 2 || xMoveValue == -2)
        {
            return a * 0.5f;
        }
        else if (xMoveValue == 3 || xMoveValue == -3)
        {
            return a * 0.3f;
        }
        else if (xMoveValue == 4 || xMoveValue == -4)
        {
            return a * 0.2f;
        }
        return 0;//에러케이스
        Debug.Log("xMove 범위 에러");
    }



    public IEnumerator MovingFunc()  
    {
        float origin = transform.position.x;//호출시점 x위치
        float Yorigin = transform.position.y;//호출시점 y위치
        float x = 0; float y = 0;//이동하는 과정에서의 변하는 x,y값

        /*
        if (origin >= endx && xMoveValue > 0)
        {
            //Debug.Log("도착상태라 사라짐");
            EnemyActiveFalse();
        }
        else if(origin <= -endx && xMoveValue < 0)
        {
            //Debug.Log("도착상태라 사라짐");
            EnemyActiveFalse();
        }*/


        while ((xMoveValue>0&&x < xMoveValue) || (xMoveValue < 0 && x > xMoveValue))
        {
            x += speed * xMoveValue;//2차함수에서의 점의 x축 방향 이동
            y = -a * (x - 0) * (x - (xMoveValue)); 
            //Debug.Log("x : " + (origin + x) + " y : " + (Yorigin + y));

            // 오브젝트 위치 갱신
            transform.position = new Vector3(origin + x, Yorigin + y, transform.position.z);
            yield return new WaitForSeconds(movingDelayTime);
        }
        //약간의 오차를 의도했던 값으로 이동시킴으로써 조정
        transform.position = new Vector3(origin + xMoveValue, Yorigin, transform.position.z);
    }

    void EnemyActiveFalse()//음.. 아직 필요성을 모르겠는데. 안정성이 좋아지긴 할라나?
    {
        gameObject.SetActive(false);
    }
    
}
