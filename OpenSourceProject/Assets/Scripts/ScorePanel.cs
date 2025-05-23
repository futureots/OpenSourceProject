using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public Text score;
    public Text timePanel;
    public int stageNum;

    private void Start()
    {
        SetPanel();
    }
    public void SetPanel()
    {
        var data = PlayerData.LoadPlayerData("Data");
        score.text = data.data[stageNum].point.ToString();
        int time = (int)data.data[stageNum].time;
        timePanel.text = $"{time / 60} : {time % 60}";
    }
}
