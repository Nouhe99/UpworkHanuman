using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float timeRemaining = 120f;
    private bool timerRunning = true;

    [Header("Audio")]
    public AudioClip flipSound;
    public AudioClip matchSound;
    private AudioSource audioSource;

    [Header("UI")]
    public GameObject gameContent;
    public GameObject defeatPanel;
    public GameObject victoryPanel;

    private Cards firstCard;
    private Cards secondCard;
    public bool IsBusy { get; private set; } = false;
    private int matchedPairs = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (timeRemaining <= 0)
        {
            timerRunning = false;
            gameContent.SetActive(false);
            defeatPanel.SetActive(true);

            // Hide random text displayer on defeat
            if (RandomTextDisplayer.instance != null)
                RandomTextDisplayer.instance.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Plays sound when a card is flipped.
    /// </summary>
    public void PlayFlipSound()
    {
        if (flipSound != null)
            audioSource.PlayOneShot(flipSound);
    }

    /// <summary>
    /// Plays sound when cards match and shows random text.
    /// </summary>
    public void PlayMatchSound()
    {
        if (matchSound != null)
            audioSource.PlayOneShot(matchSound);

        if (RandomTextDisplayer.instance != null)
            RandomTextDisplayer.instance.ShowRandomText();
    }

    /// <summary>
    /// Called when a card is flipped.
    /// </summary>
    public void CardFlipped(Cards card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    /// <summary>
    /// Checks if the two flipped cards match.
    /// </summary>
    private IEnumerator CheckMatch()
    {
        IsBusy = true;
        yield return new WaitForSeconds(0.5f);

        if (firstCard.CardID == secondCard.CardID)
        {
            firstCard.SetMatched();
            secondCard.SetMatched();
            PlayMatchSound();

            matchedPairs++;

            if (matchedPairs >= 6)
            {
                timerRunning = false;
                gameContent.SetActive(false);
                victoryPanel.SetActive(true);

                if (RandomTextDisplayer.instance != null)
                    RandomTextDisplayer.instance.gameObject.SetActive(false);
            }
        }
        else
        {
            firstCard.FlipBack();
            secondCard.FlipBack();
        }

        firstCard = null;
        secondCard = null;
        IsBusy = false;
    }

    /// <summary>
    /// Restart only the card-matching scene.
    /// </summary>
    public void RestartCardsGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Restart the full game from scene 0.
    /// </summary>
    public void RestartFullGame()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quit the application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
