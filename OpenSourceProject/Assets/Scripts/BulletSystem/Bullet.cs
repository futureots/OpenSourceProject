using System;
using UnityEngine;

namespace BulletSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        #region Variables
        [Header("Bullet Information")]
        /// <summary>
        /// 총알의 속도
        /// </summary>
        public float bulletSpeed;

        /// <summary>
        /// 총알과 충돌 시 입는 피해량
        /// </summary>
        public float bulletDamage;

        /// <summary>
        /// 총알이 날아가는 방향
        /// </summary>
        public Vector2 launchDirection;

        /// <summary>
        /// 총알의 이동 여부
        /// </summary>
        public bool isMoving = false;

        /// <summary>
        /// 총알의 색상입니다.
        /// </summary>
        public BulletColor bulletColor;

        public Sprite whiteColorSprite;
        public Sprite blackColorSprite;

        /// <summary>
        /// 총알을 발사한 오브젝트입니다.
        /// </summary>
        public GameObject bulletLaunchSource;

        // 내부 컴포넌트 변수
        private SpriteRenderer spriteRenderer;
        private Collider2D myCollider2D;
        private Camera mainCam;
        
        /// <summary>
        /// 플레이어 버프에 의해 이 총알이 색판정을 무시해야 하는지 여부입니다.
        /// </summary>
        public bool ignoreColor { get; private set; }
    
        #endregion

        #region Methods
        /// <summary>
        /// 총알의 변수를 초기화합니다.
        /// </summary>
        /// <param name="onReleaseBullet">총알을 풀에 다시 넣어주는 함수입니다</param>
        public Bullet Init()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            myCollider2D = GetComponent<Collider2D>();
            // animator = GetComponent<Animator>();
            myCollider2D.isTrigger = true;

            mainCam = Camera.main;

            // animatorTriggerHash = Animator.StringToHash(animatorTriggerName);
            return this;
        }

        /// <summary>
        /// 자동으로 오브젝트가 활성화되어 총알이 발사됩니다.
        /// </summary>
        /// <param name="position">총알이 발사될 위치</param>
        /// <param name="speed">총알의 속도</param>
        /// <param name="damage">총알과 충돌 시 받을 데미지</param>
        /// <param name="direction">총알이 발사될 방향 (자동으로 회전)</param>
        public void Launch(BulletColor color, Vector3 position, float speed, float damage, Vector2 direction, GameObject source)
        {
            gameObject.SetActive(true);

            transform.position = position;
            bulletSpeed = speed;
            bulletDamage = damage;
            launchDirection = direction.normalized;
            bulletColor = color;
            bulletLaunchSource = source;
            spriteRenderer.sprite = color == BulletColor.White ? whiteColorSprite : blackColorSprite;

            // 회전하기
            // float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));

            isMoving = true;
            myCollider2D.enabled = true;
            
            bulletLaunchSource = source;

            // (1) 발사 시점에 플레이어의 색무시 버프를 물려준다
            ignoreColor = false;
            if (source.CompareTag("Player") 
                && source.TryGetComponent<Player>(out var player)
                && player.IsAnyColorPublic)
            {
                ignoreColor = true;
            }

            isMoving = true;
            myCollider2D.enabled = true;
        }

        // Update 함수에서 총알이 발사된 방향으로 이동합니다.
        private void Update()
        {
            if (isMoving)
            {
                Move();
            }
        }

        /// <summary>
        /// 이동하는 함수 - 다르게 이동하는 총알이 필요할때를 위해
        /// </summary>
        protected virtual void Move()
        {
            transform.Translate(bulletSpeed * Time.deltaTime * launchDirection, Space.World);

            // 화면 밖이면 비활성화
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position);
            if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            {
                isMoving = false;
                myCollider2D.enabled = false;
                BulletManager.Instance.ReleaseBullet(this);
                gameObject.SetActive(false);
            }
        }

        // 총알이 특정한 사물과 충돌하면 사라지며 만약 충돌한 객체가 IBulletHitAble 인터페이스를 구현했다면 Hit 함수를 호출합니다.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("BULLET"))
            {
                return;
            }
             
            if (other.gameObject.TryGetComponent<IBulletHitAble>(out var hitAble))
            {
                bool canHit;

                // (2) “플레이어가 쏜 총알” + “버프가 묻은 총알” 이면 무조건 적중
                if (ignoreColor && bulletLaunchSource.CompareTag("Player"))
                {
                    canHit = true;
                }
                else
                {
                    // 원래 색 비교 로직
                    canHit = hitAble.CheckHitAble(bulletColor, this);
                }

                if (!canHit) 
                    return;

                hitAble.Hit(bulletDamage, this);
            }

            isMoving = false;
            myCollider2D.enabled = false;
            BulletManager.Instance.ReleaseBullet(this);
            gameObject.SetActive(false);
            // animator.SetTrigger(animatorTriggerHash);
        }
        #endregion
    }

}

