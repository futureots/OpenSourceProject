using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 적 종류
    public List<GameObject> enemyPrefabs;

    // 현재 획득한 점수
    public int point {  get; private set; }
    /// <summary>
    /// 점수를 추가하는 함수
    /// </summary>
    /// <param name="point">획득량</param>
    public void AddPoint(int point)
    {
        this.point += point;
    }
    // 스테이지에서 버틴 시간
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
    // 일정 시간마다 적을 스폰하는 코루틴
    IEnumerator SpawnEnemy(float delay)
    {
        while (true)
        {
            var randEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
            var enemyInstance = Instantiate(randEnemy);

            // enemyinstance dotween으로 위치 이동(적 내부 함수)

            yield return new WaitForSeconds(delay);
        }
    }
}
