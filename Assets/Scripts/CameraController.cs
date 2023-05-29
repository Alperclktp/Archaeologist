using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float followSpeed;

    [TabGroup("References")][SerializeField] private Transform target;
    [TabGroup("References")][SerializeField] private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, followSpeed * 60 * Time.deltaTime);

        transform.position = newPosition;
    }
}
