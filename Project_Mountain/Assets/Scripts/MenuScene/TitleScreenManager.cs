using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public void StartNewGame()
    {
        WorldSaveGameManager.instance.CreateNewGame();
        StartCoroutine(WorldSaveGameManager.instance.LoadWorldScene());
    }
}
