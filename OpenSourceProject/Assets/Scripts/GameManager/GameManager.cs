using BulletSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ItemSystem;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 占쏙옙占쏙옙占쏙옙占쏙옙 占쌤곤옙
    /// </summary>
    public int stageNumber;
    /// <summary>
    /// 占쏙옙 占쏙옙占쏙옙
    /// </summary>
    public List<GameObject> enemyPrefabs;

    public Player player;
    /// <summary>
    /// 
    /// </summary>
    public int point {  get; private set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="point">획占썸량</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }

    /// <summary>
    /// 
    /// </summary>
    public float time { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public UnityEvent OnStageEnd;
    
    /// <summary>
    ///
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

    // 占쏙옙占쏙옙 占시곤옙占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙占싹댐옙 占쌘뤄옙틴
    IEnumerator SpawnEnemy(float delay)
    {
       if(delay <= 0) delay = 1f;
        while (true)
        {
            if (enemyPrefabs.Count <= 0) yield break;
            var randEnemy = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            
            

            Vector2 worldPos;
            float radius = 1f;
            while (true)
            {
                Vector2 pos = new Vector2(Random.Range(100, Screen.width-100), Random.Range(100, Screen.height-100));
                //Debug.Log(pos);
                worldPos = Camera.main.ScreenToWorldPoint(pos);
                //Debug.Log(worldPos);
                var colls = Physics2D.OverlapCircleAll(worldPos, radius);
                //Debug.Log(colls.Length);
                if (colls.Length == 0) break;
                // 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙
                yield return new WaitForSeconds(0.1f);
                radius *= 0.9f;
            }
            var enemyInstance = Instantiate(randEnemy, worldPos, Quaternion.identity);

            // 占쏙옙티티 占쏙옙占쏙옙 占쏙옙占쏙옙
            var enemy = enemyInstance.GetComponent<Enemy>();
            int hp = Random.Range(1, 10);
            int rate = Random.Range(1, 3);
            float speed = Random.Range(1, 8f);
            BulletColor color = Random.Range(0,2) == 0 ? BulletColor.Black : BulletColor.White;
            var mode = Random.Range(0, 3);
            switch (mode)
            {
                case 0:
                    enemy.Init(hp,rate,speed,color);
                    break;
                case 1:
                    int rad = Random.Range(4,12);
                    enemy.Init(hp, rate, speed, color, rad);
                    break;
                case 2:
                    int count = Random.Range(3, 5);
                    float inter = 0.5f/rate;
                    enemy.Init(hp, rate, speed, color,count, inter);
                    break;
                default:
                    break;
            }
            
            
            Debug.Log("SpawnEnemy");
            yield return new WaitForSeconds(delay);
        }
    }
    /// <summary>
    /// 占시곤옙占쏙옙 占쏙옙占쌩댐옙 占쌉쇽옙(占싹쏙옙占쏙옙占쏙옙 占심쇽옙 占실댐옙 占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙 占쏙옙占)
    /// </summary>
    /// <param name="isPause">true占쏙옙 占쏙옙占쏙옙, false占쏙옙 占쏙옙占</param>
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
    /// 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙(占쏙옙占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙, 占쏙옙占쏙옙, 占시곤옙) 占쏙옙占쏙옙
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
    /// 占쏙옙占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙 占쌉쇽옙(占시뤄옙占싱억옙 占쏙옙占 占쏙옙 호占쏙옙)
    /// </summary>
    public void GameEnd()
    {
        Debug.Log("GameEnd");

        OnStageEnd?.Invoke();

        StopAllCoroutines();

        SaveStageData();

        StartCoroutine(EndCoroutine());
    }

    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(3f);



        PauseTime(true);
    }
}

