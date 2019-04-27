using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDetector : MonoBehaviour
{
    public bool isPlayerDetected = false;

    private HashSet<Listener> listeners = new HashSet<Listener>();

    public void AddListener(Listener listener) {
        listeners.Add(listener);
    }

    public void RemoveListener(Listener listener) {
        listeners.Remove(listener);
    }

    void OnTriggerEnter2D(Collider2D other) {
        isPlayerDetected = true;
        foreach (Listener listener in listeners) {
            listener.OnPlayerEntered();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        isPlayerDetected = false;
        foreach (Listener listener in listeners) {
            listener.OnPlayerExited();
        }
    }

    public interface Listener {
        void OnPlayerEntered(); 
        void OnPlayerExited();
    }
}
