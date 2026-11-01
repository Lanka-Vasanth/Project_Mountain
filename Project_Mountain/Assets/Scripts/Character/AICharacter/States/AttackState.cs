using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "AI/States/Attack")]
public class AttackState : AIState
{
    [Header("Current Attack")]
    public AICharacterAttackAction currentAttack;
    public bool willPerformCombo = false;

    [Header("State Flags")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack= false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }
    
        //ROTATE TOWARD TARGET WHILE ATTACKING
        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhileAttack(aiCharacter);

        //SET MOVEMENT VALUES TO 0
        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0,0, false);

        //PERFORM A COMBO
        if(willPerformCombo && !hasPerformedCombo)
        {
            if (currentAttack.comboAction != null)
            {

                //IF CAN COMBO 
                // hasPerformedCombo = true;
                // currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }

        if (aiCharacter.isPerformingAction)
        {
            return this;
        }

        if (!hasPerformedAttack)
        {
            //IF STILL RECOVERING FROM PERFORMING  AN ACTION, WIAT BEFORE PERFORMING THE NEXT
            if(aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
            {
                return this;
            }

            PerformAttack(aiCharacter);

            return this;
        }

        if (pivotAfterAttack)
        {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        return SwitchState(aiCharacter, aiCharacter.combatStance);
    }

    protected void PerformAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);

        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
    }

    protected override void ResetStateFlags(AICharacterManager aICharacter)
    {
        base.ResetStateFlags(aICharacter);

        hasPerformedAttack = false;
        hasPerformedCombo = false;
    }
}
