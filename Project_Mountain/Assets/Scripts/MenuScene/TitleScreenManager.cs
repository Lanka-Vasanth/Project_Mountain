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
    [SerializeField] Button deleteCharacterPopUpConfirmButton;


    [Header("Pop Ups")]
    [SerializeField] GameObject noFreeSlotsPopUp;
    [SerializeField] Button noFreeSlotsOKButton;
    [SerializeField] GameObject delectCharacterSlotPopUp;


    [Header("Character Save Slots")]
    public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

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

    //CHARACTER SLOTS

    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedSlot = characterSlot;
    }

    public void SelectNoSlot()
    {
        currentSelectedSlot = CharacterSlot.NO_SLOT;
    }

    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelectedSlot != CharacterSlot.NO_SLOT){
            delectCharacterSlotPopUp.SetActive(true);
            deleteCharacterPopUpConfirmButton.Select();
        }
    }

    public void DeleteCharacterSlot()
    {
        delectCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);

        //DISABLE>ENABLE TO REFRESH AFTER DELETION OF SLOT
        titleScreenLoadMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();
    }

    public void CloseDeleteCharacterPopUp()
    {
        delectCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();
    }

}
