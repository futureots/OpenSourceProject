using UnityEngine;

public class DebugGameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.GameEnd();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            GameManager.Instance.AddPoint(50);
        }
    }
}
