using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ConstellationNumRef {
    Perseus = 0,
    Lovers = 1,
    Trickster = 2,
    Dionysus = 3,
    Draco = 4,
    Gemini = 5
}

public class GameManager : Singleton<GameManager> {


    /// <summary> Internal reference to the active GameManager; </summary>
    // private static GameManager _instance;
    // /// <summary> Public instance reference to the active GameManager; </summary>
    // public static GameManager Instance => _instance;

    public int ConstellationSceneTransfer = 2;

    public enum DialogueState
    {
        NotTalking,
        Talking,
        Puzzle
    }

    public DialogueState dialogueState;
    public string lastInteractionId;
    public bool puzzleComplete;

    public Transform lastPosition;
    public int playerLevel;
    public int expAmount;

    public const string RESET_FILE_NAME = "newGame";

    void Awake() {
        // /// Initialize Singleton;
        // if (_instance != null) {
        //     Destroy(gameObject);
        // } else {
        //     _instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        InitializeSingleton();
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        dialogueState = DialogueState.NotTalking;
        puzzleComplete = false;
        lastInteractionId = "";
        SaveSystem.SaveGame(RESET_FILE_NAME);
        // SaveSystem.TryLoadGame();
    }
}
