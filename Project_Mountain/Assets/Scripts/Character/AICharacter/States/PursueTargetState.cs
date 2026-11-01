using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/PursueTarget")]
public class PursueTargetState : AIState
{
    public override AIState Tick(AICharacterManager aiCharacter)
    {
        //CHECK IF PERFORMING ACTION(WAIT TILL DONE)
        if(aiCharacter.isPerformingAction)
        {
            return this;
        }

        //CHECK IF TARGET IS NULL, RETURN TO IDLE IF NO TARGET
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        //MAKE SURE NAVMESH AGENT IS ACTIVE
        if(!aiCharacter.navmeshAgent.enabled)
        {
            aiCharacter.navmeshAgent.enabled = true;
        }

        //IF TARGET GOES OUT OF FOV, THEN PIVOT TOWARD
        if(aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV 
           || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
        {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiCharacterLocomotionManager.RotateTowardAgent(aiCharacter);

        //IF WITHIN COMBAT RANGE, SWITCH TO COMBAT STATE
        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navmeshAgent.stoppingDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        //IF TARGET UNREACHABLE< RETURN TO HOMING POSITION

        //PURSUE TARGET
        //COULD ALSO JUST DO: aiCharacter.navmeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        NavMeshPath path = new NavMeshPath();
        aiCharacter.navmeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navmeshAgent.SetPath(path);

        return this;
    }
}
