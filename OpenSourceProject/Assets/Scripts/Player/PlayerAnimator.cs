using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public GameObject Up;
    public GameObject Down;
    public GameObject Side;
    public enum Direction
    {
        Up, Down,Left,Right
    }
    Direction curDir;
    /// <summary>
    /// 플레이어 스프라이트 전환하는 함수
    /// </summary>
    /// <param name="direction"></param>
    public void SetPlayerSprite(Direction direction)
    {

        if (curDir == direction) return;
        curDir = direction;
        if (curDir == Direction.Up)
        {
            Up.SetActive(true);
            Down.SetActive(false);
            Side.SetActive(false);
        }
        else if (curDir == Direction.Down)
        {
            Down.SetActive(true);
            Side.SetActive(false);
            Up.SetActive(false);
        }
        else
        {
            Down.SetActive(false);
            Up.SetActive(false);
            Side.SetActive(true);
            if (curDir == Direction.Left) Side.transform.localScale = Vector2.one;
            else Side.transform.localScale = new Vector2(-1, 1);
        }
    }
}
