using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyLandArea : MonoBehaviour
{
    [TabGroup("Options")][Range(0f, 1f)] public float fill;
    [TabGroup("Options")] public int requiredMoney;

    private int requiredMoneyInit;
    private bool isBuying;

    [TabGroup("References")][SerializeField] private TMP_Text requiredMoneyText;
    [TabGroup("References")] public Image[] progressBar;

    private void Awake()
    {
        requiredMoneyText = GetComponentInChildren<TMP_Text>();
        requiredMoneyInit = requiredMoney;
    }

    private void Update()
    {
        requiredMoneyText.text = requiredMoney.ToString();

        if (requiredMoney <= 0)
        {
            Destroy(this.gameObject);
            LandManager.Instance.NextLand(transform);
        }

        if(CheckPlayerMoney())
            SetProgressBarColor();
    }

    public void UpdateProgressBar()
    {
        fill = 1 - (requiredMoney / (float)requiredMoneyInit);

        progressBar[0].fillAmount = Mathf.InverseLerp(0f, 0.25f, fill);
        progressBar[1].fillAmount = Mathf.InverseLerp(0.25f, 0.5f, fill);
        progressBar[2].fillAmount = Mathf.InverseLerp(0.5f, 0.75f, fill);
        progressBar[3].fillAmount = Mathf.InverseLerp(0.75f, 1f, fill);
    }

    private void SetProgressBarColor()
    {
        if (isBuying)
        {
            for (int i = 0; i < progressBar.Length; i++)
            {
                progressBar[i].color = Color.green;
            }
        }
        else
        {
            for (int i = 0; i < progressBar.Length; i++)
            {
                progressBar[i].color = Color.red;
            }
        }
    }

    private bool CheckPlayerMoney()
    {
        if(UIManager.Instance.currentMoney <= 0)
        {
            isBuying = false;
        }

        return isBuying;
    }

    public IEnumerator IEDesiredMoney()
    {
        float decreaseRate = 5f; 
        float delayBetweenFrames = 0.01f; 

        while (requiredMoney > 0 && UIManager.Instance.currentMoney > 0)
        {
            float decreaseAmount = Mathf.Min(decreaseRate, requiredMoney, UIManager.Instance.currentMoney);

            UIManager.Instance.currentMoney -= (int)decreaseAmount;
            requiredMoney -= (int)decreaseAmount;

            UpdateProgressBar();

            yield return new WaitForSeconds(delayBetweenFrames);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() && !isBuying)
        {
            isBuying = true;
            StartCoroutine(IEDesiredMoney());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        StopAllCoroutines();
        isBuying = false;
    }

}
