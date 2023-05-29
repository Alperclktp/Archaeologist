using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VisutorManager : MonoBehaviour
{
    public static VisutorManager Instance;

    [TabGroup("Options")][SerializeField] private float spawnInterval;

    [TabGroup("References")][SerializeField] private GameObject visitorPrefab;

    public List<GameObject> allVisitors = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject obj = Instantiate(visitorPrefab, transform.position, Quaternion.identity);

            allVisitors.Add(obj);

            StartCoroutine(IEFadeIn(obj));
        }
    }

    private IEnumerator IEFadeIn(GameObject obj)
    {
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        Material sharedMaterial = renderers[0].material; 

        sharedMaterial.SetFloat("_Mode", 2);
        sharedMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        sharedMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        sharedMaterial.SetInt("_ZWrite", 0);
        sharedMaterial.DisableKeyword("_ALPHATEST_ON");
        sharedMaterial.EnableKeyword("_ALPHABLEND_ON");
        sharedMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        sharedMaterial.renderQueue = 3000;

        Color color = sharedMaterial.color;
        Color targetColor = color;
        targetColor.a = 1;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;

            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = Color.Lerp(color, targetColor, normalizedTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
