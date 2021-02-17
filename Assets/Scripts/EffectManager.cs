using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public const string saveFile = "Assets/effects.json";

    public int totalEffects = 0;
    public List<Effect> effectsCommon;
    public List<Effect> effectsAdvanced;
    public List<Effect> effectsSpecial;
    public List<Effect> effectsDefinitive;

    public void LoadEffects()
    {
        List<Effect> jsonEffects = new List<Effect>();

        var sr = File.OpenText(saveFile);
        var line = sr.ReadLine();
        while (line != null)
        {
            jsonEffects.Add(JsonUtility.FromJson<Effect>(line));
            line = sr.ReadLine();
        }
        sr.Close();

        totalEffects = jsonEffects.Count;
        DistributeEffects(jsonEffects);
    }

    public void SaveEffects()
    {
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
        }

        var sr = File.CreateText(saveFile);
        string json;

        for (var i = 0; i < effectsCommon.Count; i++)
        {
            json = JsonUtility.ToJson(effectsCommon[i]);
            sr.WriteLine(json);
        }
        for (var i = 0; i < effectsAdvanced.Count; i++)
        {
            json = JsonUtility.ToJson(effectsAdvanced[i]);
            sr.WriteLine(json);
        }
        for (var i = 0; i < effectsSpecial.Count; i++)
        {
            json = JsonUtility.ToJson(effectsSpecial[i]);
            sr.WriteLine(json);
        }
        for (var i = 0; i < effectsDefinitive.Count; i++)
        {
            json = JsonUtility.ToJson(effectsDefinitive[i]);
            sr.WriteLine(json);
        }
        sr.Close();
    }

    private void DistributeEffects(List<Effect> jsonEffects)
    {
        effectsCommon = new List<Effect>();
        effectsAdvanced = new List<Effect>();
        effectsSpecial = new List<Effect>();
        effectsDefinitive = new List<Effect>();

        for (var i = 0; i < jsonEffects.Count; i++)
        {
            switch (jsonEffects[i].rarity)
            {
                case Effect.EffectRarity.Common:
                    effectsCommon.Add(jsonEffects[i]);
                    break;
                case Effect.EffectRarity.Advanced:
                    effectsAdvanced.Add(jsonEffects[i]);
                    break;
                case Effect.EffectRarity.Special:
                    effectsSpecial.Add(jsonEffects[i]);
                    break;
                case Effect.EffectRarity.Definitive:
                    effectsDefinitive.Add(jsonEffects[i]);
                    break;
            }
        }
    }

    public void SortAllLists()
    {
        List<Effect> unsortedEffects = new List<Effect>();

        unsortedEffects.AddRange(effectsCommon);
        unsortedEffects.AddRange(effectsAdvanced);
        unsortedEffects.AddRange(effectsSpecial);
        unsortedEffects.AddRange(effectsDefinitive);

        DistributeEffects(unsortedEffects);
    }
}
