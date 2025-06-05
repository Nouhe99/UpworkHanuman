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


    /// Called when an image target changes tracking status.

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


    /// Show the associated canvas and pause scanning.

    public void ShowCanvas()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isCanvasVisible = true;
        isScanningEnabled = false;
        ToggleImageTargets(false); // Disable scanning while canvas is shown
    }


    /// Called when the canvas is closed.

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


    /// Enable or disable all image target tracking.

    private void ToggleImageTargets(bool isEnabled)
    {
        foreach (var target in imageTargets)
        {
            target.enabled = isEnabled;
        }
    }


    /// Called once all image targets have been scanned.

    private void OnAllTargetsScanned()
    {
        Debug.Log("✅ All 4 image targets have been scanned at least once!");
        scannedResult.SetActive(true);
    }


    /// Loads the next scene (example: scene 2).

    public void NextGame()
    {
        SceneManager.LoadScene(2);
    }
}
