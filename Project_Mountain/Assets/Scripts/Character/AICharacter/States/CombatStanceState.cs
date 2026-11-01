using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/States/CombatStance")]
public class CombatStanceState : AIState
{
    [Header("Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks; // LIST OF ALL POSSIBLE ATTACKS
    protected List<AICharacterAttackAction> potentialAttacks;  // LIST CREATED WHEN STATE IS ENTERED, LISTS ALL POSSIBLE ATTACKS IN THE GIVEN SITUATION
    private AICharacterAttackAction chosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Combo")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    protected bool hasRolledForComboChance = false;

    [Header("Engagement Distance")]
    [SerializeField] public float maxEngagementDistance = 5; //DISTANCE WE HAVE TO BE AWAY FROM FOR ENEMY TO SWITCH FROM COMBAT -> PURSUE

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        //PROCESS ANY COMBAT LOGIC WHILE WAITING TO ATTACK(BLOCK, DODGE, STRAFE ETC.)
        if (aiCharacter.isPerformingAction)
        {
            return this;
        }

        if (!aiCharacter.navmeshAgent.enabled)
        {
            aiCharacter.navmeshAgent.enabled = true;
        }

        if (!aiCharacter.isMoving)
        {
            if(aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
            {
                // aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
        }

        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        //IF TARGET IS NO LONGER PRESENT, RETURN TO IDLE
        if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
             return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
        }
        else
        {
            //CHECK RECOVERY TIMER
            aiCharacter.attack.currentAttack = chosenAttack;
            //ROLL FOR COMBO CHANCE

            //SWITCH STATE
            return SwitchState(aiCharacter, aiCharacter.attack);
        }

        //IF TARGET MOVES OUT OF RANGE SWICTH TO PURSUESTATE
        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget > maxEngagementDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navmeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navmeshAgent.SetPath(path);

        return this;
    }
    //SELECT ATTACK FOR ATTACK STATE
    //DEPENDS ON DISATNCE, ANGLE
    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {
        potentialAttacks = new List<AICharacterAttackAction>();

        foreach(var potentialAttack in aiCharacterAttacks)
        {
            if(potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }

            if(potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }

            if(potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
            {
                continue;
            }

            if(potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
            {
                continue;
            }

            potentialAttacks.Add(potentialAttack);
        }

        if(potentialAttacks.Count <= 0)
        {
            return;
        }

        var totalWeight = 0;
        foreach(var attack in potentialAttacks)
        {
            totalWeight = attack.attackWeight;
        }

        var randomWeightValue = Random.Range(1, totalWeight+1);
        var processedWeight = 0;

        foreach(var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if(randomWeightValue <= processedWeight)
            {
                chosenAttack = attack;
                previousAttack = chosenAttack;
                hasAttack = true;
                return;
            }
        }
    }

    protected virtual bool RollForOutcomeChance(int outcomeChance)
    {
        bool outcomeWillbePerformed = false;

        int randomPercentage = Random.Range(0, 100);
        
        if(randomPercentage < outcomeChance)
        {
            outcomeWillbePerformed = true;
        }

        return outcomeWillbePerformed;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);

        hasRolledForComboChance = false;
        hasAttack = false;
    }


}
