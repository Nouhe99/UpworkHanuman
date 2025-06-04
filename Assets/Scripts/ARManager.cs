using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class ARManager : MonoBehaviour
{
    public static ARManager Instance;

    [Header("AR Tracking")]
    public List<ObserverBehaviour> imageTargets; // Assign your image targets here
    public bool isScanningEnabled = true;
    public bool allScanned = false;

    [Header("UI")]
    public GameObject scannedResult;

    private HashSet<string> scannedTargetNames = new HashSet<string>();
    private bool isCanvasVisible = false;

    void Start()
    {
        // Setup singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Subscribe to each image target's status change event
        foreach (var target in imageTargets)
        {
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    /// <summary>
    /// Called when an image target changes tracking status.
    /// </summary>
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (!isScanningEnabled) return;

        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            Debug.Log($"Target status: {behaviour.TargetName} - {status.Status}");
            string targetName = behaviour.TargetName;

            if (!scannedTargetNames.Contains(targetName))
            {
                scannedTargetNames.Add(targetName);
                Debug.Log($"Scanned for the first time: {targetName}");

                if (!allScanned && scannedTargetNames.Count == imageTargets.Count)
                {
                    allScanned = true;
                    // All targets scanned; optional call here
                }
            }
        }
    }

    /// <summary>
    /// Show the associated canvas and pause scanning.
    /// </summary>
    public void ShowCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isCanvasVisible = true;
        isScanningEnabled = false;
        ToggleImageTargets(false); // Disable scanning while canvas is shown
    }

    /// <summary>
    /// Called when the canvas is closed.
    /// </summary>
    public void OnCanvasClosed()
    {
        isCanvasVisible = false;
        ToggleImageTargets(true);
        isScanningEnabled = true;

        if (allScanned)
        {
            OnAllTargetsScanned();
        }
    }

    /// <summary>
    /// Enable or disable all image target tracking.
    /// </summary>
    private void ToggleImageTargets(bool isEnabled)
    {
        foreach (var target in imageTargets)
        {
            target.enabled = isEnabled;
        }
    }

    /// <summary>
    /// Called once all image targets have been scanned.
    /// </summary>
    private void OnAllTargetsScanned()
    {
        Debug.Log("✅ All 4 image targets have been scanned at least once!");
        scannedResult.SetActive(true);
    }

    /// <summary>
    /// Loads the next scene (example: scene 2).
    /// </summary>
    public void NextGame()
    {
        SceneManager.LoadScene(2);
    }
}
