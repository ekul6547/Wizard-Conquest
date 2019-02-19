using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroToLoop : MonoBehaviour {

    public AudioSource Intro;
    public AudioSource Loop;

	void Start () {
        Invoke("PlayLoop", Intro.clip.length-Time.deltaTime);
	}

    void PlayLoop()
    {
        Loop.Play();
    }
}
