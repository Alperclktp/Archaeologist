using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Museum : MonoBehaviour
{
    [TabGroup("Referances")] public Transform boneHolderPoint;

    private void Awake()
    {
        boneHolderPoint = transform.GetChild(0).transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(PlayerInteraction.Instance.goldHolder.childCount >= 1 && boneHolderPoint.childCount <= 0)
        {
            transform.parent.gameObject.layer = 0;

            UIManager.Instance.goTargetArrow.SetActive(false);

            GameObject obj = transform.GetChild(0).gameObject;

            PlayerInteraction.Instance.goldHolder.GetChild(0).transform.DOJump(boneHolderPoint.position,1,1,0.5f)
            .OnComplete(() =>
            {
                PlayerInteraction.Instance.goldHolder.GetChild(0).SetParent(boneHolderPoint);

                boneHolderPoint.transform.GetChild(0).transform.position = Vector3.zero;
                boneHolderPoint.transform.GetChild(0).transform.rotation = Quaternion.Euler(0,180,0);

                boneHolderPoint.transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;

                MuseumManager.Instance.museumList.Add(obj);
            }); 
        }
    }
}
