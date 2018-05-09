using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerManager : MonoBehaviour {

    //Audio
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    public Text text;

    private bool vidPrepared = false;

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

        //UI Text updater
        if (videoPlayer.isPlaying)
            text.text = "Video: Playing";
        else
            text.text = "Video: Paused";
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
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        vidPrepared = true;
        Debug.Log("Exiting playVideo()");
    }
}
