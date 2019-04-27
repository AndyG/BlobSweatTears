using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardMapper", menuName = "Cards/CardMapper")]
public class CardTypeMapper : ScriptableObject
{
  public Card deathCard;
  public Card loverCard;
  public Card judgmentCard;

  public Card GetCard(CardType cardType)
  {
    switch (cardType)
    {
      case CardType.DEATH: return deathCard;
      case CardType.LOVER: return loverCard;
      case CardType.JUDGMENT: return judgmentCard;
    }

    return null;
  }
}
