﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Contract 3 - Melody Generation (Non-Diegetic Audio)

Link to GitHub repository: https://github.com/atdeJimmyG/Tinkering-Audio-Team-3
Code and project authored by James Gill under the GPL-3.0 license

Copyright (C) <2019>  <James Gill>
The full licence is found at https://www.gnu.org/licenses.

This scripts is capable of creating a random order of tones, from the C Major scale

*/

public class RandomNotes : MonoBehaviour {
    public double frequency = 440.0;
    private double increment;
    private double phase;
    private double sampling_frequency = 48000.0;

    public float gain;
    public float volume = 0.1f;

    public float[] frequencies;
    public int thisFreq;
    public bool pressed = false;


    void Start() {
        //Setting array to all frequencies in C Major Scale
        frequencies = new float[8];
        frequencies[0] = 261.6f;
        frequencies[1] = 293.7f;
        frequencies[2] = 329.6f;
        frequencies[3] = 349.2f;
        frequencies[4] = 392.0f;
        frequencies[5] = 440.0f;
        frequencies[6] = 493.9f;
        frequencies[7] = 523.3f;

        //starting the delaySound Coroutine
        StartCoroutine(delaySound());
    }
    // Function Used for debugging
    /*void Update() {


        if (Input.GetKeyDown(KeyCode.Space)) {
            gain = volume;
            frequency = frequencies[thisFreq];
            thisFreq += 1;
            thisFreq = thisFreq % frequencies.Length;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
        gain = 0;
        }
    }*/

    //IEnumerator used for waiting time
    IEnumerator delaySound() {
        // Waiting a random range for the duration of note playing
        yield return new WaitForSeconds(Random.Range(0.5f, 0.5f));
        gain = volume;

        frequency = frequencies[thisFreq];
        // Plays frequencies randomly between length of frequencies
        thisFreq += (Random.Range(0, 8));
        //Making sure no end, and loops frequencies
        thisFreq = thisFreq % frequencies.Length;

        // Looping the Coroutine
        StartCoroutine(delaySound());
    }
        
    //Removing "unused" function will cause no audio to be played
    private void OnAudioFilterRead(float[] data, int channels) {
        increment = frequency * 2.0 * Mathf.PI / sampling_frequency;
        // Looping around the data array 
        for(int i = 0; i < data.Length; i += channels) {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));
            // Plays in both speaker channels
            if(channels == 2) {
                data[i + 1] = data[i];
            }
            if(phase > (Mathf.PI * 2)) {
                phase = 0.0;
            }
        }
    }

    
}
