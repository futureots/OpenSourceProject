using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{

    public TextMeshProUGUI score;
    public TextMeshProUGUI timePanel;
    public void SetPanel()
    {
        var data = PlayerData.LoadPlayerData("Data");
        score.text = "Score : "+ data.data[GameManager.Instance.stageNumber].point;
        int time = (int)data.data[GameManager.Instance.stageNumber].time;
        timePanel.text = $"Time  {time / 60} : {time % 60}";
    }
}
