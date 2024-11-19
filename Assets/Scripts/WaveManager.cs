using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public class Wave
    {
        private int waveSize;
        
        private int[] enemyPattern;
        private int patternIndex = 0;
        private Transform spawnPoint;

        public Wave(List<GameObject> enemyList, int waveSizem, Transform SpawnPoint)
        {
            this.waveSize = waveSize;
            spawnPoint = SpawnPoint;
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

        public int getCurrentPattern()
        {
            return enemyPattern[patternIndex];
            patternIndex++;
        }
    }

    private int stage = -1;
    private int waveSize = 10;

    public Transform rightSpawnPoint;
    public Transform leftSpawnPoint;
    public List<GameObject> enemyGameObjects;//0부터 10까지 총 11가지 적을 가지고 있음
    

    private void StartWave()
    {
        stage++;
        waveSize = 10;
        //stage가 증가한만큼 적의 종류도 증가
        List<GameObject> copyList = enemyGameObjects.GetRange(0, stage);
        Wave rightWave = new Wave(copyList, waveSize, rightSpawnPoint);
        Wave leftWave = new Wave(copyList, waveSize, leftSpawnPoint);

        Debug.Log("All waves completed!");
    }
}
