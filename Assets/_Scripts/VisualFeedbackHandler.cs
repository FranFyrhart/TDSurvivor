using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VisualFeedbackHandler : MonoBehaviour
{
    private Tween colorTween;
    private Material material;

    public void FlashMaterialColor()
    {
        if (colorTween != null)
        {
            if (!colorTween.IsComplete())
            {

                Debug.Log("Color tween not yet completed");
                return;
            }

            Debug.Log($"playing tween {colorTween.IsActive()}");
            colorTween.Restart();
            return;
        }

        colorTween = material.DOColor(Color.black, "_BaseColor", 0.1f).From().SetLoops(1).SetAutoKill(false);
    }
}