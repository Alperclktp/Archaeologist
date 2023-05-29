using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [TabGroup("Options")] public float detectionRange;
    [TabGroup("Options")] public float repeatDetectEffectTime;

    private Vector3 newDetectPosition;

    private void Update()
    {
        DetectObjects();
    }

    private void DetectObjects()
    {
        Vector3 detectionPosition = CalculateDetectionPosition();

        Collider[] colliders = Physics.OverlapSphere(detectionPosition, detectionRange);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.CompareTag("Gold"))
            {
                PlayerInteraction.ActiveLand.GoldFound();

                //ActivateDetectionEffects();

                //Debug.Log("<color=#ffa>ALTIN BULDUN</color> <color=#faa>AQ</color> <color=#fff><(OoO)></color>");
            }
        }
    }

    private void ActivateDetectionEffects()
    {
        UIManager.Instance.detectSignal.SetActive(true);

        if (!PlayerInteraction.Instance.isEffectRunning)
        {
            StartCoroutine(PlayerInteraction.Instance.IEDetectGoldEffect(repeatDetectEffectTime));
        }
    }

    private Vector3 CalculateDetectionPosition()
    {
        Vector3 detectionPosition = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        return detectionPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(newDetectPosition, detectionRange);
    }
}
