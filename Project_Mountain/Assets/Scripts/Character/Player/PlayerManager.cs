using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerStatsManager playerStatsManager;

    public FixedString64Bytes characterName;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

        WorldSaveGameManager.instance.player = this;
    }
    protected override void Start()
    {
        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;

        OnStaminaChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        OnStaminaChanged += playerStatsManager.ResetStaminaRegenTimer;

        maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(endurance);
        currentStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(endurance);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina);
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegerateStamina();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;

        currentCharacterData.characterName = characterName.ToString();

        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        characterName = currentCharacterData.characterName;
        
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition,
                                         currentCharacterData.yPosition,
                                         currentCharacterData.zPosition);

        characterController.enabled = false;
        transform.position = myPosition;
        characterController.enabled = true;
    }
}
