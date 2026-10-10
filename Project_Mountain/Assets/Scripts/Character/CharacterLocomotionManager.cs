using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Ground Check")]
    [SerializeField] protected float gravityForce = -5.55f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSphereRadius = 1;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedVelocity = -20;
    [SerializeField] protected float fallStartVelocity = -5;
    protected bool fallingVelocitySetCheck = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Update()
    {
        HandleGroundCheck();

        if (!character.isGrounded)
        {
            if (!fallingVelocitySetCheck)
            {
                fallingVelocitySetCheck = true;
                yVelocity.y = fallStartVelocity;
            }

            inAirTimer = inAirTimer+Time.deltaTime;
            character.animator.SetFloat("inAirTimer", inAirTimer);
            yVelocity.y +=gravityForce*Time.deltaTime;
        }

        //ALWAYS KEEP A DOWNWARD FORCE ON THE CHARACTER
        character.characterController.Move(yVelocity*Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {   
        Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }
}
