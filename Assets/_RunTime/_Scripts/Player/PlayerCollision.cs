using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    [SerializeField] private ScreenController screenController;

    [SerializeField] private AudioClip pipeHitSound;
    private PlayerController playerController;

    private void Awake() 
    {
        playerController = GetComponent<PlayerController>();    
    }

    private void LateUpdate() 
    {
        if(playerController.DeathZone)
        {
            GameOver();
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag(GameTags.Obstacle))
        {
            GameOver();
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag(GameTags.Obstacle))
        {
            GameOver();
        }

    }

    private void OnTriggerExit2D(Collider2D other) 
    {

        if(other.CompareTag(GameTags.PassedTrigger) && !playerController.IsDead)
        {
            gameMode.OnPassedPipe();
            
        }

    }

    private void GameOver()
    {
        if(gameMode.IsGameOver) return;
        AudioUtility.PlaySFX(playerController.AudioSource, pipeHitSound);
        playerController.Death();
        gameMode.OnGameOver();
        screenController.ShowEndGameOverlays();

    }

}
