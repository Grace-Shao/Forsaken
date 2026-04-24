using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;

    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    #region Private Fields
    // The SpriteRenderer that should flash.
    private SpriteRenderer[] spriteRenderers;
    // The material that was in use, when the script started.
    private Material[] originalMaterials;
    // The currently running coroutine.
    private Coroutine flashRoutine;
    #endregion

    void Awake()
    {
        // Get the SpriteRenderer to be used,
        // alternatively you could set it from the inspector.
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        // Get the material that the SpriteRenderer uses, 
        // so we can switch back to it after the flash ended.
        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        originalMaterials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            originalMaterials[i] = spriteRenderers[i].material;
        }
    }

    public void BeginFlash()
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }

        // Start the Coroutine, and store the reference for it.
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material = flashMaterial;
        }

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].material = originalMaterials[i];
        }

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
