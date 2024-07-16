using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Create a Singleton that can be globally accessible and doesn't require a reference in the project
    public static GameManager Instance; 
    // Other references
    public AudioManager audioManager;
    public UI UI;
    private void Awake() {
        if (Instance != null && Instance != this) Destroy(Instance);
        else Instance = this;
    }
}
