using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{

    public TextMeshProUGUI score;
    public TextMeshProUGUI timePanel;
    public void SetPanel()
    {
        var data = PlayerData.LoadPlayerData("Data");
        score.text = "Score : " + GameManager.Instance.point;
        int time = (int)GameManager.Instance.time;
        timePanel.text = $"Time  {time / 60} : {time % 60}";
    }
}
