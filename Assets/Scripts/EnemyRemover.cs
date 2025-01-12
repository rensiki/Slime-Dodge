using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRemover : MonoBehaviour
{
    public bool isLeft;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            //Debug.Log("Enemy Detected by EnemyRemover");
            if(isLeft){
                if(other.GetComponent<Enemy>().xMoveValue < 0)
                {
                    Debug.Log("Enemy Destroyed by EnemyRemover");
                    other.gameObject.SetActive(false);
                }
            }
            else{
                if(other.GetComponent<Enemy>().xMoveValue > 0)
                {
                    Debug.Log("Enemy Destroyed by EnemyRemover"); 
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
}
