using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/VFX", fileName = "VFX", order = 2)]
public class VFXSO : ScriptableObject
{
    public VFXType type;

    public GameObject prefab;
}
