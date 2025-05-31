using UnityEngine;
using BulletSystem;

namespace ItemSystem
{
    public class ItemManager : Singleton<ItemManager>
    {
        public ItemContainer itemPrefab;
        public float speed = 5f;
        public int wallHitCount = 5;

        /// <summary>
        /// 아이템을 들어온 위치에 소환해서 랜덤한 방향으로 발사합니다.
        /// <para>아이템의 공용 수치(속도, 벽에 몇번까지 튕길 수 있는 지)는 매니저에서 관리합니다.</para>
        /// </summary>
        /// <param name="itemInfo">설정할 아이템</param>
        /// <param name="position">아이템의 위치</param>
        public static void SummonItem(ItemInfo itemInfo, Vector3 position)
        {
            float angle = Random.Range(0f, 360f);
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;

            ItemContainer item = Instantiate(Instance.itemPrefab, position, Quaternion.identity);
            item.Initialize(itemInfo, direction, Instance.speed, Instance.wallHitCount);
        }
        /// <summary>
        /// 아이템을 들어온 위치에 소환해서 랜덤한 방향으로 발사합니다.
        /// <para>아이템의 공용 수치(속도, 벽에 몇번까지 튕길 수 있는 지)는 매니저에서 관리합니다.</para>
        /// <para>아이템은 Resources/Item/{ItemName}을 불러옵니다.</para>
        /// </summary>
        /// <param name="itemName">Resources/Item/{ItemName}에 있는 아이템을 로드합니다.</param>
        /// <param name="position">아이템의 위치</param>
        public static void SummonItem(string itemName, Vector3 position)
        {
            ItemInfo itemInfo = Resources.Load<ItemInfo>($"Item/{itemName}");
            if (itemInfo == null)
            {
                Debug.LogError($"Item {itemName} not found.");
                return;
            }
            SummonItem(itemInfo, position);
        }

        /// <summary>
        /// 아이템의 효과를 플레이어에게 적용합니다.
        /// <para>아이템의 효과는 아이템의 ID를 통해 결정됩니다.</para>
        /// <para>함수는 아이템이 플레이어와 충돌 시 호출됩니다.</para>
        /// </summary>
        /// <param name="itemInfo"></param>
        public static void ApplyItemEffectToPlayer(ItemInfo itemInfo)
        {
            Debug.Log($"Applying item effect: {itemInfo.ItemName}, ID: {itemInfo.ItemID}");

            var player = GameManager.Instance.player;

            switch (itemInfo.ItemID)
            {
                case 0: player.ChangeHp(1); break;
                case 1: BulletManager.ClearAllBullets(); break;
                case 2: player.ApplyRapidFireBuff(5f, 2f); break;
                case 3: player.ApplyDamageBuff(5f, 2f); break;
                case 4: player.ApplyAnyColorBuff(5f); break;
                case 5:
                    int extra = Random.Range(1, Mathf.CeilToInt(GameManager.Instance.time));
                    GameManager.Instance.AddPoint(extra);
                    break;
                default: Debug.LogWarning($"Unknown ItemID: {itemInfo.ItemID}"); break;
            }


        }

        [ContextMenu("Spawn Item HP")]
        public void DebugItemSpawnHp()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_HP", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }
        
        [ContextMenu("Spawn Item Clear")]
        public void DebugItemSpawnClear()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_Clear", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }
        
        [ContextMenu("Spawn Item Rapid")]
        public void DebugItemSpawnRapid()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_Rapid", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }
        
        [ContextMenu("Spawn Item Damage")]
        public void DebugItemSpawnDamage()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_Damage", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }
        
        [ContextMenu("Spawn Item AnyColor")]
        public void DebugItemSpawnAnyColor()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_AnyColor", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }
        
        [ContextMenu("Spawn Item Point")]
        public void DebugItemSpawnPoint()
        {            
            Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
            SummonItem("Item_Point", spawnPosition);
            Debug.Log($"Spawned item at {spawnPosition}");
        }

    }
}