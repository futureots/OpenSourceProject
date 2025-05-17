using System.Collections;
using UnityEngine;
using BulletSystem;

/// <summary>
/// 적의 동작과 상태를 관리하는 클래스입니다.
/// GameManager에 의해 화면 밖에서 생성되며, 한 번 이동 후 고정됩니다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class Enemy : MonoBehaviour, IBulletHitAble
{
    #region Fields

    /// <summary>적의 색상 (White 또는 Black)</summary>
    [SerializeField]
    private BulletColor enemyColor = BulletColor.White;

    /// <summary>최대 체력</summary>
    [SerializeField]
    private int maxHp = 3;

    /// <summary>현재 체력</summary>
    private int currentHp;

    /// <summary>공격 패턴</summary>
    [SerializeField]
    private AttackPatternType pattern = AttackPatternType.TowardPlayer;

    /// <summary>초당 발사 횟수</summary>
    [SerializeField]
    private float fireRate = 1f;

    /// <summary>총알 속도</summary>
    [SerializeField]
    private float bulletSpeed = 8f;

    /// <summary>총알 피해량</summary>
    [SerializeField]
    private float bulletDamage = 1f;

    /// <summary>Burst 발사 개수</summary>
    [SerializeField]
    private int burstCount = 3;

    /// <summary>Burst 연속 발사 간격</summary>
    [SerializeField]
    private float burstInterval = 0.1f;

    /// <summary>사망 시 획득 점수</summary>
    [SerializeField]
    private int scoreValue = 10;

    /// <summary>적이 살아있는지 여부</summary>
    private bool isAlive = true;

    #endregion

    #region Unity Methods

    /// <summary>
    /// 생성 직후 호출되며, 체력 및 색상 초기화 후 공격 루틴을 시작합니다.
    /// </summary>
    private void Start()
    {
        InitializeHp();
        InitializeColor();
        StartCoroutine(AttackRoutine());
    }

    #endregion

    #region Initialization

    // 체력을 최대치로 초기화합니다.
    private void InitializeHp()
    {
        currentHp = maxHp;
    }

    // 스프라이트 색상을 enemyColor에 맞추어 설정합니다.
    private void InitializeColor()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = (enemyColor == BulletColor.White) ? Color.white : Color.black;
    }

    #endregion

    #region Attack

    // 적의 공격을 처리하는 코루틴입니다.
    private IEnumerator AttackRoutine()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(1f / fireRate);
            switch (pattern)
            {
                case AttackPatternType.TowardPlayer:
                    FireTowardPlayer();
                    break;
                case AttackPatternType.Radial:
                    FireRadial(8);
                    break;
                case AttackPatternType.Burst:
                    yield return StartCoroutine(FireBurst());
                    break;
            }
        }
    }

    // 플레이어 방향으로 단발 총알을 발사합니다.
    private void FireTowardPlayer()
    {
        Vector2 dir = (GameManager.Instance.player.transform.position - transform.position).normalized;
        BulletManager.LaunchBullet(enemyColor, transform.position, bulletSpeed, bulletDamage, dir, gameObject);
    }

    // 지정 개수만큼 360도 방사형 공격을 수행합니다.
    private void FireRadial(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = 2f * Mathf.PI / count * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            BulletManager.LaunchBullet(enemyColor, transform.position, bulletSpeed, bulletDamage, dir, gameObject);
        }
    }

    // Burst 공격을 연속으로 발사하는 코루틴입니다.
    private IEnumerator FireBurst()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Vector2 dir = (GameManager.Instance.player.transform.position - transform.position).normalized;
            BulletManager.LaunchBullet(enemyColor, transform.position, bulletSpeed, bulletDamage, dir, gameObject);
            yield return new WaitForSeconds(burstInterval);
        }
    }

    #endregion

    #region IBulletHitAble Implementation

    /// <summary>
    /// 해당 색상의 총알과 상호작용할 수 있는지 여부를 반환합니다.
    /// </summary>
    /// <param name="color">충돌한 총알 색상</param>
    /// <returns>같은 색일 경우 true를 반환합니다.</returns>
    public bool CheckHitAble(BulletColor color, Bullet bullet)
    {
        return color == enemyColor;
    }

    /// <summary>
    /// 총알과 충돌했을 때 호출되며, 데미지를 적용합니다.
    /// </summary>
    /// <param name="damage">총알의 피해량</param>
    /// <param name="bullet">충돌한 Bullet 객체</param>
    public void Hit(float damage, Bullet bullet)
    {
        if (!isAlive) return;
        ApplyDamage(damage);
    }

    #endregion

    #region Damage & Death

    // 데미지를 적용하고 HP가 0 이하일 경우 사망을 처리합니다.
    private void ApplyDamage(float damage)
    {
        currentHp -= Mathf.CeilToInt(damage);
        if (currentHp <= 0)
            Die();
        else
            StartCoroutine(FlashOnHit());
    }

    // 피격 시 빨간색으로 잠시 반짝합니다.
    private IEnumerator FlashOnHit()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color orig = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = orig;
    }

    // 사망 시 점수를 추가하고 오브젝트를 파괴합니다.
    private void Die()
    {
        isAlive = false;
        GameManager.Instance.AddPoint(scoreValue);
        Destroy(gameObject);
    }

    #endregion
}