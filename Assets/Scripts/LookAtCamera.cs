using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float rotationSpeed;

    private void Update()
    {
        LookAt();
    }

    private void LookAt()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
        targetRotation.z = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
