using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] pools;// GameObject 리스트 배열. 배열속 리스트들의 개수는 prfabs의 원소의 개수와 동일


    void Awake() 
    {
        pools = new List<GameObject>[prefabs.Length];//각 몬스터 프리펩마다 전용 pool이 존재.
        for(int i = 0; i< pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)//index는 몬스터 종류 선택.
    {
        GameObject select = null;
        // ... 선택한 풀의 놀고 있는(비활성화 된) 게임오브젝트 접근
        foreach (GameObject item in pools[index]){
            if(!item.activeSelf){
                // ... 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        if(!select){
            // ... 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }

    public void Release(GameObject obj)//이건 왜필요하지? 알아서 도착하면 비활성화 되지않나?=>아 플레이어에 의한 사망 처리!
    {
        obj.SetActive(false);
    }

    public void PoolsMoving()
    {
        for(int i = 0; i < pools.Length; i++)
        {
            foreach(GameObject item in pools[i])
            {
                if(item.activeSelf)
                {
                    //Debug.Log("pool moving");
                    Enemy enemy = item.GetComponent<Enemy>();
                    enemy.StartCoroutine(enemy.MovingFunc());
                }
            }
        }
    }
}
