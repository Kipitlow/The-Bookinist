using UnityEngine;

[System.Serializable]
public class PlayerSettings
{
    public float playerSoundGeneral = 0.5f;
    public float playerSoundMusic = 0.5f;
    public float playerSoundSound = 0.5f;

    public void ChangeGeneralVal(float newVal)
    {
        playerSoundGeneral = Mathf.Clamp(newVal, 0f, 1f);
    }

    public void ChangeMusicVal(float newVal)
    {
        playerSoundMusic = Mathf.Clamp(newVal, 0f, 1f);
    }

    public void ChangeEffectVal(float newVal)
    {
        playerSoundSound = Mathf.Clamp(newVal, 0f, 1f);
    }
}
