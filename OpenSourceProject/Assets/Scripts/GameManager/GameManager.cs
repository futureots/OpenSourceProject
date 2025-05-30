using BulletSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ItemSystem;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 스테이지 단계(인덱스)
    /// </summary>
    public int stageNumber;
    /// <summary>
    /// 적 종류
    /// </summary>
    public List<GameObject> enemyPrefabs;

    public Player player;
    /// <summary>
    /// 해당 스테이지 점수
    /// </summary>
    public int point {  get; private set; }
    /// <summary>
    /// 점수 추가
    /// </summary>
    /// <param name="point">획득량</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }

    /// <summary>
    /// 해당 스테이지 시간
    /// </summary>
    public float time { get; private set; }

    /// <summary>
    /// 스테이지 종료 이벤트
    /// </summary>
    public UnityEvent OnStageEnd;
    
    /// <summary>
    /// 획득 가능한 
    /// </summary>
    public List<ItemInfo> AvailableItems;

    private void Start()
    {
        point = 0;
        time = 0;
        Player.OnPlayerDie += GameEnd;
        StartCoroutine(SpawnEnemy(5f));
        
    }
    private void Update()
    {
        time += Time.deltaTime;
    }
    

    public ItemInfo GetRandomItemInfo()
    {
        if (AvailableItems == null || AvailableItems.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, AvailableItems.Count);
        return AvailableItems[index];
    }

    // 적 랜덤 위치 스폰 코루틴
    IEnumerator SpawnEnemy(float delay)
    {
        if(delay <= 0) delay = 1f;
        float count = 0;
        while (true)
        {
            if (enemyPrefabs.Count <= 0) yield break;
            var randEnemy = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];



            Vector2 worldPos;
            float radius = 1f;
            while (true)
            {
                Vector2 pos = new Vector2(Random.Range(100, Screen.width - 100), Random.Range(100, Screen.height - 100));
                //Debug.Log(pos);
                worldPos = Camera.main.ScreenToWorldPoint(pos);
                //Debug.Log(worldPos);
                var colls = Physics2D.OverlapCircleAll(worldPos, radius);
                //Debug.Log(colls.Length);
                if (colls.Length == 0) break;
                // ���� ���� ����
                yield return new WaitForSeconds(0.5f);

                radius *= 0.9f;
            }
            var enemyInstance = Instantiate(randEnemy, worldPos, Quaternion.identity);

            var enemy = enemyInstance.GetComponent<Enemy>();
            int level = 0;
            int hp = Random.Range(level+1, level+10);
            int rate = Random.Range((int)(level*0.4f) + 1, (int)(level * 0.4f)+3);
            float speed = Random.Range(level * 0.3f + 1, level * 0.3f + 8f);
            BulletColor color = Random.Range(0, 2) == 0 ? BulletColor.Black : BulletColor.White;
            var mode = Random.Range(0, 3);
            switch (mode)
            {
                case 0:
                    enemy.Init(hp, rate, speed, color);
                    break;
                case 1:
                    int rad = Random.Range(4, level + 12);
                    enemy.Init(hp, rate, speed, color, rad);
                    break;
                case 2:
                    int burstCount = Random.Range(3, level + 5);
                    float inter = 0.5f / rate;
                    enemy.Init(hp, rate, speed, color, burstCount, inter);
                    break;
                default:
                    break;
            }


            Debug.Log("SpawnEnemy");
            yield return new WaitForSeconds(delay);
            count++;
            if (count > 10)
            {
                count = 0;
                delay *= 0.7f;
                level++;
            }
        }
    }
    /// <summary>
    /// TimeScale 정지
    /// </summary>
    /// <param name="isPause">true : 정지, false : 재생</param>
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

    /// <summary>
    /// Data.json에 플레이어 데이터 저장
    /// </summary>
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

    /// <summary>
    /// 게임 종료 호출 
    /// </summary>
    public void GameEnd()
    {
        Debug.Log("GameEnd");

        StopAllCoroutines();

        SaveStageData();

        OnStageEnd?.Invoke();

        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(3f);



        PauseTime(true);
    }
}

