using UnityEngine;

public class SlowEffect : IAttackEffect
{
    public float slowAmount;
    public float duration;

    public SlowEffect(float slowAmount, float duration)
    {
        this.slowAmount = slowAmount;
        this.duration = duration;
    }
    public void ApplyEffect(Enemy target)
    {
        if (target != null)
        {
            target.StartSlow(slowAmount, duration);
        }
    }
}
