using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDashSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.dashSFX);
    }
}
