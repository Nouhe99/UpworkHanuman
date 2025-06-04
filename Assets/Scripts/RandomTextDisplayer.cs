using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextDisplayer : MonoBehaviour
{
    public static RandomTextDisplayer instance;

    [Tooltip("Text component where the random string will be displayed.")]
    public TextMeshProUGUI textDisplay;

    [Tooltip("List of strings to randomly pick from.")]
    public List<string> lines = new List<string>();

    [Tooltip("Delay before hiding this GameObject automatically.")]
    public float hideDelay = 2f;

    // Keeps track of which indexes have already been shown
    private List<int> usedIndexes = new List<int>();

    void Awake()
    {
        // Setup singleton instance
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Hide the GameObject at start
        gameObject.SetActive(false);
    }

    // Public method to show a random line from the list
    public void ShowRandomText()
    {
        // If all lines have been used, reset the list
        if (usedIndexes.Count >= lines.Count)
            usedIndexes.Clear();

        int index;

        // Select a random unused index
        do
        {
            index = Random.Range(0, lines.Count);
        } while (usedIndexes.Contains(index));

        usedIndexes.Add(index); // Mark index as used

        // Display the selected line in the UI
        if (textDisplay != null && index < lines.Count)
            textDisplay.text = lines[index];

        // Activate the GameObject to show the text
        gameObject.SetActive(true);

        // Cancel previous hide coroutine if any, then start a new one
        StopAllCoroutines();
        StartCoroutine(HideAfterDelay());
    }

    // Coroutine to hide this GameObject after a delay
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        gameObject.SetActive(false);
    }
}
