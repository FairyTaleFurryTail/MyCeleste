using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
    public void DustOnLand(Vector3 position)
    {
        GameManager.Instance.spm.DustOnLand(position);
    }
}
