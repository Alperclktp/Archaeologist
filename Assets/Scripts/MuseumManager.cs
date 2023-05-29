using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuseumManager : MonoBehaviour
{
    public static MuseumManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> museumEmptyBoardList = new List<GameObject>();

    public List<GameObject> museumList = new List<GameObject>();

}
