using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    //PROCESS INSTANT EFFECTS (DAMAGE< HEAL ETC)
    public virtual void ProcessInstantEffects(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }

    //PROCESS TIMED EFFECTS (POISON< BUILD UPS OVER TIME)

    //PROCESS STATIC EFFECTS (BUFFS FROM ITEMS, PASSIVE ABILITIES )
}
