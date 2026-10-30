using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Timeline;

public class AICharacterCombatManager : CharacterCombatManager
{
    [Header("Detection")]
    [SerializeField] float detectionRadius = 15;
    [SerializeField] float minimumDetectionAngle = -35;
    [SerializeField] float maximumDetectionAngle = 35;

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
                float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                if(viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
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
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                    }
                }
            }
        }
    }
}
