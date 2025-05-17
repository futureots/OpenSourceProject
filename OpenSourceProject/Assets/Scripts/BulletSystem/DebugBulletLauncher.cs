using BulletSystem;
using UnityEngine;

public class DebugBulletLauncher : MonoBehaviour
{
    public BulletColor color;
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BulletManager.LaunchBullet(color, transform.position, 10f, 5f, Vector2.up, gameObject);
        }    
    }
}
