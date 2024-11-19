using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    Rigidbody2D rigid;

    public int xMoveValue = 1;
    public float speed = 0.1f;
    public float a = 5f;
    public float movingDelayTime = 0.01f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //StartCoroutine(Test());
        

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            StartCoroutine(MovingFunc(transform.position.x, xMoveValue, transform.position.y, getAbyxMove()));
        }
    }

    float getAbyxMove(){
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



    IEnumerator MovingFunc(float origin, float xMoveValue, float Yorigin, float A = 1, float x=0, float y=0)  
    {
        while ((xMoveValue>0&&x < xMoveValue) || (xMoveValue < 0 && x > xMoveValue))
        {
            x += speed * xMoveValue;//2차함수에서의 점의 x축 방향 이동
            y = -A * (x - 0) * (x - (xMoveValue)); 
            //Debug.Log("x : " + (origin + x) + " y : " + (Yorigin + y));

            // 오브젝트 위치 갱신
            transform.position = new Vector3(origin + x, Yorigin + y, transform.position.z);
            yield return new WaitForSeconds(movingDelayTime);
        }
        //약간의 오차를 의도했던 값으로 이동시킴으로써 조정
        transform.position = new Vector3(origin + xMoveValue, Yorigin, transform.position.z);
    }


    
}
