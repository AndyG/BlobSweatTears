using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICard : MonoBehaviour
{
 
    [SerializeField]
    private Image image;
    [SerializeField]
    private Image percentageBar;

    [SerializeField]
    private Card card;

    [SerializeField]
    private int percentage = 0;

    public void SetData(Card card, int percentage) {
        this.card = card;
        this.percentage = percentage;
        Render();
    }

    void Render() {
        if (card != null) {
            RenderCard(card);
        } else if (percentage > 0) {
            RenderPercentage(percentage);
        } else {
            RenderNone();
        }
    }

    void RenderCard(Card card) {
        image.sprite = card.cardArt;
        image.enabled = true;
        percentageBar.enabled = false; 
    }

    void RenderPercentage(int percentage) {
        image.enabled = false;
        percentageBar.enabled = true; 
        percentageBar.transform.localScale = new Vector3(
            percentageBar.transform.localScale.x,
            percentage / 100f,
            percentageBar.transform.localScale.z
        );
    }

    void RenderNone() {
        image.enabled = false;
        percentageBar.enabled = false;
    }
}
