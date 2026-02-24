using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBars", menuName = "ScriptableObjects/PlayerBars", order = 1)]
public class PlayerBarsSO : ScriptableObject
{
    public float health = 100f;
    public float mana = 100f;
    public float xp = 0f;
}