using UnityEngine;

public class LightPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.LightPelletEaten(this);
        SoundEffect soundEffect = gameObject.GetComponentInParent<SoundEffect>();
        soundEffect.playSound(1);
    }

}
