using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "UI/Player Bar Data")]
public class PlayerBarData : ScriptableObject
{
    [CreateProperty]
    public float health = 100f;

    [CreateProperty]
    public float mana = 100f;

    [CreateProperty]
    public float xp = 0f;
}
