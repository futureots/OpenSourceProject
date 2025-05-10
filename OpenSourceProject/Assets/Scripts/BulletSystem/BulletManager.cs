using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BulletSystem
{
    public enum BulletColor
    {
        White, Black
    }

    public class BulletManager : MonoBehaviour
    {
        #region Singleton
        // 간단한 싱글톤 패턴입니다.
        public static BulletManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        /// <summary>
        /// 총알의 색상입니다.
        /// </summary>


        #region Variables
        /// <summary>
        /// 총알 프리팹
        /// </summary>
        public Bullet bulletPrefab;

        /// <summary>
        /// 미리 생성할 총알의 개수입니다.
        /// </summary>
        public int preLoadedBulletCount = 10;

        /// <summary>
        /// 총알의 최대 개수입니다.
        /// <para>최대 개수를 초과하면 그 후 풀에 돌아오는 총알은 삭제됩니다.</para>
        /// </summary>
        public int maximumBulletCount = 100;

        /// <summary>
        /// 총알이 저장될 풀입니다.
        /// </summary>
        private ObjectPool<Bullet> bulletPool;

        #endregion

        #region Methods
        // 초기화가 필요한 변수를 할당합니다.
        private void Start()
        {
            bulletPool = new ObjectPool<Bullet>(
                createFunc: CreateBullet,
                actionOnDestroy: OnDestroyBullet,
                collectionCheck: false,
                defaultCapacity: preLoadedBulletCount,
                maxSize: maximumBulletCount
                );
        }

        #region Pool Methods

        // 풀에서 총알을 생성하는 함수입니다.
        private Bullet CreateBullet()
        {
            return Instantiate(bulletPrefab, transform).Init();
        }
        // 풀에서 총알을 제거하는 함수입니다.
        private void OnDestroyBullet(Bullet bullet)
        {
            Destroy(bullet.gameObject);
        }

        /// <summary>
        /// 총알을 풀에 반환하는 함수입니다.
        /// </summary>
        /// <param name="bullet"></param>
        public void ReleaseBullet(Bullet bullet)
        {
            bulletPool.Release(bullet);
        }
        #endregion

        /// <summary>
        /// 총알을 발사하는 함수입니다
        /// <para>총알은 오브젝트 풀로 관리되며 최대치를 넘을 경우 임시 객체를 생성해 반환합니다</para>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="direction"></param>
        public static void LaunchBullet(BulletColor color, Vector3 position, float speed, float damage, Vector2 direction)
        {
            var bulletManager = Instance;

            Bullet bullet;
            if (bulletManager.bulletPool.CountActive >= bulletManager.maximumBulletCount)
            {
                bullet = bulletManager.CreateBullet();
            }
            else
            {
                bullet = bulletManager.bulletPool.Get();
            }

            // 총알을 발사합니다.
            bullet.Launch(color, position, speed, damage, direction);
        }

        #endregion
    }
}