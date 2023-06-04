using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private Player player;
    public Level level;
    public SceneParticleManager spm;
    protected override void Awake()
    {
        base.Awake();
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
