using UnityEngine;

namespace BulletSystem
{
    /// <summary>
    /// 총알과 충돌 시 피해를 입는 오브젝트에 붙이는 인터페이스입니다. Collision2D가 존재해야합니다.
    /// <para>이 인터페이스를 구현한 오브젝트는 총알과 충돌 시 Hit 함수를 호출하여 피해를 입습니다.</para>
    /// </summary>
    public interface IBulletHitAble
    {
        /// <summary>
        /// 총알과 충돌 시 피해를 입는 함수입니다.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="bullet"></param>
        public void Hit(float damage, Bullet bullet);
    }
}