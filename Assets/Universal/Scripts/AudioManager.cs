using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // The audio clips that we need to play
    public AudioClip player_attack;
    public AudioClip player_jump;
    public AudioClip player_hurt;
    public AudioClip monster_attack;
    public AudioClip monster_hurt;
    public AudioClip monster_dead;
    // An array of audio sources
    List<AudioSource> audioSources = new List<AudioSource>();
    void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }
    }
    public void playAudioClip(int index, string clipName, bool loop)
    {
        AudioClip clip = getAudioClip(clipName);
        if(clip != null) {
            AudioSource audio = audioSources[index];
            audio.clip = clip;
            audio.loop = loop;
            audio.Play(); //* built-in method
        }
    }
    AudioClip getAudioClip(string clipName)
    {
        switch (clipName)
        {
            // Clips for player
            case "player_attack":
                return player_attack;
            case "player_jump":
                return player_jump;
            case "player_hurt":
                return player_hurt;
            // Clips for monster
            case "monster_attack":
                return monster_attack;
            case "monster_hurt":
                return monster_hurt;
            case "monster_dead":
                return monster_dead;
        }
        return null;
    }
}
