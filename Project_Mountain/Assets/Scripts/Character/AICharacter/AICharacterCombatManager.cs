using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Timeline;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Target Information")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;

    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] public float minimumFOV = -35;
    [SerializeField] public float maximumFOV = 35;

    [Header("Attack Rotation")]
    public float attackRotationSpeed = 25;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    public void FindTargetAlongLineOfSight(AICharacterManager aiCharacter)
    {
        if(currentTarget != null)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if (targetCharacter == null)
            {
                continue;
            }

            if (targetCharacter == aiCharacter)
            {
                continue;
            }

            if (targetCharacter.isDead)
            {
                continue;
            }

            if(WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                //IF POTENTIAL TARGET FOUND, CHECK IF WITHIN VIEWABLE ANGLE (IN FRONT)
                Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                {
                    //CHECK FOR ANY ENVIRONMENT BLOCKING LOS
                    if(Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                                        targetCharacter.characterCombatManager.lockOnTransform.position, 
                                        WorldUtilityManager.instance.GetEnvironmentLayers()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        Debug.Log("BLOCKED");
                    }
                    else
                    {
                        targetDirection = targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetDirection);

                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        // PivotTowardsTarget(aiCharacter);
                    }
                }
            }
        }
    }

    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        //PLAY PIVOT ANIMATION BASED ON VIEWABLE ANGLE
        if (aiCharacter.isPerformingAction)
        {
            return;
        }

        if(viewableAngle >= 20 && viewableAngle <= 60)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Right_45", true);
        }else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Left_45", true);
        }else if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Right_90", true);
        }else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Left_90", true);
        }

        if(viewableAngle >= 110 && viewableAngle <= 145)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Right_135", true);
        }else if (viewableAngle <= -110 && viewableAngle >= -145)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Left_135", true);
        }

        if(viewableAngle >= 146 && viewableAngle <= 180)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Right_180", true);
        }else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            // aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Pivot_Left_180", true);
        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isMoving)
        {
            aiCharacter.transform.rotation = aiCharacter.navmeshAgent.transform.rotation;
        }
    }

    public void RotateTowardsTargetWhileAttack(AICharacterManager aiCharacter)
    {
        if(currentTarget == null)
        {
            return;
        }

        //CHECK IF WE CAN ROTATE
        if (!aiCharacter.canRotate)
        {
            return;
        }

        if (!aiCharacter.isPerformingAction)
        {
            return;
        }

        //ROTATE TOWARD TARGET WRT SPECIFIED ROTATION ATTACKSPEED DURING SPECIFIED FRAMES
        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;

        targetDirection.y = 0;
        targetDirection.Normalize();


        if(targetDirection == Vector3.zero)
        {
            targetDirection = aiCharacter.transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed*Time.deltaTime);
    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if(actionRecoveryTimer > 0)
        {
            if (!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;
            }
        }
    }
}
