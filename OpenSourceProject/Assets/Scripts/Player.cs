using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
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
    [SerializeField]
    private int currentHP;
    private int maxHP = 10;

    //is Damaged




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
    }


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
        }
    }

    // 총알 피격
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BULLET"))
        {
            Debug.Log("총알 충돌했다!");
            currentHP -= 1; //총알 데미지 관련 수정 가능성 O

            StartCoroutine(Invincible());
        }
    }
    
    // 무적 시간
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

}
