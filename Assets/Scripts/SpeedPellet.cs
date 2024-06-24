using UnityEngine;

public class SpeedPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.SpeedPelletEaten(this);
        SoundEffect soundEffect = gameObject.GetComponentInParent<SoundEffect>();
        soundEffect.playSound(2);
    }

}
