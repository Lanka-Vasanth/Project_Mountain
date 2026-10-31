using System.Collections;
using Unity.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : CharacterManager
{
    [Header("DEBUG MENU")]
    [SerializeField] bool respawnCharacter = false;
    [SerializeField] bool switchWeapon = false; 

    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerStatsManager playerStatsManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerEquipmentManager playerEquipmentManager;
    public PlayerCombatManager playerCombatManager;

    public FixedString64Bytes characterName;

    public event Action<int, int> OnCurrentWeaponChanged;
    public int _currentWeaponInUse;
    public int currentWeaponInUse
    {
        get =>_currentWeaponInUse;
        set
        {
            if (_currentWeaponInUse == value)
                return;

            int oldValue = _currentWeaponInUse;
            _currentWeaponInUse = value;
            OnCurrentWeaponChanged?.Invoke(oldValue, _currentWeaponInUse);
        }
    }


    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        
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

        OnHandWeaponIDChanged += OnCurrentHandWeaponChange;
        OnCurrentWeaponChanged += OnCurrentWeaponInUseIDChange;

        OnHealthChanged += CheckHP;

        OnIsLockOnChanged += OnIsLockedOnChange;

        OnIsChargingAttackChanged += OnIsChargingAttackChange;
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
        playerStatsManager.RegenerateStamina();
        DebugMenu();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();

        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        isDead = false;
        //...AND MORE
        playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
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

    public void OnCurrentHandWeaponChange(int oldID, int newID)
    {
        WeaponItem newWeapon =  Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerInventoryManager.currentInHandWeapon = newWeapon;
        playerEquipmentManager.LoadWeapon();
    }

    public void OnCurrentWeaponInUseIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        playerCombatManager.currentWeaponInUse = newWeapon;
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        OnVitalityChanged -= SetNewMaxHealth;
        OnEnduranceChanged -= SetNewMaxStamina;

        OnHealthChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewHealthValue;
        OnStaminaChanged -= PlayerUIManager.instance.playerUIHUDManager.SetNewStaminaValue;

        OnStaminaChanged -= playerStatsManager.ResetStaminaRegenTimer;

        OnHandWeaponIDChanged -= OnCurrentHandWeaponChange;
        OnCurrentWeaponChanged -= OnCurrentWeaponInUseIDChange;
        OnHealthChanged -= CheckHP;
        OnIsLockOnChanged -= OnIsLockedOnChange;
        OnIsChargingAttackChanged -= OnIsChargingAttackChange;
    }

    private void DebugMenu()
    {
        if (respawnCharacter)
        {
            respawnCharacter = false;
            ReviveCharacter();
        }

        if (switchWeapon)
        {
            switchWeapon = false;
            playerEquipmentManager.SwitchWeapon();
        }
    }
}
