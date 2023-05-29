using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [TabGroup("Options")] public int currentMoney;

    [TabGroup("References")] public TMP_Text currentMoneyText;

    [TabGroup("References")] public GameObject gameCanvas;

    [TabGroup("References")] public GameObject detectSignal;
    [TabGroup("References")] public GameObject maxHoldSignal;
    [TabGroup("References")] public GameObject goTargetArrow;

    [TabGroup("References")] public Button scanButton;
    [TabGroup("References")] public Button cleanerButton;
    [TabGroup("References")] public Button shoverButton;

    private void Awake()
    {
        Instance = this;

        currentMoneyText = gameCanvas.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        currentMoneyText.text = currentMoney.ToString() + "$";

        if (currentMoney <= 0)
            currentMoney = 0;
    }
}
