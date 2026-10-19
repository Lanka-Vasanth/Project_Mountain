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
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
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
}
