using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;

public class Land : MonoBehaviour
{
    [FoldoutGroup("Debug")] public bool goldFound;
    [FoldoutGroup("Debug")] public bool goldPicked;

    [TabGroup("Options")] public bool hasGold;
    [TabGroup("Options")] public float goldPickDuration;
    [TabGroup("Options")] public AnimationCurve goldPickCurve;

    [TabGroup("References")] public GameObject gold;
    [TabGroup("References")] public Transform soil;

    [HideInInspector] public LandDeformerSystem landDeformerSystem;

    private void Awake()
    {
       landDeformerSystem = GetComponent<LandDeformerSystem>();

       hasGold = Random.value < 0.5f; //Test

       gold.SetActive(hasGold);
    }

    public void PickGold()
    {
        //if (!hasGold) Debug.Log("<color=#faa>AMINI SÝKEEEM <(OoO)></color>");
        //else Debug.Log("<color=#afa>Allah allah :c?</color>");

        ZýplaSikerim();

        goldPicked = true;
        //gold.SetActive(false);

        PlayerInteraction.Instance.canDig = false;
        PlayerInteraction.Instance.backShovel.SetActive(true);

        UIManager.Instance.goTargetArrow.SetActive(true);
   
    }

    public void ZýplaSikerim()
    {
        if (goldPicked) return;

        StartCoroutine(IECollectGold());
    }

    private IEnumerator IECollectGold()
    {
        Vector3 pos = gold.transform.position;
        Quaternion rot = gold.transform.rotation;

        float time = 0;

        while (time < goldPickDuration)
        {
            float lerp = time / goldPickDuration;

            gold.transform.position = Vector3.Lerp(pos, PlayerInteraction.Instance.goldHolder.position, lerp) + Vector3.up * goldPickCurve.Evaluate(lerp);
            gold.transform.rotation = Quaternion.Lerp(rot, PlayerInteraction.Instance.goldHolder.rotation, lerp);

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        gold.transform.parent = PlayerInteraction.Instance.goldHolder;

        gold.transform.localPosition = Vector3.zero;
        gold.transform.localRotation = Quaternion.identity;

    } 

    public void GoldFound()
    {
        goldFound = true;
    }
}
