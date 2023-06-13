using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GameManager : SingletonMono<GameManager>
{
    private Player player;
    [SerializeField] private Level _level;
    [SerializeField] private SceneEffectManager _sem;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private CanvasManager _cm;
    protected override void Awake()
    {
        base.Awake();
        player = new Player();
    }

    private void Start()
    {
        NewPlayer(level.curPoint.position);
    }
    private void Update()
    {
        
    }

    public static Level level { get { return Instance._level; } }
    public static SceneEffectManager sem { get { return Instance._sem; } }
    public static CanvasManager cm { get { return Instance._cm; } }
    private void NewPlayer(Vector3 pos)
    {
        PlayerEntity newPlayerEntity = player.Instantiation(pos);
        sem.PlayOnce("Respawn", pos,rotation:Vector3.zero);
        cinemachine.Follow = newPlayerEntity.transform;
    }
    public void Respawn()
    {
        StartCoroutine(cm.SwitchScreen());
        NewPlayer(level.curPoint.position);
    }
}
