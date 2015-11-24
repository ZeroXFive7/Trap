using UnityEngine;
using System.Collections;

public class GameplayManager : SingletonMonobehaviour<GameplayManager>
{
    [Header("Prefab References")]
    [SerializeField]
    private Character characterPrefab = null;
    [SerializeField]
    private PlayerCamera playerCameraPrefab = null;

    [SerializeField]
    private Color[] characterColors = null;

    [Header("Settings")]
    private int enemyCount = 3;

    private static readonly int playerLayerId = LayerMask.NameToLayer("Player");

    public Level CurrentLevel { get; private set; }

    public Character SpawnPlayerCharacter()
    {
        Character newPlayer = Instantiate(characterPrefab);
        newPlayer.gameObject.layer = playerLayerId;
        newPlayer.Renderer.material.color = characterColors[Random.Range(0, characterColors.Length - 1)];

        PlayerCamera playerCamera = Instantiate(playerCameraPrefab);
        playerCamera.transform.SetParent(newPlayer.Aiming.transform, false);

        PlayerInput input = newPlayer.gameObject.AddComponent<PlayerInput>();
        PlayerController controller = newPlayer.gameObject.AddComponent<PlayerController>();
        controller.Character = newPlayer;
        controller.Camera = playerCamera;
        controller.Input = input;

        newPlayer.Health.Respawn();
        return newPlayer;
    }

    public Character SpawnNonPlayerCharacter()
    {
        Character newCharacter = Instantiate(characterPrefab);
        newCharacter.Renderer.material.color = characterColors[Random.Range(0, characterColors.Length - 1)];

        newCharacter.Health.Respawn();
        return newCharacter;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = FindObjectOfType<Level>();

        Camera[] allCameras = FindObjectsOfType<Camera>();
        for (int i = 0; i < allCameras.Length; ++i)
        {
            if (allCameras[i] != null)
            {
                Destroy(allCameras[i].gameObject);
            }
        }

        SpawnPlayerCharacter();

        for (int i = 0; i < enemyCount; ++i)
        {
            SpawnNonPlayerCharacter();
        }
    }
}
