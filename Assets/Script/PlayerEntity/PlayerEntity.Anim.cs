using System.Collections;
using UnityEngine;

public partial class PlayerEntity : MonoBehaviour
{
    private void UpdateSprite()
    {
        scale.x = Mathf.MoveTowards(scale.x, 1, 1.75f * Time.deltaTime);
        scale.y = Mathf.MoveTowards(scale.y, 1, 1.75f * Time.deltaTime);
    }

    private IntervalTimer flashInterval = new IntervalTimer(TimeSet.FlashInterval);
    private bool flashing = false;
    private void AnimUpdate()
    {
        flashInterval.start = IsTired;
        flashing = flashInterval.GetStatus();
        if (flashing)
            sprite.color = flashColor;
        else
            sprite.color = Color.white;
    }
}
