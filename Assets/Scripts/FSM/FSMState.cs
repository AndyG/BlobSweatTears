using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FSMState<C>
{
  void Tick();
  void Exit();
  void BindContext(C context);
}