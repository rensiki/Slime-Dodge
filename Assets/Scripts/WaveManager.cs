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
            Debug.Log(this.patternSize);
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
            int patternValue = enemyPattern[patternIndex];
            patternIndex++;
            return patternValue;
        }
    }

    int stage = 0;
    int waveSize = 10;
    bool nowStage = false;
    Wave leftWave;
    Wave rightWave;

    public Transform rightSpawnPoint;
    public Transform leftSpawnPoint;
    public List<GameObject> enemyGameObjects;//0부터 9까지 총 10가지 적을 가지고 있음


    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)&&!nowStage)
        {
            StartStage();
        }
    }

    void StartStage()
    {
        stage++;
        waveSize = stage*5;
        leftWave = new Wave(waveSize, stage);
        rightWave = new Wave(waveSize, stage);
        nowStage = true;
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
                if (leftPattern != -1)
                {
                    Debug.Log("Left Pattern : " + leftPattern);
                }
                if (rightPattern != -1)
                {
                    Debug.Log("Right Pattern : " + rightPattern);
                }
            }
        }
    }
}
