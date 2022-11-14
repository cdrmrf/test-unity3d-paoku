using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceControl : MonoBehaviour {
    
    public static AudioSourceControl Instance;
    private AudioSource audioSourcePlayer;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
        audioSourcePlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void Play(string audioName) {
        AudioClip audioClip = Resources.Load<AudioClip>(audioName);
        audioSourcePlayer.PlayOneShot(audioClip);
    }
    
}
