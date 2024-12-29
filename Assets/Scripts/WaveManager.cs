using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public class Wave
    {
        int waveSize = 0;
        int patternSize = 0;
        int patternIndex = 0;
        List<int> enemyPattern = new List<int>();

        public Wave(){}

        public Wave(int waveSize, int patternSize)
        {
            this.waveSize = waveSize;
            this.patternSize = patternSize;
            Debug.Log("wave's patternSize: "+this.patternSize);
            makePattern();
        }

        void makePattern()
        {
            for(int i = 0; i < waveSize; i++)
            {
                //Debug.Log(Random.Range(0, patternSize));
                enemyPattern.Add(Random.Range(0, patternSize));//0부터 patternSize-1까지의 랜덤한 숫자를 생성
            }
            Debug.Log("Pattern created!");
            for(int i = 0; i < waveSize; i++)
            {
                Debug.Log(enemyPattern[i]);
            }
        }

        public int getCurrentPattern()
        {  
            if (patternIndex >= waveSize)
            {
                Debug.Log("Wave completed!");
                return -1;
            }
            /* 패턴 인덱스 반환만 함. 소환 한 후에 증가시키는건 다른 함수에서 처리
            int patternValue = enemyPattern[patternIndex];
            patternIndex++;*/
            return enemyPattern[patternIndex];
        }

        public void increasePatternIndex()
        {
            patternIndex++;
        }
    }

    int stage = 0;
    int waveSize = 10;
    bool nowStage = false;
    Wave leftWave;
    Wave rightWave;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    float leftSpawnPointXnum = 0;
    float rightSpawnPointXnum = 0;

    void Start()
    {
        leftSpawnPointXnum = leftSpawnPoint.position.x;
        rightSpawnPointXnum = rightSpawnPoint.position.x;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)&&!nowStage)
        {
            StartStage();
        }
    }

    public void StartStage()
    {
        stage++;
        waveSize = stage*5;
        leftWave = new Wave(waveSize, stage);
        rightWave = new Wave(waveSize, stage);
        nowStage = true;
    }

    public void SpwanPointChange(){
        int leftRandom = Random.Range(0, 3);//0,1,2 중 랜덤으로 선택. 이 값만큼 기존 위치에서 멀어짐
        int rightRandom = Random.Range(0, 3);//0,1,2 중 랜덤으로 선택. 이 값만큼 기존 위치에서 멀어짐
        //왼쪽 스폰포인트 변경
        leftSpawnPoint.position = new Vector3(leftSpawnPointXnum-leftRandom, leftSpawnPoint.position.y, leftSpawnPoint.position.z);
        //오른쪽 스폰포인트 변경
        rightSpawnPoint.position = new Vector3(rightSpawnPointXnum+rightRandom, rightSpawnPoint.position.y, leftSpawnPoint.position.z);
    }

    public void SpwanEnemy()
    {
        if (nowStage)
        {
            int leftPattern = leftWave.getCurrentPattern();
            int rightPattern = rightWave.getCurrentPattern();
            if (leftPattern == -1 && rightPattern == -1)
            {
                nowStage = false;
                Debug.Log("All waves completed!");
            }
            else
            {
                SpwanPointChange();//스폰포인트 위치 변경

                if (leftPattern != -1)
                {
                    if(SpawnPointRaycast(leftSpawnPoint))//스폰포인트에 적이 있으면 스폰하지 않음==>확률적으로 소환을 잠시 쉼(물론 스폰포인트 이동으로 인해 상쇄될 수 있음)
                    {
                        Debug.Log("leftSpawnPoint is blocked!");
                    }
                    else
                    {
                        //test
                        GameObject left = GameManager.Instance.pool.Get(leftPattern);
                        left.transform.position = leftSpawnPoint.position;
                        left.transform.rotation = leftSpawnPoint.rotation;//기본적으로 오른쪽으로 이동이기 때문에 설정 필요 없음
                        leftWave.increasePatternIndex();//다음 패턴으로 넘겨주면서, 사용한 패턴은 삭제처리
                        Debug.Log("Left Pattern : " + leftPattern);
                    }
                }
                if (rightPattern != -1)
                {
                    if(SpawnPointRaycast(rightSpawnPoint))//스폰포인트에 적이 있으면 스폰하지 않음
                    {
                        Debug.Log("rightSpawnPoint is blocked!");
                    } 
                    else{
                        //test
                        GameObject right = GameManager.Instance.pool.Get(rightPattern);
                        right.transform.position = rightSpawnPoint.position;
                        right.transform.rotation = rightSpawnPoint.rotation;
                        right.GetComponent<Enemy>().reverse_xMoveValue();//왼쪽으로 이동하도록 변경해야 하기 때문에 음수로 바꿔줌
                        rightWave.increasePatternIndex();//다음 패턴으로 넘겨주면서, 사용한 패턴은 삭제처리
                        Debug.Log("Right Pattern : " + rightPattern);
                    }
                }
            }
        }
    }
    bool SpawnPointRaycast(Transform trans){
        //Debug.Log("Raycast");
        RaycastHit2D hit = Physics2D.Raycast(trans.position, new Vector3(0,0,-1), 10);
        if (hit.collider != null)
        {
            if(hit.collider.tag == "Enemy")
            {
                Debug.Log(hit.collider.name);
                hit.collider.GetComponent<Transform>().position = new Vector3(hit.collider.GetComponent<Transform>().position.x, 4,0);
                return true;
            }
        }
        return false;
    }
    

}
