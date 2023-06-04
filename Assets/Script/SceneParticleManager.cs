using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem dustOnLand;
    public void DustOnLand(Vector3 position)
    {
        dustOnLand.transform.position = position;
        dustOnLand.Play();
    }
}
