using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    Rigidbody2D rigid;
    Transform trans;

    public int destination = 1;
    public float speed = 0.001f;
    public float a = 0.1f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        //StartCoroutine(Test());
        

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            Moving(transform.position.x, destination);
        }
    }

    void Moving(float origin, float destination)
    {
        float x = origin;
        while (origin < destination)
        {
            x += speed;
            float y = -a * (x - origin) * (x - (origin + destination)) + 1;

            // 오브젝트 위치 갱신
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
    /*IEnumerator Test()
    {
        trans.position = Vector3.Slerp(trans.position, new Vector3(testing, 0.01f, 0), 0.3f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Test());
    }*/





}
