using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Player player;
    public Level level;
    public SceneParticleManager spm;
    private void Awake()
    {
        player = new Player();
    }

    private void Start()
    {
        //player.Instantiation(level.responPoint.position);
    }
    private void Update()
    {
        //CoroutineManager.Instance.Update();
    }

}
