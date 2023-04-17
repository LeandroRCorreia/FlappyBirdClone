using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScreenController))]
public class MainHUD : MonoBehaviour
{
    [SerializeField] private GameMode gamemode;

    [Header("Overlays")]
    [SerializeField] private GameObject waitingStartGameOverlay;
    [SerializeField] private GamePlayOverlay gamePlayOverlay;
    [SerializeField] private GameOverOverlay gameOverOverlay;
    [SerializeField] private GameObject pauseGameOverlay;
    [SerializeField] private Image fadeInAndOutOverlay;

    private ScreenController screenController;

    private void Awake() 
    {
        screenController = GetComponent<ScreenController>();    
    }


}
