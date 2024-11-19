using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public class Wave
    {
        public int waveSize;
        
        private int[] enemyPattern;

        public Wave(List<GameObject> enemyList, int waveSize)
        {
            this.waveSize = waveSize;
            //패턴 생성. 받은 enemy 종류의 개수, n에 따라 0에서 n까지의 값을 가짐 
            makePattern(enemyList.Count);

        }

        void makePattern(int listSize)
        {
            enemyPattern = new int[waveSize];
            for (int i = 0; i < waveSize; i++)
            {
                enemyPattern[i] = Random.Range(0, listSize);
            }
        }

        public int getPattern(int index)
        {
            return enemyPattern[index];
        }
    }

    private int stage;

    public Transform spawnPoint;
    public List<GameObject> enemyGameObjects;

    private Wave currentWave;
    private int waveSize = 0;

    private void StartWave()
    {
        stage = 0;
        waveSize = 10;
        //stage가 증가한만큼 적의 종류도 증가
        List<GameObject> copyList = enemyGameObjects.GetRange(0, stage);
        Wave rightWave = new Wave(copyList, waveSize);
        Wave leftWave = new Wave(copyList, waveSize);

        for (int i = 0; i < waveSize; i++)
        {
            int curPattern = currentWave.getPattern(i);
            foreach(int enemyIndex in curPattern)
            {
                SpwanEnemy(enemyIndex);
            }
        }
        Debug.Log("All waves completed!");
    }
    void SpwanEnemy(int enemyIndex)
    {
        //스폰
        Instantiate(enemyGameObjects[enemyIndex], spawnPoint.position, spawnPoint.rotation);
    }
}
