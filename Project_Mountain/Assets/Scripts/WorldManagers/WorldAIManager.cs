using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldAIManager : MonoBehaviour
{
    public static WorldAIManager instance;

    [Header("DEBUG")]
    [SerializeField] bool despawnCharacters = false;
    [SerializeField] bool respawnCharacters = false;

    [Header("Characters")]
    [SerializeField] GameObject[] aiCharacters;
    [SerializeField] List<GameObject> spawnedCharacters;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (despawnCharacters)
        {
            despawnCharacters = false;
            DespawnAllCharacters();
        }

        if (respawnCharacters)
        {
            respawnCharacters = false;
            SpawnAllCharacters();
        }
    }

    private void Start()
    {
        StartCoroutine(WaitForSceneLoadThenSpawnCharacters());
    }

    private IEnumerator WaitForSceneLoadThenSpawnCharacters()
    {
        while (!SceneManager.GetActiveScene().isLoaded)
        {
            yield return null;
        }

        SpawnAllCharacters();
    }

    private void SpawnAllCharacters()
    {
        foreach(var character in aiCharacters)
        {
            GameObject instantiatedCharacter = Instantiate(character);
            spawnedCharacters.Add(instantiatedCharacter);
        }
    }

    private void DespawnAllCharacters()
    {
        foreach (var character in spawnedCharacters)
        {
            Destroy(character);
        }
    }

    private void DisableAllCharacters()
    {
        
    }
}
