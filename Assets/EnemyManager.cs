using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyManager : MonoBehaviour
{
  public bool isBossUnlocked = false;

  int countTrashEnemies;

  private Listener listener;

  [SerializeField]
  private AudioClip enemyDeathClip;

  private AudioSource audioSource;

  void Start() {
    this.audioSource = GetComponent<AudioSource>();
  }

  public void SetListener(Listener listener)
  {
    this.listener = listener;
  }

  public void IncrementTrashEnemies()
  {
    this.countTrashEnemies++;
  }

  public void DecrementTrashEnemies()
  {
    StartCoroutine(DelayedDecrement());
  }

  public bool IsBossUnlocked()
  {
    return isBossUnlocked;
  }

  private void DoBossUnlock()
  {
    isBossUnlocked = true;
    listener.OnBossUnlocked();
  }

  private IEnumerator DelayedDecrement() {
    yield return new WaitForSeconds(1.5f);
    audioSource.PlayOneShot(enemyDeathClip);
    this.countTrashEnemies--;
    if (countTrashEnemies == 0)
    {
      DoBossUnlock();
    }
  }

  public interface Listener
  {
    void OnBossUnlocked();
  }
}
