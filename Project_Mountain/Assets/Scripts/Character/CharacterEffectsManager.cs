using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("VFX")]
    [SerializeField] GameObject bloodSplatterFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    //PROCESS INSTANT EFFECTS (DAMAGE< HEAL ETC)
    public virtual void ProcessInstantEffects(InstantCharacterEffect effect)
    {
        effect.ProcessEffect(character);
    }

    public void PlayBloodSplatter(Vector3 contactPoint)
    {
        //IF THERE IS A DIFFERENT KIND OF VFX TO PLAY IN CERTAIN PLACES, ELSE PLAY THE NORMAL
        if(bloodSplatterFX != null)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterFX, contactPoint, Quaternion.identity);
        }
        else
        {
            GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterFX, contactPoint, Quaternion.identity);
        }
    }

    //PROCESS TIMED EFFECTS (POISON< BUILD UPS OVER TIME)

    //PROCESS STATIC EFFECTS (BUFFS FROM ITEMS, PASSIVE ABILITIES )
}
