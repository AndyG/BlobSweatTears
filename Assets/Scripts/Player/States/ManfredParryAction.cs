using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManfredParryAction : ManfredStates.ManfredState2Params<GameObject, Vector3>
{

  private float duration = 0.3f;

  private float timeInState = 0f;

  private GameObject spawnObject;
  private Vector3 parryPosition;
  private bool hasSpawnedObject = false;

  public override void Enter(GameObject spawnObject, Vector3 parryPosition)
  {
    timeInState = 0f;
    this.spawnObject = spawnObject;
    this.parryPosition = parryPosition;
    this.hasSpawnedObject = false;
  }

  public override void Tick()
  {
    if (!hasSpawnedObject)
    {
      SpawnObject();
    }
    timeInState += Time.deltaTime;
    if (timeInState >= duration)
    {
      manfred.fsm.ChangeState(manfred.stateGrounded, manfred.stateGrounded);
    }
  }

  public override string GetAnimation()
  {
    return "ManfredParryAction";
  }

  private void SpawnObject()
  {
    GameObject gameObject = GameObject.Instantiate(spawnObject, parryPosition, Quaternion.identity);
    Directable directable = gameObject.GetComponent<Directable>();

    if (directable != null) {
      directable.DirectToward(GetInputDirection());
    }
    hasSpawnedObject = true;
  }

  private Vector2 GetInputDirection() {
    float rawHorizontalInput = manfred.playerInput.GetHorizInputRaw();
    float rawVerticalInput = manfred.playerInput.GetVerticalInputRaw();
    Vector2 direction = new Vector2(rawHorizontalInput, rawVerticalInput).normalized;
    return direction;
  }
}
