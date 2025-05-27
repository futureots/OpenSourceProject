using BulletSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public Player player;
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

    /// <summary>
    /// 스테이지 종료 시 실행
    /// </summary>
    public UnityEvent OnStageEnd;

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

    // 일정 시간마다 적을 스폰하는 코루틴
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
                // 무한 루프 방지
                yield return new WaitForSeconds(0.5f);
                radius *= 0.9f;
            }
            var enemyInstance = Instantiate(randEnemy, worldPos, Quaternion.identity);

            // 엔티티 랜덤 설정
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
            if (count * delay > 20)
            {
                count -= 20f;
                delay *= 0.8f;
                level++;
            }
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

    /// <summary>
    /// 현재 스테이지 정보(스테이지 레벨, 점수, 시간) 저장
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
    /// 스테이지 종료 함수(플레이어 사망 시 호출)
    /// </summary>
    public void GameEnd()
    {
        Debug.Log("GameEnd");

        StopAllCoroutines();

        SaveStageData();

        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(3f);

        //UI에서 이 이벤트에 스테이지 종료 UI 표시 설정
        OnStageEnd?.Invoke();
        //UI 표시 및 레벨 화면으로 이동

        PauseTime(true);
    }
}

