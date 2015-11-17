using UnityEngine;
using System.Collections;

public class GameplayManager : SingletonMonobehaviour<GameplayManager>
{
    public Level CurrentLevel { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentLevel = FindObjectOfType<Level>();
    }
}
