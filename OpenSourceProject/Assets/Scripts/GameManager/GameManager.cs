using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using static UnityEditor.PlayerSettings;

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
            var randEnemy = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
            var enemyInstance = Instantiate(randEnemy);

            Vector2 worldPos;
            float radius = 1f;
            while (true)
            {
                Vector2 pos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
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

            // enemyinstance DOTween���� ��ġ �̵�
            enemyInstance.transform.position = worldPos;
            

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

        //UI���� �� �̺�Ʈ�� �������� ���� UI ǥ�� ����
        OnStageEnd?.Invoke();

        SaveStageData();

        PauseTime(true);
    }
}

