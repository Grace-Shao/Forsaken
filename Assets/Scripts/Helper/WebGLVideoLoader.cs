using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoLoader : MonoBehaviour
{
    [SerializeField] private string videoFileName;

    void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        string videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = videoPath;
        Debug.Log("Loading video from: " + videoPath);
        videoPlayer.Prepare();
        videoPlayer.playOnAwake = false;
    }
}
