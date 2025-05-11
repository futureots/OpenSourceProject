using BulletSystem;
using UnityEngine;

public class DebugBulletHit : MonoBehaviour, IBulletHitAble
{
    public BulletColor bulletColor;
    public bool isHitSameColor = true;

    public void Hit(float damage, Bullet bullet)
    {
        Debug.Log($"Hit! Damage: {damage} | Bullet: {bullet.name}");
    }

    public bool CheckHitAble(BulletColor color)
    {
        if (isHitSameColor)
        {
            return bulletColor == color;
        }
        else
        {
            return bulletColor != color;
        }
    }
}
