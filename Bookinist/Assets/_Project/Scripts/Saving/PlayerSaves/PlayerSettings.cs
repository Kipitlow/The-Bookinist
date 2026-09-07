using UnityEngine;

[System.Serializable]
public class PlayerSettings
{
    public float playerSoundMusic = 0.5f;
    public float playerSoundEffects = 0.5f;

    public void ChangeMusicVal(float newVal)
    {
        playerSoundMusic = Mathf.Clamp(newVal, 0f, 1f);
    }

    public void ChangeEffectVal(float newVal)
    {
        playerSoundEffects = Mathf.Clamp(newVal, 0f, 1f);
    }
}
