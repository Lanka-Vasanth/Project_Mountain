using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterAnimatorManager : MonoBehaviour
{
    CharacterManager character;

    int vertical;
    int horizontal;

    [Header("Damage Animations")]
    public string lastAnimationPlayed;

    [SerializeField] public string hitForward01 = "hitForward01";
    [SerializeField] public string hitBackward01 = "hitBackward01";
    [SerializeField] public string hitLeftward01 = "hitLeftward01";
    [SerializeField] public string hitRightward01 = "hitRightward01";

    public List<string> forward_Damage = new List<string>();
    public List<string> backward_Damage = new List<string>();
    public List<string> leftward_Damage = new List<string>();
    public List<string> rightward_Damage = new List<string>();

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    protected virtual void Start()
    {
        forward_Damage.Add(hitForward01);
        backward_Damage.Add(hitBackward01);
        leftward_Damage.Add(hitLeftward01);
        rightward_Damage.Add(hitRightward01);
    }

    public string GetRandomAnimationFromList(List<string> animationList)
    {
        List<string> finalList = new List<string>();

        foreach(var item in animationList)
        {
            finalList.Add(item);
        }

        //CHECK FOR A RECENTLY USED ANIM, TO AVOID REPEAT
        finalList.Remove(lastAnimationPlayed);

        for(int i = finalList.Count - 1; i >= 0; i--)
        {
            if(finalList[i] == null)
            {
                finalList.RemoveAt(i);
            }
        }

        int randomValue = Random.Range(0, finalList.Count);

        return finalList[randomValue];
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
    {
        float snappedHorizontalAmount = horizontalValue;
        float snappedVerticalAmount = verticalValue;

        if(horizontalValue > 0 && horizontalValue <= 0.5f)
        {
            snappedHorizontalAmount = 0.5f;
        }
        else if (horizontalValue > 0.5f && horizontalValue <= 1)
        {
            snappedHorizontalAmount = 1;
        }
        else if(horizontalValue < 0 && horizontalValue >= -0.5f)
        {
            snappedHorizontalAmount = -0.5f;
        }
        else if(horizontalValue < -0.5f && horizontalValue >= -1f)
        {
            snappedHorizontalAmount = -1f;
        }
        else
        {
            snappedHorizontalAmount = 0;
        }

        if(verticalValue > 0 && verticalValue <= 0.5f)
        {
            snappedVerticalAmount = 0.5f;
        }
        else if (verticalValue > 0.5f && verticalValue <= 1)
        {
            snappedVerticalAmount = 1;
        }
        else if(verticalValue < 0 && verticalValue >= -0.5f)
        {
            snappedVerticalAmount = -0.5f;
        }
        else if(verticalValue < -0.5f && verticalValue >= -1f)
        {
            snappedVerticalAmount = -1f;
        }
        else
        {
            snappedVerticalAmount = 0;
        }

        // if (isSprinting)
        // {
        //     snappedHorizontalAmount = 2;
        //     snappedVerticalAmount = 2;
        // }

        character.animator.SetFloat(horizontal, snappedHorizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, snappedVerticalAmount, 0.1f, Time.deltaTime);
    }

    public virtual void PlayTargetActionAnimation(string targetAnimation, 
                                                  bool isPerformingAction, 
                                                  bool applyRootMotion = true, 
                                                  bool canRotate = false, 
                                                  bool canMove = false)
    {
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        //STOPS CHARACTER FROM ATTEMPTING NEW ACTIONS,
        //EXAMPLE IF DAMAGED, PREVENTS STUNLOCKING 
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;
    }

    public virtual void PlayTargetAttackActionAnimation(AttackType attackType,
                                                  string targetAnimation, 
                                                  bool isPerformingAction, 
                                                  bool applyRootMotion = true,  
                                                  bool canRotate = false, 
                                                  bool canMove = false)
    {
        //KEEP TRACK OF LAST ATTACK PERFORMED FOR COMBOS

        //KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY)
        character.characterCombatManager.currentAttackType = attackType;
        
        //UPDATE ANIMATION SET TO CURRENT WEAPON ANIAMTIONS

        //DECIDE IF ATTACK CAN BE PARRIED
        
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);

        //STOPS CHARACTER FROM ATTEMPTING NEW ACTIONS,
        //EXAMPLE IF DAMAGED, PREVENTS STUNLOCKING 
        character.isPerformingAction = isPerformingAction;
        character.canMove = canMove;
        character.canRotate = canRotate;
    }
}
