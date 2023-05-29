using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class got : MonoBehaviour
{
    private Vector3 initScale;
    public float timeOffset;

    private Material m;

    private void Awake()
    {

        m = GetComponent<MeshRenderer>().material= new Material(GetComponent<MeshRenderer>().material);
        
        initScale = transform.localScale;
    }

    private void Update()
    {
        Vector3 maxScale = initScale + Vector3.one * 0.0005f;

        transform.localScale = initScale + Vector3.one * Mathf.Abs(Mathf.Sin((Time.time + timeOffset) * 3)) * 0.00075f;

        //m.color = Color.Lerp(new Color(0.8980392f, 0.7294118f, 0.7294118f, 1), Color.red, Mathf.InverseLerp(initScale.x, maxScale.x, transform.localScale.x));

    }
}
