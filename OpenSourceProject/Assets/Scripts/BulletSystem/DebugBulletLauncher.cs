using BulletSystem;
using UnityEngine;

public class DebugBulletLauncher : MonoBehaviour
{
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BulletManager.Instance.LaunchBullet(transform.position, 10f, 5f, Vector2.up);
        }    
    }
}
