using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LandManager : MonoBehaviour
{
    public static LandManager Instance;

    [TabGroup("Options")] public float offset;

    [TabGroup("Referances")][SerializeField] private GameObject prefab;
    [TabGroup("Referances")][SerializeField] private Transform firstLandSpawnPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeLandSpawner();
    }

    private void InitializeLandSpawner()
    {
        Instantiate(prefab, firstLandSpawnPosition.position, Quaternion.identity);
    }

    public void NextLand(Transform spawnAt)
    {
        Vector3 pos = spawnAt.position + spawnAt.forward * offset;
        pos.y = firstLandSpawnPosition.position.y;

        Instantiate(prefab, pos, spawnAt.rotation);
    }
}
