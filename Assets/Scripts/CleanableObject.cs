using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanableObject : MonoBehaviour
{
    private Cleaner cleaner;

    private void Awake()
    {
        cleaner = FindObjectOfType<Cleaner>(true);
    }

    private void Start()
    {
        StartCoroutine(IEDisabledAnimator());
    }

    private void Update()
    {
        if (transform.localScale.x <= cleaner.destroyThreshold)
            DestroyCleanableObject();
    }

    public void Clean()
    {
        transform.localScale -= Vector3.one * Time.deltaTime * cleaner.shrinkSpeed;
    }

    private void DestroyCleanableObject()
    {
        ObjectSpawner objectSpawner = GetComponentInParent<ObjectSpawner>();
        objectSpawner.RemoveFromSpawnedList(gameObject);
        Destroy(gameObject);
    }

    public void DisableAnimator(bool status)
    {
        GetComponent<Animator>().enabled = status;
    }

    private IEnumerator IEDisabledAnimator()
    {
        yield return new WaitForSeconds(0.5f);

        DisableAnimator(false);
    }
}

