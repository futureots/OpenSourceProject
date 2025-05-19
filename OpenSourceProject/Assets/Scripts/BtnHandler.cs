using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnHandler : MonoBehaviour
{
    /// <summary>
    /// 버튼의 종류입니다.
    /// </summary>
    public enum BtnType
    {
        GoLevelScene,
        GoMainScene,
        GoBattleScene,
        Exit
    }

    /// <summary>
    /// 버튼의 종류 중 하나를 unity inspector의 스플릿 버튼에서 선택하면 됩니다.
    /// </summary>
    public BtnType currentType;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BtnType.GoLevelScene:
                Debug.Log("LevelTestScene 전환");
                SceneManager.LoadScene("LevelTestScene");
                break;

            case BtnType.GoMainScene:
                Debug.Log("MainTestScene 전환");
                SceneManager.LoadScene("MainTestScene");
                break;

            case BtnType.GoBattleScene:
                Debug.Log("AssetTestScene 전환");
                SceneManager.LoadScene("AssetTestScene");
                break;

            case BtnType.Exit:
                Debug.Log("게임종료");
                Application.Quit();
                break;

        }

    }
}
