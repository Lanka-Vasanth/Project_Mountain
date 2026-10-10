using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public PlayerManager player;

    //VALUES WILL BE TAKEN FROM THE INPUTMANAGER
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    [Header("MOVEMENT SETTINGS")]
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 5;
    [SerializeField] float sprintSpeed = 15;
    [SerializeField] float sprintingStaminaCost = 2f;
    [SerializeField] float freeFallSpeed = 2;

    [Header("Dash")]
    private Vector3 dashDirection;
    [SerializeField] int dashStaminaCost = 25;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
        HandleFreeFallMovement();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput; 
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundMovement()
    {
        if (!player.canMove)
        {
            return;
        }

        GetMovementValues();

        Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
        camForwardDirection.y=0; 

        Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
        camRightDirection.y=0;

        moveDirection = camForwardDirection*verticalMovement + camRightDirection*horizontalMovement;
        moveDirection.Normalize();

        if (player.isSprinting)
        {
            player.characterController.Move(moveDirection*sprintSpeed*Time.deltaTime);
        }else{
            
            if(PlayerInputManager.instance.moveAmount > 0.5f)
            {
                player.characterController.Move(moveDirection*runSpeed*Time.deltaTime);
            }else if(PlayerInputManager.instance.moveAmount <= 0.5f)
            {
                player.characterController.Move(moveDirection*walkSpeed*Time.deltaTime);
            }
        }
    }

    private void HandleFreeFallMovement()
    {
        if (!player.isGrounded)
        {
            Vector3 freeFallDirection;
            Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
            camForwardDirection.y=0; 

            Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
            camRightDirection.y=0;

            freeFallDirection = camForwardDirection*PlayerInputManager.instance.verticalInput + camRightDirection*PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;
            freeFallDirection.Normalize();

            player.characterController.Move(freeFallDirection*freeFallSpeed*Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (!player.canRotate)
        {
            return;
        }
        Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
        camForwardDirection.y=0;

        Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
        camRightDirection.y=0;

        targetRotationDirection = Vector3.zero;
        targetRotationDirection = camForwardDirection*verticalMovement + camRightDirection*horizontalMovement;
        targetRotationDirection.Normalize();

        if(targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed*Time.deltaTime);
    
        transform.rotation = targetRotation;
    }

    public void HandleSprinting()
    {
        if (player.isPerformingAction)
        {
            player.isSprinting = false;
        }

        if(player.currentStamina <= 0)
        {
            player.isSprinting = false;
            return;
        }

        if(moveAmount >= 0.5)
        {
            player.isSprinting = true;
        }
        else
        {
            player.isSprinting = false;
        }

        if (player.isSprinting)
        {
            player.currentStamina -= sprintingStaminaCost * Time.deltaTime;
        }

    }

    public void AttemptToPerformDash()
    {
        if (player.isPerformingAction)
        {
            return;
        }

        if(player.currentStamina <= 0)
        {
            return;
        }

        if(PlayerInputManager.instance.moveAmount > 0){
            Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
            camForwardDirection.y=0;

            Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
            camRightDirection.y=0;

            dashDirection = camForwardDirection * PlayerInputManager.instance.verticalInput + camRightDirection * PlayerInputManager.instance.horizontalInput;
            dashDirection.y = 0;
            dashDirection.Normalize();
            Quaternion playerRotation = Quaternion.LookRotation(dashDirection);

            player.transform.rotation = playerRotation;

            //PERFORM DASH ANIMATION
            player.playerAnimatorManager.PlayTargetActionAnimation("Dash", true, true);
        }
        //IF STATIONARY
        else
        {
            //PERFORM BACKSTEP ANIMATION
            player.playerAnimatorManager.PlayTargetActionAnimation("Backflip", true, true);
        }

        player.currentStamina -= dashStaminaCost;
    }

}
