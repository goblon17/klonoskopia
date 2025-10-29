using StorkStudios.CoreNest;
using UnityEngine;

public class MainMenuController : Singleton<MainMenuController>
{
    public void PlayButton()
    {
        GameManager.Instance.ChangeScene(GameManager.Scene.Cutscene);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
