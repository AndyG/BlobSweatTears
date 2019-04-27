using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class CrumblingTile : MonoBehaviour
{

  private Rigidbody2D rb;
  private SpriteRenderer spriteRenderer;
  private CharacterDetector characterDetector;

  [Header("Tile Healths")]
  [SerializeField]
  private Sprite spriteHealth5;
  [SerializeField]
  private Sprite spriteHealth4;
  [SerializeField]
  private Sprite spriteHealth3;
  [SerializeField]
  private Sprite spriteHealth2;
  [SerializeField]
  private Sprite spriteHealth1;

  void Start()
  {
    this.rb = GetComponent<Rigidbody2D>();
    this.spriteRenderer = GetComponent<SpriteRenderer>();
    this.characterDetector = GetComponentInChildren<CharacterDetector>();
  }

  public void NotifyHealth(int health)
  {
    if (health >= 5)
    {
      spriteRenderer.sprite = spriteHealth5;
    }
    else if (health == 4)
    {
      spriteRenderer.sprite = spriteHealth4;
    }
    else if (health == 3)
    {
      spriteRenderer.sprite = spriteHealth3;
    }
    else if (health == 2)
    {
      spriteRenderer.sprite = spriteHealth2;
    }
    else if (health == 1)
    {
      spriteRenderer.sprite = spriteHealth1;
    }
  }

  public bool IsCharacterDetected() => characterDetector.isPlayerDetected;
}
