using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using BulletSystem;

public class Player : MonoBehaviour, IBulletHitAble
{
    private Rigidbody2D rb;
    //private SpriteRenderer spriteRd;
    ColorConverter spriteRenderer;

    //---Moving---
    private Vector2 moveDirection;
    [SerializeField]
    private float moveSpeed;

    //---Looking---
    private Vector2 mouseScreenPos;
    private Camera mainCam;
    private PlayerAnimator animator;

    //---HP---
    [Header("Player Status")]
    public int currentHP;
    public int maxHP = 3;

    // Bullet

    /// <summary>
    /// 현재 총알의 색상입니다.
    /// </summary>
    public BulletColor playerColor = BulletColor.White;

    /// <summary>
    /// 발사하는 총알의 속도입니다.
    /// </summary>
    public float bulletSpeed = 8f;

    /// <summary>
    /// 발사하는 총알의 피해량입니다.
    /// </summary>
    public float bulletDamage = 1f;

    /// <summary>
    /// 총알을 발사하는 간격입니다.
    /// </summary>
    public float bulletCooldown = 0.1f;
    private float bulletCooldownTimer = 0f;

    public GameObject explosionEffect;

    //Die
    public delegate void PlayerDieHandler(); //type
    public static event PlayerDieHandler OnPlayerDie; //event
    private bool isDie = false;
    
    //Item Effects
    private bool isAnyColor = false;          // A 아이템: 색 무시
    private Coroutine rapidFireRoutine;       // S 아이템
    private Coroutine damageBuffRoutine;      // D 아이템

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //---Looking---
        mainCam = Camera.main;
        animator = GetComponentInChildren<PlayerAnimator>();

        //Change Color
        //spriteRd = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponentInChildren<ColorConverter>();
        spriteRenderer.SetColor(playerColor);
        //HP

        currentHP = maxHP;
    }


    void Update()
    {
        //---Moving---
        rb.linearVelocity = moveDirection * moveSpeed;

        //---Looking---
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dirVec = mouseWorldPos - (Vector2)transform.position;

        if (dirVec != Vector2.zero)
        {
            // 플레이어 스프라이트 조작
            float slope = dirVec.y / dirVec.x;
            if (slope > -1 && slope < 1)
            {
                if(dirVec.x > 0) animator.SetPlayerSprite(PlayerAnimator.Direction.Right);
                else animator.SetPlayerSprite(PlayerAnimator.Direction.Left);
            }
            else
            {
                if (dirVec.y > 0) animator.SetPlayerSprite(PlayerAnimator.Direction.Up);
                else animator.SetPlayerSprite(PlayerAnimator.Direction.Down);
            }
            
            //transform.up = dirVec.normalized;
            //float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
            //rb.rotation = angle; // z축 기준 회전
        }

        bulletCooldownTimer -= Time.deltaTime;
        if(bulletCooldownTimer <= 0)
        {
            BulletManager.LaunchBullet(
                color: playerColor,
                position: transform.position,
                speed: bulletSpeed,
                damage: bulletDamage,
                direction: dirVec.normalized,
                source: gameObject);

            bulletCooldownTimer = bulletCooldown;
        }
    }

    //------PLAYER CONTROL------
    /// <summary>
    /// wsad를 이용해 움직입니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Mouse 방향으로 회전합니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 좌클릭 => 흰색으로 변경합니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("좌클릭!");
            //spriteRd.color = Color.white;
            spriteRenderer.SetColor(BulletColor.White);
            playerColor = BulletColor.White;
        }
    }
    
    /// <summary>
    /// 우클릭 => 검은색으로 변경합니다.
    /// </summary>
    /// <param name="context"></param>
    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("우클릭!");
            //spriteRd.color = Color.black;
            spriteRenderer.SetColor(BulletColor.Black);
            playerColor = BulletColor.Black;
        }
    }

    /// <summary>
    /// 플레이어의 hp를 관리합니다.
    /// </summary>
    /// <param name="hp"></param>
    public void ChangeHp(int hp)
    {
        currentHP += hp;
        if (currentHP > maxHP)
            currentHP = maxHP;
        else if (currentHP <= 0 && !isDie)
        {
            PlayerDie();
            isDie = true;
        }
    }

    //-----PLAYER HIT------
    /// <summary>
    /// 플레이어의 피격 후 체력 하강, 무적 상태로 전환을 합니다.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="bullet"></param>
    public void Hit(float damage, Bullet bullet)
    {
        Debug.Log("Hit!!");

        // 플레이어는 총알의 데미지와 관계없이 항상 1데미지만 받음
        ChangeHp(-1);
        var effect = Instantiate(explosionEffect,transform.position, Quaternion.identity);
        Destroy(effect,0.5f);
        if (!isDie)
        {
            // 플레이어가 맞았을 때 무적 상태로 전환
            StartCoroutine(Invincible());
        }

    }

    /// <summary>
    /// 플레이어의 총알 색상 일치 판정을 합니다.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="bullet"></param>
    /// <returns></returns>
    public bool CheckHitAble(BulletColor color, Bullet bullet)
    {
        if (bullet.bulletLaunchSource == gameObject)
            return false;

        if (isAnyColor)
            return true;

        return color == playerColor;
    }

    /// <summary>
    /// 플레이어의 무적 상태를 유지하는 코루틴입니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator Invincible()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        
        // 무적 이펙트
        yield return StartCoroutine(spriteRenderer.Invincible());

        GetComponent<CapsuleCollider2D>().enabled = true;
    }


    //------ PLAYER DIE ------
    /// <summary>
    /// 플레이어의 사망 후 처리를 합니다.
    /// </summary>
    private void PlayerDie()
    {
        Debug.Log("OMG Die!!");
        //spriteRd.color = Color.red; //TMP!

        GetComponent<PlayerInput>().enabled = false;

        OnPlayerDie?.Invoke();
        //OnPlayerDie();
        gameObject.SetActive(false);
        
    }
    
    // Item Effects
    /// <summary>
    /// 일정 시간 동안 공격속도를 multiplier 배로 높입니다.
    /// </summary>
    /// <param name="duration">지속시간(초)</param>
    /// <param name="multiplier">속도 배율</param>
    public void ApplyRapidFireBuff(float duration, float multiplier)
    {
        // 기존 코루틴이 돌고 있으면 중복 해제
        if (rapidFireRoutine != null) StopCoroutine(rapidFireRoutine);
        rapidFireRoutine = StartCoroutine(RapidFireBuff(duration, multiplier));
    }

    /// <summary>
    /// 일정 시간 동안 피해량을 multiplier 배로 높입니다.
    /// </summary>
    /// <param name="duration">지속시간(초)</param>
    /// <param name="multiplier">데미지 배율</param>
    public void ApplyDamageBuff(float duration, float multiplier)
    {
        if (damageBuffRoutine != null) StopCoroutine(damageBuffRoutine);
        damageBuffRoutine = StartCoroutine(DamageBuff(duration, multiplier));
    }

    /// <summary>
    /// 일정 시간 동안 색깔 판정을 무시합니다.
    /// </summary>
    /// <param name="duration">지속시간(초)</param>
    public void ApplyAnyColorBuff(float duration)
    {
        if (rapidFireRoutine != null) StopCoroutine(rapidFireRoutine);
        if (damageBuffRoutine != null) StopCoroutine(damageBuffRoutine);
    
        if (isAnyColor)
        {
            StopCoroutine("AnyColorBuff");
        }
    
        StartCoroutine(AnyColorBuff(duration));
    }


    //---- private Coroutine implementations ----

    private IEnumerator RapidFireBuff(float duration, float multiplier)
    {
        float originalCooldown = bulletCooldown;
        bulletCooldown /= multiplier;
        yield return new WaitForSeconds(duration);
        bulletCooldown = originalCooldown;
    }

    private IEnumerator DamageBuff(float duration, float multiplier)
    {
        float originalDamage = bulletDamage;
        bulletDamage *= multiplier;
        yield return new WaitForSeconds(duration);
        bulletDamage = originalDamage;
    }

    private IEnumerator AnyColorBuff(float duration)
    {
        Debug.Log("AnyColorBuff Start");
        isAnyColor = true;
        yield return new WaitForSeconds(duration);
        isAnyColor = false;
        Debug.Log("AnyColorBuff End");
    }




}
