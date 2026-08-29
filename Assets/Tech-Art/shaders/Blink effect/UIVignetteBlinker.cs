using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class UIVignetteBlinker : MonoBehaviour
{
    static readonly int BlinkAmountID   = Shader.PropertyToID("_BlinkAmount");
    static readonly int VignetteColorID = Shader.PropertyToID("_VignetteColor");

    [Range(0f, 1f)] public float blinkAmount;
    [ColorUsage(true, true)] public Color vignetteColor = new Color(0.6f, 0f, 0f, 1f);

    Image _image;

    void OnEnable() => _image = GetComponent<Image>();

    void LateUpdate() => Apply();

    void OnValidate()
    {
        _image = GetComponent<Image>();
        Apply();
    }

    void Apply()
    {
        if (_image == null || _image.material == null) return;
        _image.material.SetFloat(BlinkAmountID, blinkAmount);
        _image.material.SetColor(VignetteColorID, vignetteColor);
    }
}