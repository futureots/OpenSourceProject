using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        point = 0;
        time = 0;
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
            var randEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var enemyInstance = Instantiate(randEnemy);

            // enemyinstance DOTween���� ��ġ �̵�

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

        //TODO : UI ���� ǥ�� �� �ð� ǥ��, ����ȭ�� �̵� ��ư


        SaveStageData();

        PauseTime(true);
    }
}

