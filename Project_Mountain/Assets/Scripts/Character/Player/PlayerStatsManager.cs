using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        CalculateHealthBasedOnVitalityLevel(player.vitality);
        CalculateStaminaBasedOnEnduranceLevel(player.endurance);
    }
}
