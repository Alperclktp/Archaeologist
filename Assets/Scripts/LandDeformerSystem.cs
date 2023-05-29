using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandDeformerSystem : MonoBehaviour
{
    [TabGroup("Options")][Range(1, 1.5f)][SerializeField] private float radius;
    [TabGroup("Options")] public float raycastDistance;
    [TabGroup("Options")][Range(0.2f, 1f)][SerializeField] private float deformationStength;

    private Land land;

    private Mesh mesh;
    private MeshFilter meshFilter;

    private RaycastHit hit;

    [ReadOnly] public Vector3[] verticies, modifiedVerts;

    private void Awake()
    {

        land = GetComponent<Land>();
    }

    private void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        mesh = meshFilter.mesh;
        verticies = mesh.vertices;
        modifiedVerts = mesh.vertices;
    }

    private void FixedUpdate()
    {
        if (land == PlayerInteraction.ActiveLand)
            DeformGroundFromPosition(PlayerController.Instance.transform.position + PlayerController.Instance.transform.forward * 0.5f);
    }

    public void DeformGround()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(PlayerInteraction.Instance.handShovel.transform.position, PlayerInteraction.Instance.handShovel.transform.forward);

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            for (int v = 0; v < modifiedVerts.Length; v++)
            {
                Vector3 hitPoint = meshFilter.transform.InverseTransformPoint(hit.point);
                hitPoint.y = 0;

                Vector3 unmodifiedVert = modifiedVerts[v];
                unmodifiedVert.y = 0;

                Vector3 distance = unmodifiedVert - hitPoint;

                float smoothinghgFactor = 2f;
                float force = deformationStength / (1f + hitPoint.sqrMagnitude);

                if (distance.sqrMagnitude < radius)
                {
                    if (PlayerInteraction.Instance.canDig)
                    {
                        modifiedVerts[v] = modifiedVerts[v] + (Vector3.down * force) / smoothinghgFactor;
                    }
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        RecalculateMesh();
    }

    public void DeformGroundFromPosition(Vector3 position)
    {
        for (int v = 0; v < modifiedVerts.Length; v++)
        {
            Vector3 point = meshFilter.transform.InverseTransformPoint(position);
            point.y = 0;

            Vector3 unmodifiedVert = modifiedVerts[v];
            unmodifiedVert.y = 0;

            Vector3 distance = unmodifiedVert - point;

            float smoothinghgFactor = 2f;
            float force = deformationStength / (1f + point.sqrMagnitude);

            if (distance.sqrMagnitude < radius)
            {
                if (PlayerInteraction.Instance.canDig)
                {
                    modifiedVerts[v] = modifiedVerts[v] + (Vector3.down * force) / smoothinghgFactor;
                }
            }
        }

        RecalculateMesh();
    }

    private void RecalculateMesh()
    {
        mesh.vertices = modifiedVerts;
        GetComponentInChildren<MeshCollider>().sharedMesh = mesh;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hit.point, radius);
    }
}
