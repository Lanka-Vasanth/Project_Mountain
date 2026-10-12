using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage Effect")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")]
    public CharacterManager characterCausingDamage;

    [Header("Damage")]
    public float physicalDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;

    [Header("Final Damage")]
    private int finalDamageDealt = 0;

    [Header("Poise")]
    public float poiseDamage = 0;
    public bool poiseIsBroken = false;

    //BUILDUPS
    // public float poisonDamage = 0;
    // public float bleedDamage = 0;
    // public float iceDamage = 0;

    [Header("Sound FX")]
    public bool willPlayDamageSFX = true;
    public AudioClip elementalSFX;

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        if (character.isDead)
        {
            return;
        }

        //CHECK INVULNERABILITY

        //CALCUKATE DAMAGE
        CalculateDamage(character);

        //CHECL DAMAGE DIRECTION

        //PLAY DAMAGE ANIMATION

        //CHECK FOR BUILDUP (POISON, BLEED, ICE ETC)

        //PLAY DAMAGE VFX, SFX

    }

    private void CalculateDamage(CharacterManager character)
    {
        if(characterCausingDamage != null)
        {
            //CHECK FOR DAMAGE MODIFIERS, TO MODIFY BASE DAMAGE
        }

        //CHECK FOR ARMOR ABSORPTIONS< SUBTRACT FROM BASE DAMAGE

        finalDamageDealt = Mathf.RoundToInt(physicalDamage + fireDamage + lightningDamage);

        if(finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }
        
        character.currentHealth -= finalDamageDealt;

        //CALCULATE POISE DAMAGE, TO DETERMINE IF CHARACTER IS STUNNED 
    }
}
