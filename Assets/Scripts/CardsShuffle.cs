using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsShuffle : MonoBehaviour
{

    void Start()
    {
        ShuffleCards();
    }

    void ShuffleCards()
    {
        int count = gameObject.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, count);
            gameObject.transform.GetChild(randomIndex).SetSiblingIndex(i);
        }
    }
}
