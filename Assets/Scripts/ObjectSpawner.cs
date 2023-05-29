using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private int count;
    [TabGroup("Options")][SerializeField] private float radius;
    [TabGroup("Options")][SerializeField] private float separationDistance;

    [TabGroup("Options")][SerializeField] private GameObject[] prefabs;
    [TabGroup("Options")] public List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        StartCoroutine(IESpawnGenerator());
    }

    private IEnumerator IESpawnGenerator()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = Random.insideUnitSphere * radius;
            randomPos.y = 1;

            Vector3 worldPos = transform.position + randomPos;

            RaycastHit hit;
            if (Physics.Raycast(worldPos, Vector3.down, out hit))
            {
                int prefabIndex = Random.Range(0, prefabs.Length);
                GameObject prefab = prefabs[prefabIndex];

                GameObject obj = Instantiate(prefab, hit.point, prefab.transform.rotation);
                obj.transform.position += Vector3.up * 0.10f;
                obj.transform.SetParent(transform);

                spawnedObjects.Add(obj);

                CheckNested(obj);

                if (obj.transform.position.y > 0.5f)
                {
                    obj.transform.position = new Vector3(obj.transform.position.x, 0.5f, obj.transform.position.z);
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void CheckNested(GameObject obj)
    {
        foreach (GameObject otherObj in spawnedObjects)
        {
            if (obj == otherObj) continue;

            Vector3 diff = obj.transform.position - otherObj.transform.position;
            if (diff.magnitude < separationDistance)
            {
                obj.transform.position += diff.normalized * (separationDistance - diff.magnitude);
            }
        }
    }

    public void RemoveFromSpawnedList(GameObject obj)
    {
        spawnedObjects.Remove(obj);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, radius);
    }
}
