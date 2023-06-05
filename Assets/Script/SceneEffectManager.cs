using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class SceneEffectManager : MonoBehaviour
{
    [SerializeField] private Transform OnceFolder;
    [SerializeField] private Transform KeepFolder;
    private float _pauseTimer;

    private Dictionary<string, ParticleSystem> OnceDic = new Dictionary<string, ParticleSystem>();
    private Dictionary<string, KeepEntity> KeepDic = new Dictionary<string, KeepEntity>();
    private void Awake()
    {
        foreach(var a in OnceFolder.GetComponentsInChildren<ParticleSystem>())
            OnceDic.Add(a.name, a);
        foreach (var a in KeepFolder.GetComponentsInChildren<ParticleSystem>())
            KeepDic.Add(a.name, new KeepEntity(a));
    }

    private void Update()
    {
        if (pauseTimer > 0) pauseTimer -= Time.unscaledDeltaTime;
    }

    private void LateUpdate()
    {
        foreach(var a in KeepDic)
        {
            if (a.Value.time > 0)
                a.Value.time-= Time.deltaTime;
            else
                a.Value.effect.gameObject.SetActive(false);
        }
    }

    public void PlayOnce(string name,Vector3 position, Vector3? rotation=null,Vector3? scale=null)
    {
        if (rotation==null) rotation = new Vector3(-90, 0, 0);
        if (scale == null) scale = Vector3.one;
        if(OnceDic.TryGetValue(name,out ParticleSystem ps))
        {
            ps.transform.position = position;
            ps.transform.rotation = Quaternion.Euler(rotation.Value);
            ps.transform.localScale = scale.Value;
            ps.Play();
        }
    }

    public ParticleSystem PlayKeep(string name, Vector3 position, Vector3? rotation = null, Vector3? scale = null, float time =0.2f)
    {
        if (rotation == null) rotation = new Vector3(-90, 0, 0);
        if (scale == null) scale = Vector3.one;
        if (KeepDic.TryGetValue(name, out KeepEntity ps))
        {
            ps.time = time;
            ps.effect.transform.position= position;
            ps.effect.transform.rotation = Quaternion.Euler(rotation.Value);
            ps.effect.transform.localScale = scale.Value;
            ps.effect.gameObject.SetActive(true);
            if (!ps.effect.isPlaying) ps.effect.Play();
            return ps.effect;
        }
        return null;
    }

    public float pauseTimer
    { set { _pauseTimer = value; Time.timeScale = value > 0 ? 0 : 1; } get { return _pauseTimer; } }

}

public class KeepEntity
{
    public ParticleSystem effect;
    public float time;

    public KeepEntity(ParticleSystem effect)
    {
        this.effect = effect;
        time = 0;
    }
}