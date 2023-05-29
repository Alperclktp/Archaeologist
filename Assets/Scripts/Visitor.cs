using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float stoppingDistance;

    [TabGroup("References")][SerializeField] private Transform exitPos;

    [TabGroup("References")][SerializeField] GameObject currentBone;
    [TabGroup("References")][SerializeField] GameObject currentEmptyBoard;

    [FoldoutGroup("Debug")][SerializeField] private bool isVisiting;
    [FoldoutGroup("Debug")][SerializeField] private bool isWalking;
    [FoldoutGroup("Debug")][SerializeField] private bool isBuying;

    private NavMeshAgent agent;

    private Renderer rend;

    private Animator anim;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (MuseumManager.Instance.museumList.Count == 0)
        {
            StartCoroutine(IEVisitMuseumEmptyBoard());
        }
    }

    private void Update()
    {
        agent.stoppingDistance = stoppingDistance;

        if (!isVisiting)
        {
            StartCoroutine(IEVisitMuseumBoard());
        }

        UpdateVisitorAnimations();
    }

    private IEnumerator IEVisitMuseumBoard()
    {
        MuseumManager museumManager = MuseumManager.Instance;

        isVisiting = true;

        while (museumManager.museumList.Count != 0)
        {
            currentEmptyBoard = null;

            int randomIndex = Random.Range(0, museumManager.museumList.Count);
            currentBone = museumManager.museumList[randomIndex].transform.GetChild(0).gameObject;

            isWalking = true;

            Vector3 targetPosition = currentBone.transform.position;
            targetPosition.y = transform.position.y;

            agent.SetDestination(targetPosition);

            yield return new WaitForSeconds(0.5f);

            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) <= stoppingDistance);

            isWalking = false;

            isBuying = true;

            yield return new WaitForSeconds(Random.Range(1f, 2f));

            StartCoroutine(IEVisitMuseumEmptyBoard());

            if (currentBone != null)
            {
                ReceiveMoney(currentBone);

                Destroy(currentBone);

                museumManager.museumList.RemoveAt(randomIndex);

                StartCoroutine(IEGoToExit());

                isBuying = false;
            }
            else
            {
                //StartCoroutine(IEVisitMuseumEmptyBoard());
            }

            yield return new WaitForSeconds(1f);

            /*
            if (museumManager.museumList.Count > 0)
            {
                int newRandomIndex = Random.Range(0, museumManager.museumList.Count);
                GameObject newRandomBone = museumManager.museumList[newRandomIndex];

                isWalking = true;

                targetPosition = newRandomBone.transform.position;
                targetPosition.y = transform.position.y;

                agent.SetDestination(targetPosition);

                yield return new WaitForSeconds(0.5f);

                isWalking = false;
            }
            */
        }

        isVisiting = false;
    }

    private IEnumerator IEVisitMuseumEmptyBoard()
    {
        MuseumManager museumManager = MuseumManager.Instance;

        isVisiting = true;

        int randomIndex = Random.Range(0, museumManager.museumEmptyBoardList.Count);
        currentEmptyBoard = museumManager.museumEmptyBoardList[randomIndex];

        //currentEmptyBoard = museumManager.museumEmptyBoardList.OrderBy(mb => Vector3.Distance(transform.position, mb.transform.position)).First();

        isWalking = true;

        Vector3 targetPosition = currentEmptyBoard.transform.position;
        targetPosition.y = transform.position.y;

        agent.SetDestination(targetPosition);

        yield return new WaitForSeconds(0.5f);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) <= stoppingDistance);

        isWalking = false;

        yield return new WaitUntil(() => museumManager.museumList.Count == 0);

        isVisiting = false;
    }

    private void ReceiveMoney(GameObject bone)
    {
        string visitorName = this.gameObject.name;
        int sellAmount = 150;

        string formattedVisitorName = string.Format("<color=#00ff00><b>{0}</b></color>", visitorName);
        string logMessage = string.Format("{0}: <color=#ff7337>ADLI ZIYARETCI TARAFINDAN </color> {1} <color=#0099bc><b> ADLI OBJE </b></color> <color=#0099bc><b> {2}$ </b></color> <color=#0099bc><b>FIYATA SATILDI.</b></color>", formattedVisitorName, bone.name, sellAmount);
        Debug.Log(logMessage);

        UIManager.Instance.currentMoney += sellAmount;
    }


    private IEnumerator IEGoToExit()
    {
        isWalking = true;

        stoppingDistance = 0;

        transform.LookAt(exitPos.transform);

        agent.SetDestination(exitPos.position);

        yield return new WaitUntil(() => Vector3.Distance(transform.position, exitPos.position) < 1);

        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        rend.material.SetFloat("_Mode", 2);
        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.EnableKeyword("_ALPHABLEND_ON");
        rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = 3000;

        Color color = rend.material.color;
        Color targetColor = color; targetColor.a = 0;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;

            rend.material.color = Color.Lerp(color, targetColor, normalizedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        VisutorManager.Instance.allVisitors.Remove(this.gameObject);

        Destroy(gameObject);
    }

    private void UpdateVisitorAnimations()
    {
        if (isWalking)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}