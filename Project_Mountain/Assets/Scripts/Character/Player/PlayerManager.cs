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

        //UPDATE MAX AMOUNT WHEN STAT IS IS UPGRADED
        OnVitalityChanged += SetNewMaxHealth;
        OnEnduranceChanged += SetNewMaxStamina;

        //UPDATES UI STATS BARS< WHEN CHANGES
        OnHealthChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;

        OnStaminaChanged += PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;
        OnStaminaChanged += playerStatsManager.ResetStaminaRegenTimer;
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public void SetNewMaxHealth(int oldVitality, int newVitality)
    {
        maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(newVitality);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxHealthValue(maxHealth);
        currentHealth = maxHealth;
    }

    public void SetNewMaxStamina(int oldEndurance, int newEndurance)
    {
        maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(newEndurance);
        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina);
        currentStamina = maxStamina;
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        if (activeScene != 0){
            currentCharacterData.sceneIndex = activeScene;
        }

        currentCharacterData.characterName = characterName.ToString();

        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        currentCharacterData.currentHealth = currentHealth;
        currentCharacterData.currentStamina = currentStamina;

        currentCharacterData.vitality = vitality;
        currentCharacterData.endurance = endurance;
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

        vitality =  currentCharacterData.vitality;
        endurance = currentCharacterData.endurance;

        maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(currentCharacterData.vitality);
        maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);

        currentHealth = currentCharacterData.currentHealth;
        currentStamina = currentCharacterData.currentStamina;

        PlayerUIManager.instance.playerUIHUDManager.SetMaxStaminaValue(maxStamina);
    }
}
