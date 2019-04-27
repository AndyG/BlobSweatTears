using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private UICard card1;
    [SerializeField]
    private UICard card2;

    public void SetCards(List<Card> cards, int percentTowardNextCard) {
        // assume always [0,2] cards
        if (cards.Count == 0) {
            card1.SetData(null, percentTowardNextCard);
            card2.SetData(null, 0);
        } else if (cards.Count == 1) {
            card1.SetData(cards[0], 0);
            card2.SetData(null, percentTowardNextCard);
        } else if (cards.Count == 2) {
            card1.SetData(cards[0], 0);
            card2.SetData(cards[1], 0);
        }
    }
}
