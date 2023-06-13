using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level : MonoBehaviour
{
    public Transform respawnPointFolder;
    private List<Transform> respawnPoints=new List<Transform>();
    public int curIndex;
    private void Awake()
    {
        foreach(Transform a in respawnPointFolder.GetComponentInChildren<Transform>())
            respawnPoints.Add(a);
        curIndex = 0;
    }
    public Transform curPoint { get { return respawnPoints[curIndex]; } }
}