using UnityEngine;

public class PlayerUIHUDManager : MonoBehaviour
{
    [SerializeField] UI_Statbar staminaBar;

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue)); 
    }
    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina); 
    }
}
