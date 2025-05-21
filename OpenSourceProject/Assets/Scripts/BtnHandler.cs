using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnHandler : MonoBehaviour
{
    /// <summary>
    /// ��ư�� �����Դϴ�.
    /// </summary>
    public enum BtnType
    {
        GoLevelScene,
        GoMainScene,
        GoBattleScene,
        Exit
    }

    /// <summary>
    /// ��ư�� ���� �� �ϳ��� unity inspector�� ���ø� ��ư���� �����ϸ� �˴ϴ�.
    /// </summary>
    public BtnType currentType;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BtnType.GoLevelScene:
                Debug.Log("LevelTestScene ��ȯ");
                SceneManager.LoadScene("LevelTestScene");
                break;

            case BtnType.GoMainScene:
                Debug.Log("MainTestScene ��ȯ");
                SceneManager.LoadScene("MainTestScene");
                break;

            case BtnType.GoBattleScene:
                Debug.Log("AssetTestScene ��ȯ");
                SceneManager.LoadScene("AssetTestScene");
                break;

            case BtnType.Exit:
                Debug.Log("��������");
                Application.Quit();
                break;

        }

    }
}
