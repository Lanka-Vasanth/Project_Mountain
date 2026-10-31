using UnityEngine;

public class AICharacterLocomotionManager : CharacterLocomotionManager
{
    public void RotateTowardAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isMoving)
        {
            aiCharacter.transform.rotation = aiCharacter.navmeshAgent.transform.rotation;
        }
    }
}
