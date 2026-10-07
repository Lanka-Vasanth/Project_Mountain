using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerStatsManager playerStatsManager;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatsManager = GetComponent<PlayerStatsManager>();

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
}
