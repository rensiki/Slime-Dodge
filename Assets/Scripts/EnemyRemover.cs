using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRemover : MonoBehaviour
{
    public bool isLeftRemover = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            //Debug.Log("Enemy Detected by EnemyRemover");
            if(isLeftRemover)
            {
                if(other.GetComponent<Enemy>().xMoveValue < 0)
                {
                    Debug.Log("Enemy Destroyed by EnemyRemover");
                    other.GetComponent<Enemy>().EnemyActiveFalse();
                }
            }
            else{
                if(other.GetComponent<Enemy>().xMoveValue > 0)
                {
                    Debug.Log("Enemy Destroyed by EnemyRemover"); 
                    other.GetComponent<Enemy>().EnemyActiveFalse();
                }
            }
        }
    }
}
