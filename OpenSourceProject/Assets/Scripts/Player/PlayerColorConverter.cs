using BulletSystem;
using System.Collections;
using UnityEngine;

public class PlayerColorConverter : MonoBehaviour
{
    public Sprite Black;
    public Sprite White;
    new SpriteRenderer renderer;
    

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    BulletColor curColor;
    public void SetColor(BulletColor color)
    {
        if(curColor == color) return;
        curColor = color;
        switch (color)
        {
            case BulletColor.White:

                renderer.sprite = White;
                break;
            case BulletColor.Black:
                renderer.sprite = Black;
                break;
            default:
                break;
        }
    }

    public IEnumerator Invincible() {


        renderer.color = new Color(1,0,0,0.9f);
        yield return new WaitForSeconds(1f);
        renderer.color = new Color(1, 1, 1,0.75f);
    }
}
