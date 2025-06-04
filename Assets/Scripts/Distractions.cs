using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Distractions : MonoBehaviour, IPointerClickHandler
{
    public GameObject distractVFX;
    public AudioClip distractSound;
    public bool isDistracted = false;

    void OnEnable()
    {
        isDistracted = false;    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDistracted) return;
        Distract();
    }

    void Distract()
    {
        isDistracted = true;

        if (distractSound != null)
            AudioSource.PlayClipAtPoint(distractSound, transform.position);

        GameManager.Instance?.DistractionClicked();
        RandomTextDisplayer.instance.gameObject.SetActive(false);
    }
   
}
