using System;
using System.Collections;
using UnityEngine;
using static PlayerEntity;

public class Player
{
    public PlayerEntity playerEntity;
    public void Instantiation(Vector2 pos)
    {
        playerEntity = GameObject.Instantiate
            (Resources.Load<GameObject>(Path.prefabPath + "Player"),pos,Quaternion.identity).GetComponent<PlayerEntity>();
    }
}
