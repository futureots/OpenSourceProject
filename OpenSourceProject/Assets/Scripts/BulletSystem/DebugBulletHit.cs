using BulletSystem;
using UnityEngine;

public class DebugBulletHit : MonoBehaviour, IBulletHitAble
{
    public void Hit(float damage, BulletSystem.Bullet bullet)
    {
        Debug.Log($"Hit! Damage: {damage} | Bullet: {bullet.name}");
    }
}
