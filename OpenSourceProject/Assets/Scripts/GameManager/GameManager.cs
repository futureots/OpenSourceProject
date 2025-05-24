using BulletSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// �������� �ܰ�
    /// </summary>
    public int stageNumber;
    /// <summary>
    /// �� ����
    /// </summary>
    public List<GameObject> enemyPrefabs;

    public Player player;
    /// <summary>
    /// ���� ȹ���� ����
    /// </summary>
    public int point {  get; private set; }
    /// <summary>
    /// ������ �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="point">ȹ�淮</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }

    /// <summary>
    /// ������������ ��ƾ �ð�
    /// </summary>
    public float time { get; private set; }

    /// <summary>
    /// �������� ���� �� ����
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

    // ���� �ð����� ���� �����ϴ� �ڷ�ƾ
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
                // ���� ���� ����
                yield return new WaitForSeconds(0.1f);
                radius *= 0.9f;
            }
            var enemyInstance = Instantiate(randEnemy, worldPos, Quaternion.identity);

            // ��ƼƼ ���� ����
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
    /// �ð��� ���ߴ� �Լ�(�Ͻ����� �ɼ� �Ǵ� ���� ���� �� ���)
    /// </summary>
    /// <param name="isPause">true�� ����, false�� ���</param>
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
    /// ���� �������� ����(�������� ����, ����, �ð�) ����
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
    /// �������� ���� �Լ�(�÷��̾� ��� �� ȣ��)
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

        //UI���� �� �̺�Ʈ�� �������� ���� UI ǥ�� ����
        OnStageEnd?.Invoke();
        //UI ǥ�� �� ���� ȭ������ �̵�

        PauseTime(true);
    }
}

