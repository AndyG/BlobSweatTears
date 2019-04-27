using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
  [SerializeField]
  [Range(0f, 2f)]
  private float timeScale = 1f;

  [SerializeField]
  private float MAX_PAUSE_TIME = 0.3f;

  private bool isDoingDramaticPause = false;
  private float timeScaleSnapshot;

  void Awake()
  {
    Time.timeScale = timeScale;
  }

  public void SetTimeScale(float timeScale)
  {
    this.timeScale = timeScale;
    Time.timeScale = timeScale;
  }

  public void DoDramaticPause(float pauseTime)
  {
    if (isDoingDramaticPause)
    {
      StopAllCoroutines();
      StartCoroutine(DramaticPauseCoroutine(pauseTime, timeScaleSnapshot));
    }
    else
    {
      StartCoroutine(DramaticPauseCoroutine(pauseTime, timeScale));
    }
  }

  private IEnumerator DramaticPauseCoroutine(float pauseTime, float origTimeScale)
  {
    isDoingDramaticPause = true;
    timeScaleSnapshot = origTimeScale;
    SetTimeScale(0f);
    float resolvedPauseTime = Mathf.Min(pauseTime / origTimeScale, MAX_PAUSE_TIME);
    yield return new WaitForSecondsRealtime(resolvedPauseTime);
    SetTimeScale(timeScaleSnapshot);
    isDoingDramaticPause = false;
  }

  void OnValidate()
  {
    Time.timeScale = timeScale;
    timeScaleSnapshot = timeScale;
  }
}
