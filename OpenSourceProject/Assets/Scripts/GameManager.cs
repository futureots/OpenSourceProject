using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 스테이지 단계
    /// </summary>
    public int stageNumber;
    /// <summary>
    /// 적 종류
    /// </summary>
    public List<GameObject> enemyPrefabs;

    /// <summary>
    /// 현재 획득한 점수
    /// </summary>
    public int point {  get; private set; }
    /// <summary>
    /// 점수를 추가하는 함수
    /// </summary>
    /// <param name="point">획득량</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }

    /// <summary>
    /// 스테이지에서 버틴 시간
    /// </summary>
    public float time { get; private set; }

    private void Start()
    {
        point = 0;
        time = 0;
        SpawnEnemy(5f);
        SaveStageData();
    }
    private void Update()
    {
        time += Time.deltaTime;

    }

    /// 일정 시간마다 적을 스폰하는 코루틴
    IEnumerator SpawnEnemy(float delay)
    {
        while (true)
        {
            var randEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var enemyInstance = Instantiate(randEnemy);

            // enemyinstance DOTween으로 위치 이동

            yield return new WaitForSeconds(delay);
        }
    }
    /// <summary>
    /// 시간을 멈추는 함수(일시정지 옵션 또는 게임 종료 시 사용)
    /// </summary>
    /// <param name="isPause">true는 정지, false는 재생</param>
    public static void PauseTime(bool isPause)
    {
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        
    }

    public void SaveStageData()
    {
        StageData stageData = new StageData(point, time);
        var data = PlayerData.LoadPlayerData("Data");
        if (data.data.ContainsKey(stageNumber))
        {
            data.data[stageNumber] = stageData;
        }
        else
        {
            data.data.Add(stageNumber, stageData);
        }
        data.SavePlayerData("Data");
    }

    
}

