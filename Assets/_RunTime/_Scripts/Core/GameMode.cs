using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(GameSaver))]
public class GameMode : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ScreenController screenController;
    [SerializeField] private GameSaver gameSaver;


    [Header("Player Movement Params")]
    [SerializeField] private PlayerMovementParams playerRunTimeMovement;
    [SerializeField] private PlayerMovementParams PlayerFrozenMovement;

    [field:Space]
 
    [field:Header("Gameplay")]

    [field: SerializeField] public int CurrentScore {get; private set;} = 0;


    public bool IsGameStarted { get; private set; } = false;
    public bool IsGameOver { get; private set; } = false;
    public bool IsBestScore {get; private set;}
    
    void Start()
    {
        playerController.playerMovement = playerRunTimeMovement;
        StartCoroutine(WaitingForStartGame());
    }

    private IEnumerator WaitingForStartGame()
    {
        IsGameStarted = false;
        var auxInspectorGravityScale = playerController.Rb.gravityScale;
        playerController.Rb.gravityScale = 0;
        screenController.ShowWaitingStartGameOverlay();
        while(!playerController.StartGameTrigger)
        {
            yield return null;

        }
        playerController.Rb.gravityScale = auxInspectorGravityScale;
        IsGameStarted = true;
        screenController.ShowGamePlayOverlay();
    }

    public void OnPassedPipe()
    {
        CurrentScore++;
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
    }

    public void OnResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OnGameOver()
    {
        IsBestScore = CurrentScore > gameSaver.BestScore;
        var bestScore = IsBestScore ? CurrentScore : gameSaver.BestScore;  
        gameSaver.SaveGame(bestScore, CurrentScore);
        playerController.playerMovement = PlayerFrozenMovement;

        IsGameOver = true;
        screenController.ShowGameOverOverlay();
    }

    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneCoro());
    }

    private IEnumerator ReloadSceneCoro()
    {
        yield return StartCoroutine(screenController.FlashCoro());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
