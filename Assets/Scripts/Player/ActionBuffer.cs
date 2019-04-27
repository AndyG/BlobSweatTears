using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBuffer
{
  private Dictionary<string, BufferedAction> actions = new Dictionary<string, BufferedAction>();

  public void Buffer(string action, float timeToBuffer)
  {
    actions[action] = new BufferedAction(timeToBuffer);
  }

  public void Update()
  {
    foreach (KeyValuePair<string, BufferedAction> action in actions)
    {
      action.Value.timeRemaining -= Time.deltaTime;
    }
  }

  // Returns true if the action was buffered and consumes the action.
  public bool ConsumeBuffer(string action)
  {
    bool wasBuffered = actions.ContainsKey(action) && actions[action].timeRemaining > 0f;
    ClearAction(action);
    return wasBuffered;
  }

  public void ClearAction(string action)
  {
    actions.Remove(action);
  }

  private class BufferedAction
  {
    public float timeRemaining;

    public BufferedAction(float timeRemaining)
    {
      this.timeRemaining = timeRemaining;
    }
  }
}
