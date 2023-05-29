using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMuseum : MonoBehaviour
{
    [TabGroup("Options")] public float searchRadius;

    [TabGroup("Referances")] public Transform target;

    private void Update()
    {
        FindNearestTarget();
        LookAtTarget();
    }

    private void FindNearestTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Target"));

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider targetCollider in targets)
        {
            Transform targetTransform = targetCollider.transform;
            float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                closestTarget = targetTransform;
            }
        }

        target = closestTarget;
    }

    private void LookAtTarget()
    {
        if (target != null)
        {
            Vector3 direction = transform.position - target.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }   
    }
}
