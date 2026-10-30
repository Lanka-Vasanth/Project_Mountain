using UnityEngine;

[CreateAssetMenu(menuName = "AI/States/Idle")]
public class IdleState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {    
        if(aiCharacter.characterCombatManager.currentTarget != null)
        {
            //RETURN PURSUE TARGET STATE 
            Debug.Log("WE HAVE A TARGET");
            return this;
        }
        // RETURN TO THIS STATE< CONTINUALLY SEARCH FOR TARGET
        else
        {
            aiCharacter.aiCharacterCombatManager.FindTargetAlongLineOfSight(aiCharacter);
            return this;

        }
    }
}
