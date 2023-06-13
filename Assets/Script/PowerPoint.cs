using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPoint : MonoBehaviour
{
    private float responTimer;
    [SerializeField] private float responTime;
    [SerializeField] private Animator anim;
    [SerializeField] private ParticleSystem activeParticle;
    private bool _active=true;
    private bool active { get { return _active; }
        set { _active = value;if (_active) { anim.Play("Active"); activeParticle.Play();} else anim.Play("InActive"); } }

    private void Update()
    {
        responTimer.TimePassBy();
        if (responTimer <= 0 && active != true) active = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.parent.CompareTag("Player"))
        {
            responTimer = responTime;
            PlayerEntity p = collision.transform.parent.parent.GetComponent<PlayerEntity>();
            p.dashes = p.maxDashes;
            p.Stamina = PlayerEntity.ClimbSet.ClimbMaxStamina;
            active = false;
        }
    }

}
