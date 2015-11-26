using UnityEngine;
using System.Collections.Generic;

public class GameplayManager : SingletonMonobehaviour<GameplayManager>
{
    [System.Serializable]
    private struct SplitscreenViewport
    {
        public string Name;
        public Rect[] PlayerViewports;
    };

    [Header("Prefab References")]
    [SerializeField]
    private Character characterPrefab = null;
    [SerializeField]
    private PlayerCamera playerCameraPrefab = null;
    [SerializeField]
    private PlayerUI playerUIPrefab = null;

    [SerializeField]
    private Color[] characterColors = null;
    [SerializeField]
    private SplitscreenViewport[] splitscreenViewports = null;

    [Header("Settings")]
    [SerializeField]
    private int enemyCount = 3;
    [SerializeField]
    private int maxPlayerCount = 4;

    private int colorIndex = 0;

    private HashSet<int> activePlayerInputIds = new HashSet<int>();

    public List<PlayerController> Players = new List<PlayerController>();

    public Level CurrentLevel { get; private set; }

    private void Awake()
    {
        Instance = this;
        colorIndex = Random.Range(0, characterColors.Length - 1);
    }

    private void Start()
    {
        CurrentLevel = FindObjectOfType<Level>();

        for (int i = 0; i < enemyCount; ++i)
        {
            SpawnNonPlayerCharacter();
        }
    }

    private void Update()
    {
        for (int i = 0; i < Rewired.ReInput.players.Players.Count; ++i)
        {
            int id = Rewired.ReInput.players.Players[i].id;

            bool playerInputIsActive = activePlayerInputIds.Contains(id);

            if (Rewired.ReInput.players.Players[i].GetButton("Join Game") && !playerInputIsActive && Players.Count < maxPlayerCount)
            {
                activePlayerInputIds.Add(id);
                SpawnPlayerCharacter(id, "Player" + (Players.Count + 1));
                UpdateViewports();
            }
            else if (Rewired.ReInput.players.Players[i].GetButton("Quit Game") && playerInputIsActive)
            {
                activePlayerInputIds.Remove(id);
                DestroyPlayerCharacter(id);
                UpdateViewports();
            }
        }
    }

    private void SpawnPlayerCharacter(int playerInputId, string layerName)
    {
        int playerLayerId = LayerMask.NameToLayer(layerName);

        Character newPlayer = Instantiate(characterPrefab);
        newPlayer.gameObject.layer = playerLayerId;
        newPlayer.Renderer.material.color = GetNextCharacterColor();

        PlayerCamera playerCamera = Instantiate(playerCameraPrefab);
        playerCamera.transform.SetParent(newPlayer.Aiming.transform, false);
        playerCamera.FirstPersonCullLayer = layerName;

        PlayerUI playerUI = Instantiate(playerUIPrefab);
        playerUI.transform.SetParent(newPlayer.transform, false);

        PlayerController controller = newPlayer.gameObject.AddComponent<PlayerController>();
        controller.PlayerInputId = playerInputId;
        controller.Character = newPlayer;
        controller.Camera = playerCamera;
        controller.UI = playerUI;

        playerUI.Player = controller;

        newPlayer.Health.Respawn();

        Players.Add(controller);
    }

    private Character SpawnNonPlayerCharacter()
    {
        Character newCharacter = Instantiate(characterPrefab);
        newCharacter.Renderer.material.color = GetNextCharacterColor();

        newCharacter.Health.Respawn();
        return newCharacter;
    }

    private void DestroyPlayerCharacter(int playerInputId)
    {
        for (int i = 0; i < Players.Count; ++i)
        {
            if (Players[i].PlayerInputId == playerInputId)
            {
                PlayerController player = Players[i];
                Players.RemoveAt(i);
                Destroy(player.gameObject);
            }
        }
    }

    private void UpdateViewports()
    {
        SplitscreenViewport viewports = splitscreenViewports[Players.Count - 1];
        for (int i = 0; i < viewports.PlayerViewports.Length; ++i)
        {
            if (i < Players.Count)
            {
                Players[i].Camera.Viewport = viewports.PlayerViewports[i];
            }
        }
    }

    private Color GetNextCharacterColor()
    {
        Color color = characterColors[colorIndex];
        colorIndex = (colorIndex + 1) % characterColors.Length;
        return color;
    }
}
