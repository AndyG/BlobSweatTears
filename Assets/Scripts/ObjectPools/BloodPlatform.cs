using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class BloodPlatform : MonoBehaviour
{

    enum State {
        COMPACT,
        EXPANDING,
        EXPANDED
    }

    [Header("PlayerDetection")]
    [SerializeField]
    private PlayerChecker playerChecker;

    [Header("State")]
    private State state;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip spawnAudioClip;

    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rb2d;
    private AudioSource _audioSource;

    void Start() {
        this.state = State.COMPACT;
        this._animator = GetComponent<Animator>();
        this._collider = GetComponent<BoxCollider2D>();
        this._rb2d = GetComponent<Rigidbody2D>();
        this._collider.enabled = false;

        this._audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(spawnAudioClip);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.COMPACT) {
            CheckForPlayer();
        }
    }

    public void OnExpanded() {
        this.state = State.EXPANDED;
        this._collider.enabled = true;
        string animation = "BloodPlatformExpanded";
        _animator.Play(animation);
    }

    private void CheckForPlayer() {
        if (!playerChecker.IsPlayerPresent()) {
            this.state = State.EXPANDING;
            string animation = "BloodPlatformExpand";
            _animator.Play(animation);
            Debug.Log("expand");
        }
    }
}
