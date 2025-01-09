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



    // 메인 코드 시작

    int stage = 0;
    int waveSize = 10;
    public bool nowStage = false;//pool에서 적을 소환할지 말지 결정하는 변수.. 스테이지가 진행중인지 아닌지는 명확히 모름... 수정 필요=>false로 바꿔주는 코드를 제거하고 poolmanager에서 접근할 수 있도록 변경해야함
    public Wave leftWave;
    public Wave rightWave;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    float leftSpawnPointXnum = 0;
    float rightSpawnPointXnum = 0;
    bool isLeft = true;//왼쪽에서 소환할지 오른쪽에서 소환할지 결정하는 변수
    int pauseCount = 2;//몇번째 턴부터 멈출지 결정하는 변수
    int turnCounter = 0;//pause후에 몇턴이 지났는지 기록하는 변수
    

    void Start()
    {
        leftSpawnPointXnum = leftSpawnPoint.position.x;//초기 스폰포인트의 x값 저장
        rightSpawnPointXnum = rightSpawnPoint.position.x;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)&&!nowStage)
        {
            StartStage();
        }
        if (Input.GetKeyDown(KeyCode.Return)&&!nowStage)
        {
            stage++;//디버그용 스테이지 증가 코드
        }
    }

    public void StartStage()
    {
        if(nowStage)
        {
            Debug.Log("Already started!");
            return;
        }
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
        if (nowStage)//스테이지가 진행중이 아니면 함수 실행 안함
        {
            if(pauseTurn())//pauseCount만큼의 턴마다 소환을 쉬어가도록 설정
            {
                Debug.Log("Pause Turn");
                return;
            }
            int leftPattern = leftWave.getCurrentPattern();
            int rightPattern = rightWave.getCurrentPattern();
            SpwanPointChange();//스폰포인트 위치 변경

            if (isLeft)
            {
                if (leftPattern != -1)
                {
                    if (SpawnPointRaycast(leftSpawnPoint))//스폰포인트에 적이 있으면 스폰하지 않음==>확률적으로 소환을 잠시 쉼(물론 스폰포인트 이동으로 인해 상쇄될 수 있음)
                    {
                        Debug.Log("leftSpawnPoint is blocked!");
                    }
                    else
                    {
                        //test
                        GameObject left = GameManager.Instance.pool.Get(leftPattern);
                        left.transform.position = leftSpawnPoint.position;
                        left.transform.rotation = leftSpawnPoint.rotation;
                        //기본적으로 오른쪽으로 이동이긴 함.. 근데 바꿔줘야함
                        if(left.GetComponent<Enemy>().xMoveValue < 0){
                            left.GetComponent<Enemy>().reverse_xMoveValue();//오른쪽으로 이동하도록 변경해야 하기 때문에 양수로 바꿔줌
                        }
                        leftWave.increasePatternIndex();//다음 패턴으로 넘겨주면서, 사용한 패턴은 삭제처리
                        Debug.Log("Left Pattern : " + leftPattern);
                    }
                }
                isLeft = false;
            }
            else
            {
                if (rightPattern != -1)
                {
                    if (SpawnPointRaycast(rightSpawnPoint))//스폰포인트에 적이 있으면 스폰하지 않음
                    {
                        Debug.Log("rightSpawnPoint is blocked!");
                    }
                    else
                    {
                        //test
                        GameObject right = GameManager.Instance.pool.Get(rightPattern);
                        right.transform.position = rightSpawnPoint.position;
                        right.transform.rotation = rightSpawnPoint.rotation;
                        if(right.GetComponent<Enemy>().xMoveValue > 0){
                            right.GetComponent<Enemy>().reverse_xMoveValue();//왼쪽으로 이동하도록 변경해야 하기 때문에 음수로 바꿔줌
                        }
                        rightWave.increasePatternIndex();//다음 패턴으로 넘겨주면서, 사용한 패턴은 삭제처리
                        Debug.Log("Right Pattern : " + rightPattern);
                    }
                }
                isLeft = true;
            }
        }
        else{
            //Debug.Log("Stage Started by SpwanEnemy()");
            //StartStage();
        }
    }

    bool SpawnPointRaycast(Transform trans){
        int rayLength = 6;
        Vector3 rayPosition = new Vector3(trans.position.x, trans.position.y, 3);//ray의 시작이 z축으로 3만큼 여유공간 있도록 설정
        RaycastHit2D hit = Physics2D.Raycast(rayPosition, new Vector3(0,0,-1), rayLength);
        Debug.DrawRay(rayPosition, new Vector3(0,0,-1) * rayLength, Color.red, 5f);

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
    
    bool pauseTurn()
    {
        turnCounter++;
        if(turnCounter >= pauseCount)
        {
            turnCounter = 0;
            return true;
        }
        else{ return false; }
    }

}
