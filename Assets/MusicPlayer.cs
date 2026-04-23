using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private string initMusicName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioControl.Instance.PlayMusic(initMusicName);
    }
}
