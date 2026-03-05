using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public void PlayOneShotSound(EventReference sound, Vector2 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
