using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]

public class PlanteCollectible : MonoBehaviour, IPointerClickHandler
{
    public GameObject collectVFX;
    public AudioClip collectSound;
    public bool isCollected = false;

   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCollected) return;
        Collect();
    }

    void Collect()
    {
        isCollected = true;

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        RandomTextDisplayer.instance.ShowRandomText();
        GameManager.Instance?.PlantCollect();

        Destroy(gameObject); 
    }
}
