using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
   
    public static Land ActiveLand;

    private UIManager uIManager;

    [TabGroup("References")] public GameObject backShovel;
    [TabGroup("References")] public GameObject handShovel;
    [TabGroup("References")] public GameObject handScanner;
    [TabGroup("References")] public GameObject handCleaner;

    [TabGroup("References")] public Transform soilEffectTransform;
    [TabGroup("References")] public Transform soilScanEffectTransform;
    [TabGroup("References")] public Transform goldHolder;

    [FoldoutGroup("Debug")] public bool canDig;
    [FoldoutGroup("Debug")] public bool canScan;
    [FoldoutGroup("Debug")] public bool hasHold;
    [FoldoutGroup("Debug")] public bool canClean;

    [FoldoutGroup("Debug")][SerializeField] public bool landArea;

    [HideInInspector] public bool isEffectRunning = false;

    private bool canPlayEffect = true;

    private void Awake()
    {
        Instance = this;

        uIManager = UIManager.Instance;
    }

    private void Start()
    {
        canScan = false; canClean = false; canDig = false;
    }

    private void Update()
    {
        HandleLandInteractions();

        CheckHoldAvailability();
    }

    private void HandleLandInteractions()
    {
        ActiveLand = GetActiveLand();

        if (ActiveLand != null)
        {
            if (!ActiveLand.goldFound)
                EnableScanMode();
            else if (ActiveLand.soil.childCount > 0)
                EnableCleanMode();
            else if (!ActiveLand.goldPicked)
                EnableDigMode();
        }
        else
        {
            landArea = false;

            SetInteractionStatus(false);
        }

        handScanner.SetActive(canScan);
        handCleaner.SetActive(canClean);
        handShovel.SetActive(canDig);
    }

    public void EnableScanModeButton() { canScan = true; canClean = false; canDig = false; backShovel.SetActive(true); }

    private void EnableScanMode()
    {
        landArea = true;

        uIManager.scanButton.gameObject.SetActive(!canScan);
        uIManager.cleanerButton.gameObject.SetActive(false);
        uIManager.shoverButton.gameObject.SetActive(false);
        uIManager.detectSignal.SetActive(false);

        canClean = false; canDig = false;
    }

    public void EnableCleanModeButton() { canClean = true; canScan = false; canDig = false; UIManager.Instance.detectSignal.SetActive(false); backShovel.SetActive(true); }

    private void EnableCleanMode()
    {
        uIManager.scanButton.gameObject.SetActive(false);
        uIManager.cleanerButton.gameObject.SetActive(!canClean);
        uIManager.detectSignal.gameObject.SetActive(!canClean);
        uIManager.shoverButton.gameObject.SetActive(false);

        canScan = false; canDig = false;
    }

    public void EnableDigModeButton() { canDig = true; canScan = false; canClean = false; backShovel.SetActive(false); }

    private void EnableDigMode()
    {
        uIManager.scanButton.gameObject.SetActive(false);
        uIManager.cleanerButton.gameObject.SetActive(false);
        uIManager.shoverButton.gameObject.SetActive(!canDig);

        StartCoroutine(IESoilEffect());

        canScan = false; canClean = false;
    }

    private void SetInteractionStatus(bool status)
    {
        handScanner.SetActive(status);
        handCleaner.SetActive(status);
        handShovel.SetActive(status);

        UIManager.Instance.scanButton.gameObject.SetActive(status);
        UIManager.Instance.cleanerButton.gameObject.SetActive(status);
        UIManager.Instance.shoverButton.gameObject.SetActive(status);

        canScan = status;
        canClean = status;
        canDig = status;
    }

    private Land GetActiveLand()
    {
        var overlap = Physics.OverlapBox(transform.position, new Vector3(0, 10, 0), Quaternion.identity, LayerMask.GetMask("Scannable"));

        if (overlap.Length != 0 && !hasHold)
        {
            return overlap[0].GetComponentInParent<Land>();
        }

        return null;
    }

    public void OnSoilEffect()
    {
        Vector3 newEffectPosition = new Vector3(soilEffectTransform.position.x, transform.position.y - 0.05f, soilEffectTransform.position.z);

        Destroy(VFXManager.SpawnEffect(VFXType.SoilEffect, newEffectPosition, soilEffectTransform), 1);
    }

    public void OnSoilScanRippleEffect()
    {
        Vector3 newEffectPosition = new Vector3(soilScanEffectTransform.position.x, soilScanEffectTransform.position.y, soilScanEffectTransform.position.z);

        Destroy(VFXManager.SpawnEffect(VFXType.SoilScanRippleEffect, newEffectPosition, soilScanEffectTransform), 1);
    }

    private void CheckHoldAvailability()
    {
        hasHold = goldHolder.transform.childCount != 0;

        if (hasHold)
        {
            UIManager.Instance.maxHoldSignal.SetActive(true);
            hasHold = true;
        }
        else
        {
            UIManager.Instance.maxHoldSignal.SetActive(false);
            hasHold = false;
        }
    }

    public IEnumerator IEDetectGoldEffect(float repeatTime)
    {
        if (canPlayEffect)
        {
            isEffectRunning = true;
            canPlayEffect = false;

            OnSoilScanRippleEffect();

            yield return new WaitForSeconds(repeatTime);

            canPlayEffect = true;
            isEffectRunning = false;
        }
    }

    public IEnumerator IESoilEffect()
    {
        if (canPlayEffect)
        {
            isEffectRunning = true;
            canPlayEffect = false;

            OnSoilEffect();

            yield return new WaitForSeconds(3f);

            canPlayEffect = true;
            isEffectRunning = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Gold") && !hasHold)
        {
            ActiveLand.PickGold();
        }
    }

}

