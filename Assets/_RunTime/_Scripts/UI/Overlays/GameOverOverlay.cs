using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameOverOverlay : MonoBehaviour
{

    [SerializeField] private GameMode gameMode;
    [SerializeField] private GameSaver gameSaver;

    [Space]

    [Header("UI Elements")]
    [SerializeField] private GameObject windowGameOver;
    [SerializeField] private Image retryButtonImage;
    [SerializeField] private Image quitButtonImage;
    [SerializeField] private TextMeshProUGUI lastScoreTxt;
    [SerializeField] private TextMeshProUGUI bestScoreTxt;
    [SerializeField] private Image newBestScoreImage;
    [SerializeField] private Image medalImage;
    [SerializeField] private Sprite[] medalsImage;

    [Space]

    [Header("Window Elements")]
    [SerializeField] private Transform startPosition;
    [Range(0.1f, 0.5f)]
    [SerializeField] private float durationTransitionWindow = 0.25f;
    [Range(1, 3)]
    [SerializeField] private float durationLastScoreIncrementer = 0.25f;
    [Range(0.1f, 2f)]
    [SerializeField] private float durationFadeInButton = 0.25f;

    private void OnEnable()
    {
        StartCoroutine(GameOverWindowAnimationCoro());
    }

    private void OnDisable() 
    {
        windowGameOver.transform.position = startPosition.position;
    }

    private void StartAnimationWindow()
    {
        windowGameOver.transform.position = startPosition.position;
        lastScoreTxt.text = "0";
        bestScoreTxt.text = gameSaver.BestScore.ToString();
        Color colorTransparent = new Color(1, 1, 1, 0);
        retryButtonImage.color = colorTransparent;
        quitButtonImage.color = colorTransparent;
    }

    private IEnumerator GameOverWindowAnimationCoro()
    {
        StartAnimationWindow();

        yield return StartCoroutine(MovingWindowAnimationCoro());

        yield return StartCoroutine(ScoreIncrementeAnimationCoro());

        AssignMedalImage();

        yield return StartCoroutine(ButtonFadeInAndOutAnimatonCoro());

    }

    private IEnumerator MovingWindowAnimationCoro()
    {
        windowGameOver.transform.DOLocalMoveY(0, durationTransitionWindow);
        while(windowGameOver.transform.localPosition.y != 0)
        {
            yield return null;
        }
    }

    private IEnumerator ScoreIncrementeAnimationCoro()
    {
        int currentValue = 0;
        DOTween.To(()=> currentValue, x => currentValue = x, gameMode.CurrentScore, durationLastScoreIncrementer);
        while(lastScoreTxt.text != gameMode.CurrentScore.ToString())
        {
            lastScoreTxt.text = currentValue.ToString();
            
            yield return null;
        }
        newBestScoreImage.gameObject.SetActive(gameMode.IsBestScore);

    }

    private Sprite GetCurrentMedal()
    {
        return gameSaver.CurrentMedalIndex >= 0 ? medalsImage[gameSaver.CurrentMedalIndex] : null;
    }

    private void AssignMedalImage()
    {
        Sprite sprite = GetCurrentMedal();
        if (sprite != null) medalImage.sprite = sprite;
    }

    private IEnumerator ButtonFadeInAndOutAnimatonCoro()
    {
        float initialValueFadeIn = 0;
        var fadeInValue = 1;
        DOTween.To(()=> initialValueFadeIn, x => initialValueFadeIn = x, fadeInValue, durationFadeInButton);

        while(initialValueFadeIn != fadeInValue)
        {   
            retryButtonImage.color = new Color(1,1,1, initialValueFadeIn);
            quitButtonImage.color = new Color(1,1,1, initialValueFadeIn);
            yield return null;
        }

    }

}
