using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public GameManager gm;
    Transform E_trans;


    public int E_Turn=0;
    int S1count = 0;
    bool callingE_Move = false;
    int E_xPos = 0;

    public int test = 0;
    void Awake(){
        E_trans = GetComponent<Transform>();
    }

    void Update(){
        TurnCheck();
    }

    void FixedUpdate()
    {
        SlerpMoving();
    }

    void SlerpMoving()
    {
        if (callingE_Move)
        {
            Debug.Log("E_Move");
            E_trans.position = Vector3.Slerp(new Vector3(E_trans.position.x, E_trans.position.y, E_trans.position.z), new Vector3(E_xPos, 0.001f, 0), 0.3f);

        }
    }
       
    void TurnCheck(){
        //Debug.Log(gm.turn);

        if(gm.turn > E_Turn)
        {
            Debug.Log("턴체크");
            E_Turn = gm.turn;


            if(gameObject.tag == "Slime1")
            {
                if(S1count == 0)
                {
                    spriteRenderer.sprite = sprites[1];
                    Debug.Log("s1웅크림");
                    S1count = 1;
                }   
                else if(S1count == 1)//두번째 턴부터 점프. 착각 주의
                {
                    spriteRenderer.sprite = sprites[2];//점프 스프라이트 필요
                    Debug.Log("s1점프");
                    StartCoroutine(E_Mover());
                }

            }



        }
    }

    IEnumerator E_Mover()
    {
        Debug.Log("E_Mover");
        E_trans.position = new Vector3(E_trans.position.x, E_trans.position.y, E_trans.position.z);
        callingE_Move = true;
        yield return new WaitForSeconds(0.5f);
        callingE_Move = false;
        Debug.Log("E_MoverEnd");

        S1count = 0;
        spriteRenderer.sprite = sprites[0];
    }




    
}
