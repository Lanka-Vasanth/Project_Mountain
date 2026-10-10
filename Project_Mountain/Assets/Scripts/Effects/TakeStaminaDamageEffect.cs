using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage Effect")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float staminaDamage;

    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);

        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        //COMPARED TO BASE STAMINA
        character.currentStamina -= staminaDamage;
    }
}
