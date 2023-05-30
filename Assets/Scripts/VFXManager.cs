using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class VFXManager
{
    [SerializeField] private static List<VFXGroup> vfxGroups;

    [RuntimeInitializeOnLoadMethod]
    private static void InitResources()
    {
        List<VFXSO> vfxSOs = Resources.LoadAll<VFXSO>("VFX").ToList();

        int groupCount = Enum.GetNames(typeof(VFXType)).Length;

        vfxGroups = new List<VFXGroup>();

        for (int i = 0; i < groupCount; i++)
        {
            VFXGroup vg = new VFXGroup();

            vg.type = (VFXType)i;

            List<VFXSO> subGroup = vfxSOs.FindAll(x => x.type == vg.type);

            foreach (VFXSO vfxSO in subGroup)
            {
                vg.prefabs.Add(vfxSO.prefab);
            }

            vfxGroups.Add(vg);
        }
    }

    public static List<GameObject> GetEffects(VFXType type)
    {
        return vfxGroups.Find(x => x.type == type).prefabs;
    }

    public static GameObject GetEffect(VFXType type)
    {
        List<GameObject> effects = GetEffects(type);

        return effects[UnityEngine.Random.Range(0, effects.Count)];
    }

    public static GameObject SpawnEffect(VFXType type, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        GameObject effect = GameObject.Instantiate(GetEffect(type), position, rotation, parent);

        return effect;
    }

    public static GameObject SpawnEffect(VFXType type, Vector3 position = default, Transform parent = null)
    {
        GameObject effect = GameObject.Instantiate(GetEffect(type), position, default, parent);

        return effect;
    }
}

[System.Serializable]
public class VFXGroup
{
    public VFXType type;

    public List<GameObject> prefabs;

    public VFXGroup()
    {
        prefabs = new List<GameObject>();
    }
}

public enum VFXType
{
   SoilEffect,
   SoilScanRippleEffect,
   MoneyEffect,
   ConfettiEffect,
   BuyAreaEffect
}