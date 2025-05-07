using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // �� ����
    public List<GameObject> enemyPrefabs;

    // ���� ȹ���� ����
    public int point {  get; private set; }
    /// <summary>
    /// ������ �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="point">ȹ�淮</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }
    // ������������ ��ƾ �ð�
    public float time { get; private set; }

    private void Start()
    {
        point = 0;
        time = 0;
    }
    private void Update()
    {
        time += Time.deltaTime;

    }
    // ���� �ð����� ���� �����ϴ� �ڷ�ƾ
    IEnumerator SpawnEnemy(float delay)
    {
        while (true)
        {
            var randEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var enemyInstance = Instantiate(randEnemy);

            // enemyinstance dotween���� ��ġ �̵�(�� ���� �Լ�)

            yield return new WaitForSeconds(delay);
        }
    }
}
