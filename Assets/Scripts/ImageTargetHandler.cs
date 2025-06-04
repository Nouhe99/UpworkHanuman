using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ImageTargetHandler : MonoBehaviour
{
    public GameObject targetCanvas; // Assign this manually in the Inspector
    private bool hasBeenScanned = false;
    private bool canvasShown = false;

    private ObserverBehaviour observer;

    private void Awake()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (canvasShown) return;

        if ((status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED) && !hasBeenScanned)
        {
            hasBeenScanned = true;
            canvasShown = true;

            Debug.Log($"✅ First scan of: {behaviour.TargetName}");

            targetCanvas.SetActive(true);

            // Optional: notify ARManager if needed
            // ARManager.Instance.RegisterScan(behaviour.TargetName);
        }
    }

    public void CloseCanvas()
    {
        targetCanvas.SetActive(false);
        canvasShown = false;

        // Re-enable tracking if needed
        observer.enabled = true;
    }
}
