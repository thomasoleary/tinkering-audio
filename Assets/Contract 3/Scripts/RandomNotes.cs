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

Code editited from https://www.youtube.com/watch?v=GqHFGMy_51c
*/

public class RandomNotes : MonoBehaviour {
    private double frequency = 440.0;
    private double increment;
    private double phase;
    private double samplingFrequency = 48000.0;
    public float SampleDuration = 0f;
    public AudioSource AudioSource;
    private AudioClip AudioClip;
    private int counter = 0;
    private int FileNameCounter = 0;
    public int position = 0;


    private float gain;
    private float volume = 0.1f;

    private float[] frequencies;
    private int ThisFreq;
    //private bool pressed = false;


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

        AudioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(DelaySound());

        
    }
    // used for debugging
    /*void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //starting the delaySound Coroutine
            
        }
    }*/

    //IEnumerator used for waiting time
    IEnumerator DelaySound() {
        float SampleDurationLength = SampleDuration;
        while (counter <= SampleDuration) {
            // Waiting a random range for the duration of note playing
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));

            frequency = frequencies[ThisFreq];
            // Plays frequencies randomly between length of frequencies
            ThisFreq += (Random.Range(0, frequencies.Length));
            //Making sure no end, and loops frequencies
            ThisFreq %= frequencies.Length;
            counter++;

            FileNameCounter++;
            GenerateClip();
            SaveToWav();

        }
        
    }

    //This function allows player to hear sounds, without it, no audio will play
    public void OnAudioFilterRead(float[] data, int channels) {
        gain = volume;
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;
        // Looping around the data array 
        for (int i = 0; i < data.Length; i += channels) {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));
            // Plays in both speaker channels
            if (channels == 2) {
                data[i + 1] = data[i];
            }
            if (phase > (Mathf.PI * 2)) {
                phase = 0.0;
            }
        }
    }

    // generating a audioClip between random range 
    void GenerateClip() {
        float randomNoteLen = Random.Range(0.2f, 0.5f); 
        AudioClip = AudioClip.Create("tone", (int)(samplingFrequency *  randomNoteLen), 2, (int)samplingFrequency, false, OnAudioRead, OnAudioSetPosition);
    }

    //function has been made to work with saving functions 
    void OnAudioRead(float[] data)
    {
        gain = volume;
        increment = frequency * 2.0 * Mathf.PI / samplingFrequency;
        // Looping around the data array 
        for (int i = 0; i < data.Length; i += 2)
        {
            phase += increment;
            data[i] = (float)(gain * Mathf.Sin((float)phase));
            // Plays in both speaker channels
             data[i + 1] = data[i];
            if (phase > (Mathf.PI * 2))
            {
                phase = 0.0;
            }
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }

    public void SaveToWav() {
        if(FileNameCounter <= counter) {
            string str = "Example" + FileNameCounter; 
            SaveWavUtil.Save(str, AudioClip);
        }
        
    }

    
}
