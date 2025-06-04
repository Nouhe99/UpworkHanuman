using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay Settings")]
    public GameObject gameContent;
    public int totalPlants = 10;
    private int plantsCollected = 0;
    public TextMeshProUGUI plantTextUI;

    [Header("Timer Settings")]
    public float timeLeft = 120f;
    public TextMeshProUGUI timerText;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject distractionUI;
    public GameObject gameUI;

    [Header("Distractions")]
    public Transform distractionsContainer;
    private Coroutine spawnRoutine;
    private bool isSpawning = false;

    private bool isGameOver = false;
    private bool isGameStarted = false;

    void Awake()
    {
        // Setup singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Start with game paused
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (isGameOver || !isGameStarted) return;

        // Countdown timer
        timeLeft -= Time.deltaTime;
        UpdateTimerUI();

        // Check for win or lose conditions
        CheckWin();
        CheckLose();

        // Manage distraction spawning logic
        HandleDistractionSpawning();
    }

    // Start the game
    public void StartGame()
    {
        isGameStarted = true;
        Time.timeScale = 1f;
    }

    // Update the timer display
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Called when a plant is collected
    public void PlantCollect()
    {
        plantsCollected++;

        if (plantTextUI != null)
            plantTextUI.text = $"{plantsCollected}/{totalPlants}";

        if (plantsCollected >= totalPlants)
        {
            CheckWin();
        }
    }

    // Return current number of collected plants
    public int GetPlantsCount()
    {
        return plantsCollected;
    }

    // Called when player clicks a distraction
    public void DistractionClicked()
    {
        gameContent.SetActive(false);
        Time.timeScale = 0f;
        distractionUI.SetActive(true);
        timeLeft -= 5;
    }

    // Check if any distractions are currently active
    public bool AnyDistractionActive()
    {
        foreach (Transform child in distractionsContainer)
        {
            if (child.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    // Stop and deactivate all distractions
    void DeactivateAllDistractions()
    {
        foreach (Transform child in distractionsContainer)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Handle spawning of distractions over time
    void HandleDistractionSpawning()
    {
        if (!isSpawning && gameContent.activeInHierarchy && timeLeft > 1f)
        {
            spawnRoutine = StartCoroutine(SpawnDistractions());
            isSpawning = true;
        }
        else if (!gameContent.activeInHierarchy && isSpawning)
        {
            if (spawnRoutine != null)
                StopCoroutine(spawnRoutine);

            isSpawning = false;
            DeactivateAllDistractions();
        }
    }

    // Coroutine to spawn distractions periodically
    IEnumerator SpawnDistractions()
    {
        while (true)
        {
            // Wait a random time before showing the next distraction
            float waitTime = Random.Range(6f, 12f);
            yield return new WaitForSeconds(waitTime);

            if (AnyDistractionActive()) continue;

            int count = distractionsContainer.childCount;
            if (count == 0) yield break;

            int randomIndex = Random.Range(0, count);
            Transform distraction = distractionsContainer.GetChild(randomIndex);
            distraction.gameObject.SetActive(true);

            // Show distraction for 6 seconds
            yield return new WaitForSeconds(6f);
            distraction.gameObject.SetActive(false);
        }
    }

    // Check if the player has won
    void CheckWin()
    {
        if (!isGameOver && timeLeft > 0f && plantsCollected >= totalPlants)
        {
            isGameOver = true;
            Time.timeScale = 0f;
            gameUI.SetActive(false);
            gameContent.SetActive(false);
            winPanel.SetActive(true);
            Debug.Log("You win!");
        }
    }

    // Check if the player has lost
    void CheckLose()
    {
        if (!isGameOver && timeLeft <= 0.1f && plantsCollected < totalPlants)
        {
            isGameOver = true;
            Time.timeScale = 0f;
            gameContent.SetActive(false);
            losePanel.SetActive(true);
            Debug.Log("Time’s up! You lose.");
        }
    }

    // Restart the current level
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load next level (scene index 3)
    public void NextLevel()
    {
        SceneManager.LoadScene(3);
    }
}
