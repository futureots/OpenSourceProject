using TMPro;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointPanel;
    public TextMeshProUGUI timePanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)GameManager.Instance.time;
        timePanel.text = $"{time/60} : {time%60}";

        pointPanel.text = "Point : " + GameManager.Instance.point.ToString();
    }
}
