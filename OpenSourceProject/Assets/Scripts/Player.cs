using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using BulletSystem;

public class Player : MonoBehaviour, IBulletHitAble
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRd;

    //Moving
    private Vector2 moveDirection;
    [SerializeField]
    private float moveSpeed;

    //Looking
    private Vector2 mouseScreenPos;
    private Camera mainCam;

    //HP
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
    public float bulletCooldown = 0.5f;
    private float bulletCooldownTimer = 0f;


    //Die
    public delegate void PlayerDieHandler(); //type
    public static event PlayerDieHandler OnPlayerDie; //event
    private bool isDie = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        //Looking
        mainCam = Camera.main;

        //Change Color
        spriteRd = GetComponent<SpriteRenderer>();

        //HP
        currentHP = maxHP;
    }


    void Update()
    {
        //Moving
        rb.linearVelocity = moveDirection * moveSpeed;

        //Looking
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        Vector2 dirVec = mouseWorldPos - (Vector2)transform.position;

        if (dirVec != Vector2.zero)
        {
            transform.up = dirVec.normalized;
            //float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
            //rb.rotation = angle; // z축 기준 회전
        }

        if (bulletCooldownTimer > 0)
        {
            bulletCooldownTimer -= Time.deltaTime;
        }
        else
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

    //------ PLAYER CONTROL ------
    /// <summary>
    /// player : wsad Moving
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// player : Mouse dir Looking
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// player : Left click => change color to white
    /// </summary>
    /// <param name="context"></param>
    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("좌클릭!");
            spriteRd.color = Color.white;
            playerColor = BulletColor.White;
        }
    }
    
    /// <summary>
    /// player : Right click => change color to black
    /// </summary>
    /// <param name="context"></param>
    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("좌클릭!");
            spriteRd.color = Color.black;
            playerColor = BulletColor.Black;
        }
    }

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

    // ----- PLAYER HIT ------
    public void Hit(float damage, Bullet bullet)
    {
        Debug.Log("Hit!!");

        // 플레이어는 총알의 데미지와 관계없이 항상 1데미지만 받음
        ChangeHp(-1);

        if (!isDie)
        {
            // 플레이어가 맞았을 때 무적 상태로 전환
            StartCoroutine(Invincible());
        }

    }

    public bool CheckHitAble(BulletColor color, Bullet bullet)
    {
        if (bullet.bulletLaunchSource == gameObject)
        {
            // 발사한 총알은 무시
            return false;
        }

        if (color == playerColor)
        {
            // 같은 색깔의 총알은 맞음
            return true;
        }
        else
        {
            // 다른 색깔의 총알은 무시
            return false;
        }
    }

    IEnumerator Invincible()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        spriteRd.color = new Color(spriteRd.color.r, spriteRd.color.g, spriteRd.color.b,
                                   0.5f);

        yield return new WaitForSeconds(1f);

        GetComponent<CapsuleCollider2D>().enabled = true;
        spriteRd.color = new Color(spriteRd.color.r, spriteRd.color.g, spriteRd.color.b,
                                   1f);
    }


    //------ PLAYER DIE ------
    private void PlayerDie()
    {
        Debug.Log("OMG Die!!");
        spriteRd.color = Color.red; //TMP!

        GetComponent<PlayerInput>().enabled = false;

        OnPlayerDie?.Invoke();
        //OnPlayerDie();
    }

}
