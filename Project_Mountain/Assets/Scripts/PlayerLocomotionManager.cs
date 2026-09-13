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

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 5;


    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleGroundMovement();
        HandleRotation();
    }

    private void GetMovementValues()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput; 
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundMovement()
    {
        GetMovementValues();

        Vector3 camForwardDirection = Camera.main.transform.forward;
        camForwardDirection.y=0;

        Vector3 camRightDirection = Camera.main.transform.right;
        camRightDirection.y=0;

        moveDirection = camForwardDirection*verticalMovement + camRightDirection*horizontalMovement;
        moveDirection.Normalize();
        
        if(PlayerInputManager.instance.moveAmount > 0.5f)
        {
            player.characterController.Move(moveDirection*runSpeed*Time.deltaTime);
        }else if(PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            player.characterController.Move(moveDirection*walkSpeed*Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        Vector3 camForwardDirection = Camera.main.transform.forward;
        camForwardDirection.y=0;

        Vector3 camRightDirection = Camera.main.transform.right;
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
}
