using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    [Header("Menu Objects")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;

    [Header("Buttons")]
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button mainMenuNewGameButton;


    [Header("Pop Ups")]
    [SerializeField] GameObject noFreeSlotsPopUp;
    [SerializeField] Button noFreeSlotsOKButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttemptToCreateNewGame();
    }

    //ALSO CAN ADD AUDIO WHEN CLICKED TYPE SHI, WITHIN THIS
    public void OpenLoadGameMenu()
    {
        //CLOSE MAIN MENU, OPEN LOAD MENU
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        //SELECT RETURN BUTTON
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);

        //SELECT LOAD BUTTON
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeSlotsPopUp()
    {
        noFreeSlotsPopUp.SetActive(true);
        noFreeSlotsOKButton.Select();
    }

    public void CloseNoFreeSlotsPopUp()
    {
        noFreeSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }

}
