using UnityEngine;

namespace ItemSystem
{
    public class ItemMagnet : MonoBehaviour
    {
        public ItemContainer itemContainer;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // 플레이어와 충돌하면 아이템의 방향을 플레이어 쪽으로 바꿉니다.
                Vector3 playerPosition = collision.transform.position;
                Vector3 direction = (playerPosition - transform.position).normalized;
                itemContainer.direction = direction;
                itemContainer.speed *= 1.5f;
            }
        }
    }
}


