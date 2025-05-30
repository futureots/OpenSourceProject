using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI pointPanel;
    public TextMeshProUGUI timePanel;
    public GameObject hpPanel;
    public GameObject EndPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EndPanel.SetActive(false);
        GameManager.Instance.OnStageEnd.AddListener(() =>
        {
            EndPanel.SetActive(true);
            EndPanel.GetComponent<ResultPanel>().SetPanel();
        });
    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)GameManager.Instance.time;
        timePanel.text = $"{time/60} : {time%60}";

        pointPanel.text = "Point : " + GameManager.Instance.point.ToString();

        ShowHpPanel();
        
    }

    // 현재 체력 패널 업데이트
    void ShowHpPanel()
    {
        Player player = GameManager.Instance.player;
        var maxHp = player.maxHP > hpPanel.transform.childCount ? hpPanel.transform.childCount : player.maxHP;
        var i = player.currentHP;
        i = Mathf.Clamp(i, 0, maxHp);

        for(int j = 0; j < hpPanel.transform.childCount; j++)
        {
            if(j< i)
            {
                hpPanel.transform.GetChild(j).gameObject.SetActive(true);
            }
            else
            {
                hpPanel.transform.GetChild(j).gameObject.SetActive(false);
            }
            
        }
    }
}
