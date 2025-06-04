using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    public int cardID;
    public GameObject front;
    public GameObject back;

    private bool isFlipped = false;
    private bool isMatched = false;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickCard);
        ShowBack();
    }

    private void OnClickCard()
    {
        Debug.Log("click");

        if (isFlipped || isMatched || CardsManager.Instance.IsBusy)
            return;

        FlipCard();
        CardsManager.Instance.PlayFlipSound();
        CardsManager.Instance.CardFlipped(this);
    }

    public void FlipCard()
    {
        isFlipped = true;
        front.SetActive(true);
        back.SetActive(false);
    }

    public void FlipBack()
    {
        isFlipped = false;
        front.SetActive(false);
        back.SetActive(true);
    }

    public void SetMatched()
    {
        isMatched = true;
        GetComponent<Button>().interactable = false;
    }

    public void ShowBack()
    {
        isFlipped = false;
        front.SetActive(false);
        back.SetActive(true);
    }

    public bool IsMatched => isMatched;
    public int CardID => cardID;
}
