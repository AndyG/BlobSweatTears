using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[CreateAssetMenu(fileName = "Input", menuName = "PlayerInput", order = 1)]
public class PlayerInput : ScriptableObject
{

  [SerializeField]
  private int playerId;

  [SerializeField]
  private float horizontalDeadzone = 0.02f;

  [SerializeField]
  private float verticalDeadzone = 0.02f;

  [SerializeField]
  private float jumpBufferTime;

  private Player player;

  private float horizInputRaw;
  private float verticalInputRaw;
  private float horizInput;
  private float verticalInput;
  private bool didPressJump;
  private bool didReleaseJump;
  private bool didPressAttack;
  private bool didPressParry;
  private bool didPressSiphon;
  private bool isHoldingSiphon;

  private ActionBuffer actionBuffer;

  public void Awake()
  {
    player = ReInput.players.GetPlayer(playerId);
    actionBuffer = new ActionBuffer();
  }

  public void Update()
  {
    actionBuffer.Update();
    Clear();
    horizInput = _GetHorizInput();
    verticalInput = _GetVerticalInput();
    didPressJump = _GetDidPressJump();
    didReleaseJump = _GetDidReleaseJump();
    didPressAttack = _GetDidPressAttack();
    didPressParry = _GetDidPressParry();
    didPressSiphon = _GetDidPressSiphon();
    isHoldingSiphon = _GetIsHoldingSiphon();

    if (didPressJump)
    {
      actionBuffer.Buffer(ACTION_JUMP, jumpBufferTime);
    }
    else if (didReleaseJump)
    {
      actionBuffer.ClearAction(ACTION_JUMP);
    }
  }

  private void Clear()
  {
    horizInput = 0f;
    verticalInput = 0f;
    didPressJump = false;
    didReleaseJump = false;
    didPressAttack = false;
    didPressParry = false;
    didPressSiphon = false;
    isHoldingSiphon = false;
    horizInputRaw = 0f;
    verticalInputRaw = 0f;
  }

  #region Getters
  public float GetHorizInputRaw() => horizInputRaw;
  public float GetVerticalInputRaw() => verticalInputRaw;
  public float GetHorizInput() => horizInput;
  public float GetVerticalInput() => verticalInput;
  public bool GetDidPressJump() => didPressJump;
  public bool GetDidPressJumpBuffered() => actionBuffer.ConsumeBuffer(ACTION_JUMP);
  public bool GetDidReleaseJump() => didReleaseJump;
  public bool GetDidPressAttack() => didPressAttack;
  public bool GetDidPressParry() => didPressParry;
  public bool GetDidPressSiphon() => didPressSiphon;
  public bool GetIsHoldingSiphon() => isHoldingSiphon;
  #endregion

  #region Get raw input

  private float _GetHorizInput()
  {
    horizInputRaw = player.GetAxis("MoveHorizontal");
    if (Mathf.Abs(horizInputRaw) < horizontalDeadzone)
    {
      return 0;
    }
    else
    {
      return Mathf.Sign(horizInputRaw);
    }
  }

  private float _GetVerticalInput()
  {
    verticalInputRaw = player.GetAxis("MoveVertical");
    if (Mathf.Abs(verticalInputRaw) < verticalDeadzone)
    {
      return 0;
    }
    else
    {
      return Mathf.Sign(verticalInputRaw);
    }
  }

  private bool _GetDidPressJump() => player.GetButtonDown("Jump");
  private bool _GetDidReleaseJump() => player.GetButtonUp("Jump");
  private bool _GetDidPressAttack() => player.GetButtonDown("PrimaryAttack");
  private bool _GetDidPressParry() => player.GetButtonDown("Parry");
  private bool _GetDidPressSiphon() => player.GetButtonDown("Siphon");
  private bool _GetIsHoldingSiphon() => player.GetButton("Siphon");
  #endregion

  #region Buffer keys
  private static string ACTION_JUMP = "Jump";
  #endregion
}
