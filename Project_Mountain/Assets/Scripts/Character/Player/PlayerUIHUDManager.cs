using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUIHUDManager : MonoBehaviour
{
    [Header("STAT BARS")]
    [SerializeField] UI_Statbar healthBar;
    [SerializeField] UI_Statbar staminaBar;

    // [Header("Quick Slots")]

    public void RefreshHUD()
    {
        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);

        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);
    }

    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue); 
    }
    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth); 
    }

    public void SetNewStaminaValue(float oldValue, float newValue)
    {
        staminaBar.SetStat(Mathf.RoundToInt(newValue)); 
    }
    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetMaxStat(maxStamina); 
    }
}
