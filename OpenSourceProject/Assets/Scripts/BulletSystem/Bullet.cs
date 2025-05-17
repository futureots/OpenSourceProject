using System;
using UnityEngine;

namespace BulletSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour
    {
        #region Variables
        [Header("Bullet Infomation")]
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

        /// <summary>
        /// 총알을 발사한 오브젝트입니다.
        /// </summary>
        public GameObject bulletLaunchSource;


        [Header("Bullet Properties")]

        /// <summary>
        /// Animator의 Hit 트리거 이름입니다.
        /// 애니메이터에 별도의 트리거를 사용한다면 이 값을 바꿔야합니다.
        /// </summary>
        public string animatorTriggerName = "Hit";
        private int animatorTriggerHash;

        /// <summary>
        /// 총알의 자동 회전과 스프라이트와의 회전을 보정하기 위한 값입니다.
        /// </summary>
        public float rotateCorrection = 180f;

        // 내부 컴포넌트 변수
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Collider2D myCollider2D;
        private Camera mainCam;
    
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
            animator = GetComponent<Animator>();
            myCollider2D.isTrigger = true;

            mainCam = Camera.main;

            animatorTriggerHash = Animator.StringToHash(animatorTriggerName);
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

            // 회전하기
            float angle = Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));

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
            }
        }

        // 총알이 특정한 사물과 충돌하면 사라지며 만약 충돌한 객체가 IBulletHitAble 인터페이스를 구현했다면 Hit 함수를 호출합니다.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<IBulletHitAble>(out var hitAble))
            {
                if (!hitAble.CheckHitAble(bulletColor, this))
                {
                    return;
                }
                hitAble.Hit(bulletDamage, this);
            }

            isMoving = false;
            myCollider2D.enabled = false;
            animator.SetTrigger(animatorTriggerHash);
        }
        #endregion
    }

}

