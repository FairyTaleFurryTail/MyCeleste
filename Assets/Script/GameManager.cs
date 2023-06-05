using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private Player player;
    [SerializeField] private Level _level;
    [SerializeField] private SceneEffectManager _sem;
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


    public static SceneEffectManager sem { get { return Instance._sem; } }

}
