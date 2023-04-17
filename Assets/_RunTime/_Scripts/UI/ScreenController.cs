using System.Collections;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [SerializeField] private GameObject[] screens;
    [SerializeField] private GameMode gamemode;

    public GameObject ActiveCurrentScreen {get; private set;}

    [Header("Effect")]
    [SerializeField] private Image flashEffectImage;

    private void Awake()
    {
        CloseAllScreens();
    }

    private void CloseAllScreens()
    {
        foreach (GameObject screen in screens)
        {
            screen.SetActive(false);
        }
    }

    private GameObject GetScreenWithType(Type typeOverlay)
    {
        foreach(GameObject screen in screens)
        {
            if(screen.GetComponent(typeOverlay) != null)
            {
                return screen;
            }
        }
        return null;
    }
    
    private void ShowScreen(GameObject screen)
    {
        CloseCurrentScreen();
        screen.SetActive(true);
        ActiveCurrentScreen = screen;
    }

    private void CloseCurrentScreen()
    {
        if(ActiveCurrentScreen != null)
        {
            ActiveCurrentScreen.SetActive(false);
        }

    }

    public void ShowPauseGameOverlay()
    {
        ShowScreen(GetScreenWithType(typeof(PauseGameOverlay)));

        gamemode.OnPauseGame();
    }

    public void ShowWaitingStartGameOverlay()
    {
        ShowScreen(GetScreenWithType(typeof(WaitingStartGameOverlay)));

        gamemode.OnResumeGame();
    }

    public void ShowGamePlayOverlay()
    {
        ShowScreen(GetScreenWithType(typeof(GamePlayOverlay)));

        gamemode.OnResumeGame();

    }

    public void ShowGameOverOverlay()
    {
        ShowScreen(GetScreenWithType(typeof(GameOverOverlay)));

    }


    #region Effects
    public void ShowEndGameOverlays()
    {   
        StartCoroutine(EffectsEndGame());

    }

    private IEnumerator EffectsEndGame()
    {
        yield return StartCoroutine(FlashCoro());
        ShowGameOverOverlay();

    }

    public IEnumerator FlashCoro()
    {
        flashEffectImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(FadeOut());
        flashEffectImage.gameObject.SetActive(false);

    }

    private IEnumerator FadeIn()
    {
        flashEffectImage.color = new Color(1, 1, 1, 0);

        float currentValueFadeIn = 0;
        float fadeInTargetvalue = 1;
        float durationFadeIn = 0.05f;

        DOTween.To(()=> currentValueFadeIn, x => currentValueFadeIn = x, fadeInTargetvalue, durationFadeIn);

        while(flashEffectImage.color.a != fadeInTargetvalue)
        {
            flashEffectImage.color = new Color(0, 0, 0, currentValueFadeIn);
            yield return null;
        }

    }

    private IEnumerator FadeOut()
    {
        flashEffectImage.color = new Color(0, 0, 0, 1);

        float currentValueFadeOut = 1;
        float fadeOutTargetvalue = 0;
        float durationFadeOut = 0.05f;

        DOTween.To(()=> currentValueFadeOut, x => currentValueFadeOut = x, fadeOutTargetvalue, durationFadeOut);

        while(flashEffectImage.color.a != fadeOutTargetvalue)
        {
            flashEffectImage.color = new Color(0, 0, 0, currentValueFadeOut);
            yield return null;
        }

    }
    #endregion
    
}
