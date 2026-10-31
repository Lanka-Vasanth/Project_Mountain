using UnityEngine;

public class AIState : ScriptableObject
{
    public virtual AIState Tick(AICharacterManager aiCharacter)
    {
        return this; 
    }

    protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
    {
         ResetStateFlags(aiCharacter);
         return newState;
    }

    protected virtual void ResetStateFlags(AICharacterManager aICharacter)
    {
        //RESET ANY STATE FLAGS SO WHEN YOU RETURN, THEY ARE AT DEFAULT
    }
}
