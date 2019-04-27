using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingTileGroup : MonoBehaviour
{

  private Rigidbody2D rb;
  private CharacterDetector characterDetector;

  [SerializeField]
  private float degradationInterval;
  [SerializeField]
  private int health;

  private float currentDegradationTime;

  private CrumblingTile[] crumblingTiles;

  void Start()
  {
    this.rb = GetComponent<Rigidbody2D>();
    this.characterDetector = GetComponent<CharacterDetector>();
    crumblingTiles = GetComponentsInChildren<CrumblingTile>();
    NotifyHealth();
  }

  // Update is called once per frame
  void Update()
  {
    bool isCharacterDetected = false;
    for (int i = 0; i < crumblingTiles.Length; i++)
    {
      CrumblingTile tile = crumblingTiles[i];
      isCharacterDetected = tile.IsCharacterDetected();
      if (isCharacterDetected)
      {
        break;
      }
    }

    if (!isCharacterDetected)
    {
      currentDegradationTime = 0f;
      return;
    }

    currentDegradationTime += Time.deltaTime;

    // character is detected
    if (currentDegradationTime > degradationInterval)
    {
      currentDegradationTime = 0f;
      Degrade();
    }
  }

  private void Degrade()
  {
    health--;
    NotifyHealth();

    if (health <= 0f)
    {
      GameObject.Destroy(this.transform.gameObject);
    }
  }

  private void NotifyHealth()
  {
    for (int i = 0; i < crumblingTiles.Length; i++)
    {
      CrumblingTile tile = crumblingTiles[i];
      tile.NotifyHealth(health);
    }
  }
}
