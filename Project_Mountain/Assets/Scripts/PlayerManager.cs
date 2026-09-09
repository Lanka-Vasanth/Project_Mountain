using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public PlayerLocomotionManager playerLocomotionManager;
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        animator = GetComponent<Animator>();

        PlayerCamera.instance.player = this;
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();

    }
}
