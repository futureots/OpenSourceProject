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
        
        /// <summary>
        /// 해당 오브젝트가 이 색을 가진 총알과 충돌 시 피해를 입는지 확인하는 함수입니다.
        /// <para>만약 false를 반환할 경우 총알은 사라지지 않고 해당 오브젝트를 지나갑니다.</para>
        /// </summary>
        /// <param name="color">충돌한 총알의 색입니다.</param>
        /// <returns>true - 총알과 충돌하고 Hit 함수가 호출됩니다. false - 총알이 무시하고 지나갑니다.</returns>
        public bool IsHitAble(BulletColor color);
    }
}