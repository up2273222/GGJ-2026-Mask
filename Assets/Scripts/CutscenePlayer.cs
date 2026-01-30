using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
   private VideoPlayer videoPlayer;
   private bool videoStarted;

   private void Awake()
   {
      videoPlayer = GetComponent<VideoPlayer>();
   }

   private void Start()
   {
      videoPlayer.loopPointReached += OnVideoEnd;
   }

   void OnVideoEnd(VideoPlayer vp)
   {
      SceneManager.LoadScene("SamuelScene");
   }
}
