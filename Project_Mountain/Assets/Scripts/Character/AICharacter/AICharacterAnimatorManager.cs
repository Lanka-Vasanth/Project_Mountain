using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {
        if (!aiCharacter.characterLocomotionManager.isGrounded)
        {
            return;
        }

        Vector3 velocity = aiCharacter.animator.deltaPosition;

        aiCharacter.characterController.Move(velocity);
        aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
    }
}
