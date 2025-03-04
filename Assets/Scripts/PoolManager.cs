using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] pools;// GameObject 리스트 배열. 배열속 리스트들의 개수는 prfabs의 원소의 개수와 동일
    public int nowEnemyNum = 0;//WaveManager에서 enemy를 소환할 때마다 증가시켜주고, enemy가 사망할 때마다 감소시켜줌.

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
        //Instantiate(select, new Vector3(0, 3, 0), Quaternion.identity);//디버깅용
        return select;//알맞은 gameObject 반환해줌
    }

    public void Release(GameObject obj)//이건 왜필요하지? 알아서 도착하면 비활성화 되지않나?=>아 플레이어에 의한 사망 처리!
    {
        obj.SetActive(false);
    }

    public void PoolsMoving()
    {
        //int enemyNum = 0;
        for(int i = 0; i < pools.Length; i++)
        {
            foreach(GameObject item in pools[i])
            {
                if(item.activeSelf)
                {
                    //Debug.Log("pool moving");
                    Enemy enemy = item.GetComponent<Enemy>();
                    enemy.StartCoroutine(enemy.MovingFuncStart());
                    //enemyNum++;
                }
            }
        }
        /*SATGE관리는 WaveManager에서 처리하자자
        if(enemyNum == 0)
        {
            if(GameManager.Instance.wave.leftWave.getCurrentPattern() == -1 
            && GameManager.Instance.wave.rightWave.getCurrentPattern() == -1)//모든 웨이브가 소환되었을 때
            {
                GameManager.Instance.wave.nowStage = false;
                Debug.Log("Stage Cleared by PoolManager");
            }
        }*/
    }
}
