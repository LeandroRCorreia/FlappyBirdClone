using TMPro;
using UnityEngine;

public class GamePlayOverlay : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;

    [Header("Ui Elements")]
    
    [SerializeField] private TextMeshProUGUI scoreText;

    void LateUpdate()
    {
        scoreText.text = gameMode.CurrentScore.ToString();
    }

}
