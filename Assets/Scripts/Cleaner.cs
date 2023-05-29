using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{

    [TabGroup("Options")] public float shrinkSpeed = 30;
    [TabGroup("Options")] public float destroyThreshold = 1.5f;

    private void OnTriggerStay(Collider other)
    {
        var cleanable = other.GetComponent<CleanableObject>();
        if (cleanable) cleanable.Clean();
    }
}
