using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this);
        SoundEffect soundEffect = gameObject.GetComponentInParent<SoundEffect>();
        soundEffect.playSound(0);
    }

}
