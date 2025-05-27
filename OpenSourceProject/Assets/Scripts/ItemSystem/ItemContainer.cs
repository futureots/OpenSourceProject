using System.Collections;
using UnityEngine;

namespace ItemSystem
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class ItemContainer : MonoBehaviour
    {
        /// <summary>
        /// 아이템의 정보를 담고 있는 ItemInfo입니다.
        /// </summary>
        public ItemInfo itemInfo;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 아이템의 이동방향입니다.
        /// </summary>
        public Vector3 direction;
        /// <summary>
        /// 아이템의 속도입니다.
        /// </summary>
        public float speed;

        /// <summary>
        /// 아이템이 벽에 튕길 수 있는 횟수입니다.
        /// </summary>
        public int hitCount;

        private Camera mainCamera;

        /// <summary>
        /// 아이템의 정보를 설정합니다.
        /// </summary>
        /// <param name="info">아이템의 정보입니다.</param>
        /// <param name="dir">아이템의 발사 방향입니다.</param>
        /// <param name="spd">아이템의 속도입니다.</param>
        public void Initialize(ItemInfo info, Vector3 dir, float spd, int wallHitCount)
        {
            mainCamera = Camera.main;

            direction = dir;
            speed = spd;
            hitCount = wallHitCount;

            itemInfo = info;
            spriteRenderer.sprite = info.ItemSprite;

        }

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * direction);

            // 화면 밖으로 나가면 튕겨나갑니다.
            
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);

            if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            {
                hitCount--;
                Debug.Log($"Item {itemInfo.ItemName} hit wall. Remaining hits: {hitCount}");
                if (hitCount == 1)
                {
                    StartCoroutine(NoticeLastHit());
                }

                if (hitCount <= 0)
                {
                    Debug.Log($"Item {itemInfo.ItemName} destroyed after hitting walls.");
                    Destroy(gameObject);
                }
                else
                {
                    Vector3 normal = Vector3.down;
                    if (screenPos.x < 0)
                    {
                        normal = Vector3.right;
                    }
                    else if (screenPos.x > Screen.width)
                    {
                        normal = Vector3.left;
                    }
                    else if (screenPos.y < 0)
                    {
                        normal = Vector3.up;
                    }
                    direction = Vector3.Reflect(direction, normal);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ItemManager.ApplyItemEffectToPlayer(itemInfo);
                Destroy(gameObject);
            }
        }

        IEnumerator NoticeLastHit()
        {
            var wait = new WaitForSeconds(0.05f);
            while (hitCount > 0)
            {
                spriteRenderer.color = Color.clear;
                yield return wait;
                spriteRenderer.color = Color.white;
                yield return wait;
                yield return wait;
            }
        }
    }
}


