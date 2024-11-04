using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    Rigidbody2D rigid;
    Transform trans;

    public int testing = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();

        

    }

    void Update()
    {
    }
    void FixedUpdate()
    {
        trans.position = Vector3.Slerp(trans.position, new Vector3(testing, 0.01f, 0), 0.3f);
    }





}
