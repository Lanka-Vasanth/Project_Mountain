using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.IO;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    public PlayerManager player;
    [SerializeField] public Transform debugHitPointTransform;

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

    [Header("Grapple")]
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] float grappleMaxRange = 50f;
    [SerializeField] float grappleMinRange = 5f;
    [SerializeField] float grappleFacingAngle = 15f;
    [SerializeField] float grappleLedgeClearance = 3f;
    [SerializeField] float grappleSpeedMin = 40f;
    [SerializeField] float grappleSpeedMax = 80f;
    [SerializeField] float grappleRampTime = 0.5f;
    [SerializeField] float grappleArc = 0.5f;
    [SerializeField] float grappleOvershootDrag = 15f;
    private Vector3 grapplePosition;
    private Vector3 grappleDirection;
    private Vector3 grappleSlideVelocity;
    private float grappleTimer;
    private float grappleSpeed;
    private bool grappleWinding;
    private bool grappleOvershooting;
    private bool grappleLedge;
    private Collider[] grappleHits = new Collider[10];


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
        HandleGrappleMovement();
    }

    public void ResetGravity()
    {
        player.playerLocomotionManager.gravityForce = -40f;
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
        camForwardDirection.Normalize();

        Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
        camRightDirection.y=0;
        camRightDirection.Normalize();

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
            camForwardDirection.Normalize();

            Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
            camRightDirection.y=0;
            camRightDirection.Normalize();

            freeFallDirection = camForwardDirection*PlayerInputManager.instance.verticalInput + camRightDirection*PlayerInputManager.instance.horizontalInput;
            freeFallDirection.y = 0;
            freeFallDirection.Normalize();

            player.characterController.Move(freeFallDirection*freeFallSpeed*Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (player.isDead)
        {
            return;
        }

        if (player.isLockedOn){
            if (player.isSprinting || player.playerLocomotionManager.isDashing)
            {
                Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
                camForwardDirection.y=0;
                camForwardDirection.Normalize();

                Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
                camRightDirection.y=0;
                camRightDirection.Normalize();
     
                targetRotationDirection = Vector3.zero;
                targetRotationDirection = camForwardDirection*verticalMovement + camRightDirection*horizontalMovement;
                targetRotationDirection.y = 0;
                targetRotationDirection.Normalize();

                if(targetRotationDirection == Vector3.zero)
                {
                    targetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed*Time.deltaTime);

                transform.rotation = targetRotation;
            }
            else
            {
                if(player.playerCombatManager.currentTarget == null)
                {
                    return;
                }

                Vector3 targetRotationDirection;
                targetRotationDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;

                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed*Time.deltaTime);

                transform.rotation = targetRotation;
            }
        }else{
            if (!player.canRotate)
            {
                return;
            }
            Vector3 camForwardDirection = PlayerCamera.instance.cameraObject.transform.forward;
            camForwardDirection.y=0;
            camForwardDirection.Normalize();

            Vector3 camRightDirection = PlayerCamera.instance.cameraObject.transform.right;
            camRightDirection.y=0;
            camRightDirection.Normalize();

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = camForwardDirection*verticalMovement + camRightDirection*horizontalMovement;
            targetRotationDirection.y = 0;
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
            player.playerLocomotionManager.isDashing = true;
        }
        //IF STATIONARY
        else
        {
            //PERFORM BACKSTEP ANIMATION
            player.playerAnimatorManager.PlayTargetActionAnimation("Backflip", true, true);
        }

        player.currentStamina -= dashStaminaCost;
    }

    public void AttemptToPerformGrapple()
    {
        if (player.isPerformingAction && !player.isGrappling){
            return;
        }

        Vector3 playerOrigin = transform.position;
        Vector3 playerForwardDir = transform.forward;
        playerForwardDir.y = 0f;
        playerForwardDir.Normalize();

        float cosHalfAngle  = Mathf.Cos(grappleFacingAngle * Mathf.Deg2Rad);
        float cosSquared = cosHalfAngle*cosHalfAngle;
        float minRangeSquare = grappleMinRange * grappleMinRange;

        int anchorCount = Physics.OverlapSphereNonAlloc(playerOrigin, grappleMaxRange, grappleHits, grappleLayer, QueryTriggerInteraction.Ignore);

        // PICK NEAREST ANCHOR POINT WITHIN RANGE AND ANGLE WINDOW
        float finalSquared = float.MaxValue;
        Vector3 finalAnchorPoint = default;
        bool finalIsLedge = false;
        bool anchorFound = false;

        for(int i = 0; i < anchorCount; i++)
        {
            Vector3 grappleAnchorPosition  = grappleHits[i].bounds.center;
            Vector3 grappleFlatDistance = grappleAnchorPosition - playerOrigin;
            bool isLedge = grappleFlatDistance.y > 3f;
            grappleFlatDistance.y = 0f;

            float sqr = grappleFlatDistance.sqrMagnitude;
            if(sqr >= finalSquared || sqr < 0.00001f)
            {
                continue;
            }
            if(isLedge && sqr < minRangeSquare)
            {
                continue;
            }

            float along = Vector3.Dot(playerForwardDir, grappleFlatDistance);
            if(along <= 0f || along*along < cosSquared * sqr)
            {
                continue;
            }

            finalSquared = sqr;
            finalAnchorPoint = grappleAnchorPosition;
            finalIsLedge = isLedge;
            anchorFound = true;
        }

        if (!anchorFound)
        {
            return;
        }

        grappleLedge = finalIsLedge;

        grapplePosition = finalIsLedge
        ? finalAnchorPoint + Vector3.up * grappleLedgeClearance
        : new Vector3(finalAnchorPoint.x, playerOrigin.y, finalAnchorPoint.z);

        if (player.isGrappling)
        {
            grappleDirection = (grapplePosition - playerOrigin).normalized;
            grappleOvershooting = false;
            return;
        }

        player.isGrappling = true;
        grappleWinding = true;
        grappleOvershooting = false;
        player.playerAnimatorManager.PlayTargetActionAnimation("Grapple", true, false);
    }

    // ANIMATION EVENT ON THE GRAPPLE ANIMATION
    public void StartGrapple()
    {
        grappleWinding = false;
        grappleTimer = 0f;
        grappleDirection = (grapplePosition - transform.position).normalized;
        yVelocity = Vector3.zero;
    }

    private void HandleGrappleMovement()
    {
        if (!player.isGrappling || grappleWinding){
            return;
        }

        if (grappleOvershooting){
            grappleSlideVelocity *= Mathf.Exp(-grappleOvershootDrag * Time.deltaTime);
            player.characterController.Move(grappleSlideVelocity * Time.deltaTime);

            if (grappleSlideVelocity.sqrMagnitude < 4f)
            {
                player.isGrappling = false;
                player.isPerformingAction = false;
                player.canMove = true;
                player.canRotate = true;
            }
            return;
        }

        grappleTimer += Time.deltaTime;
        float t = Mathf.Clamp01(grappleTimer / grappleRampTime);
        grappleSpeed = Mathf.Lerp(grappleSpeedMin, grappleSpeedMax, t);

        // Vector3 step = grappleDirection * grappleSpeed * Time.deltaTime;
        // player.characterController.Move(step);

        Vector3 moveDirection = grappleDirection;
        if (grappleLedge)
        {
            Vector3 launch = Vector3.Slerp(grappleDirection, Vector3.up, grappleArc);
            moveDirection = Vector3.Slerp(launch, grappleDirection, t);
        }

        yVelocity = Vector3.zero;
        player.characterController.Move(moveDirection * grappleSpeed * Time.deltaTime);

        if (Vector3.Dot(grapplePosition - transform.position, grappleDirection) <= 0f || grappleTimer > 3f)
        {
            grappleOvershooting = true;
            grappleSlideVelocity = moveDirection * grappleSpeed;
            yVelocity = Vector3.zero;
            fallingVelocitySetCheck = true;
        }
    }
}
