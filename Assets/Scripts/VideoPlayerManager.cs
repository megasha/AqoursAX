using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class VideoPlayerManager : MonoBehaviour {

    //Audio
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    public Text playText;
    public Text timeText;

    private bool vidPrepared = false;

    void Awake()
    {
        //Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start () {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        StartCoroutine(playVideo());
	}

    // Update is called once per frame
    void Update() {
        if (!vidPrepared)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Pause Video
            if (videoPlayer.isPlaying)
                videoPlayer.Pause();
            //Play Video
            else
                videoPlayer.Play();
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            videoPlayer.time -= 0.30f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            videoPlayer.time += 0.30f;
        }

        //UI Text updater
        if (videoPlayer.isPlaying)
            playText.text = "Video: Playing";
        else
            playText.text = "Video: Paused";

        timeText.text = "Time: " + Math.Round(videoPlayer.time,2) + " s";
	}

    IEnumerator playVideo()
    {
        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        //Set video to Play then prepare Audio to prevent Buffering
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }
        
        Debug.Log("Done Preparing Video");

        //Play Video
        //videoPlayer.Play();

        //Play Sound
        //audioSource.Play();

        vidPrepared = true;
        Debug.Log("Exiting playVideo()");
    }
}
