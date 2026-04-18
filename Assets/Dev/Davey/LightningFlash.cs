using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    public Light2D lightningLight;
    public float minDelay = 8.0f;
    public float maxDelay = 16.0f;

    public float bigFlashMinIntensity = 1.5f;
    public float bigFlashMaxIntensity = 2.7f;
    public float smallFlashMinIntensity = 0.5f;
    public float smallFlashMaxIntensity = 1.2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightningLight.intensity = 0;
        StartCoroutine(LightingCycle());
    }
    IEnumerator LightingCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            yield return StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        lightningLight.intensity = Random.Range(bigFlashMinIntensity, bigFlashMaxIntensity);
        yield return new WaitForSeconds(0.05f);
        lightningLight.intensity = 0;
        yield return new WaitForSeconds(0.05f);

        lightningLight.intensity = Random.Range(smallFlashMinIntensity, smallFlashMaxIntensity);
        yield return new WaitForSeconds(0.05f);
        lightningLight.intensity = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
