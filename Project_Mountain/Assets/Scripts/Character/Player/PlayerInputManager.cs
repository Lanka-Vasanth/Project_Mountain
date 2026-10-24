using System.Collections;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerManager player;

    private PlayerControls playerControls;

    [SerializeField] public float mouseX;
    [SerializeField] public float mouseY;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] bool enableWASDMovement = true;
    [SerializeField] Vector2 movementInput;
    [SerializeField] public float verticalInput;
    [SerializeField] public float horizontalInput;
    [SerializeField] public float moveAmount;
 
    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] public Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;

    [Header("LOCK ON INPUT")]
    [SerializeField] bool lockOnInput = false;
    [SerializeField] bool lockOnLeftInput = false;
    [SerializeField] bool lockOnRightInput = false;
    private Coroutine lockOnCoroutine;


    [Header("PLAYER ACTION INPUT")]
    [SerializeField] bool dashInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool grappleInput = false;
    [SerializeField] bool r1Input = false;
 
    private void Awake()
    {   
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
        }

        playerControls.Enable();
        
        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        playerControls.PlayerActions.Dash.performed += i => dashInput = true;
        playerControls.PlayerActions.Grapple.performed += i => grappleInput = true;
        playerControls.PlayerActions.R1.performed += i => r1Input = true;
        playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
        playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOnLeftInput = true;
        playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOnRightInput = true;


        //HOLDING THE INPUT SETS TO TRUE, LEAVING REVERTS TO FALSE
        playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
        playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

    }

    void OnDisable()
    {
        playerControls.Disable(); 
    }

    private void Update()
    {
        HandleAllInputs();
    }

    // WILL BE CALLED EACH FRAME IN UPDATE, RECORDS ALL INPUT ACTIONS
    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
        HandleDashInput();
        HandleSprintInput();
        HandleGrappleInput();
        HandleR1Input();
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
    }

    //MOVEMENT

    private void HandlePlayerMovementInput()
    {
        if (!enableWASDMovement)
        {
            return;
        }

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
    
        if(moveAmount <= 0.5f && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }else if(moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        if(player == null)
        {
            return;
        }

        //WHEN NOT LOCKED ON
        if(!player.isLockedOn || player.isSprinting){
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
        }
        else
        {
            Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
            camForwardDirection.y=0;
            camForwardDirection.Normalize();

            Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
            camRightDirection.y=0;
            camRightDirection.Normalize();

            Vector3 worldMove = Vector3.zero;
            worldMove = camForwardDirection*verticalInput + camRightDirection*horizontalInput;
            worldMove.Normalize();

            Vector3 localMove = player.transform.InverseTransformDirection(worldMove) * moveAmount;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(localMove.x, localMove.z, player.isSprinting);
        }
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }

    //ACTION

    private void HandleDashInput()
    {
        if (dashInput)
        {
            dashInput = false;

            //NOTE TO DEV: RETURN IF UI IS ACTIVE ON SCREEN
            //PERFORM DASH
            player.playerLocomotionManager.AttemptToPerformDash();
        }
    }

    private void HandleSprintInput()
    {
        if(player == null)
        {
            return;
        }

        if (sprintInput)
        {
            player.playerLocomotionManager.HandleSprinting();
        }
        else
        {
            player.isSprinting = false;
        }
    }

    private void HandleGrappleInput()
    {
        if (grappleInput)
        {
            grappleInput = false;

            player.playerLocomotionManager.AttemptToPerformGrapple();
        }
    }

    private void HandleR1Input()
    {
        if (r1Input)
        {
            r1Input = false;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentInHandWeapon.r1_Action, player.playerInventoryManager.currentInHandWeapon);
        }
    }

    private void HandleLockOnInput()
    {
        if (player.isLockedOn)
        {
            if(player.playerCombatManager.currentTarget == null)
            {
                return;
            }
            //CHECK FOR DEAD TARGETS
            if (player.playerCombatManager.currentTarget.isDead)
            {
                player.isLockedOn = false;

                //ATTEMPT TO FIND NEW TARGET OR UNLOCK
                if(lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }

            
        }

        if (lockOnInput && player.isLockedOn)
        {
            lockOnInput = false;

            // IS PLAYER ALREADY LOCKED ON, IF YES THEN UNLOCK, ELSE ATTEMPT TO LOCKON
            PlayerCamera.instance.ClearLockOnTargets();
            player.isLockedOn = false;
 
            return;
        }

        if (lockOnInput && !player.isLockedOn)
        {
            lockOnInput = false;
            
            // IF USING RANGED WEAPONS DISABLE LOCKING ON
            PlayerCamera.instance.HandleLocatingLockOnTargets();

            if(PlayerCamera.instance.nearestLockOnTarget != null)
            {
                //ASSIGN AS TARGET
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.isLockedOn = true;
            }
        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (lockOnLeftInput)
        {
            lockOnLeftInput = false;
            if (player.isLockedOn)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();
                if(PlayerCamera.instance.leftLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }

        if (lockOnRightInput)
        {
            lockOnRightInput = false;
            if (player.isLockedOn)
            {
                PlayerCamera.instance.HandleLocatingLockOnTargets();
                if(PlayerCamera.instance.rightLockOnTarget != null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                }
            }
        }
    }

    
}
