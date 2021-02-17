using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSelectorListener : MonoBehaviour
{
    public EffectSelector selector;

    public void UpdateRarities()
    {
        selector.RarityChanged();
    }
    public void UpdateEffects()
    {
        selector.EffectSelected();
    }
}
