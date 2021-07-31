using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Grenade : MonoBehaviour
{
    public ParticleSystem _Smoke, _Explosion;
    public SphereCollider collider; 
    public bool isNotExplode = false;
    [SerializeField] private float timer = 2;
    [SerializeField] private float explosionArea   = 2;
    [SerializeField] private float explodeSpeed   = 2;

    private void Start()
    {
        collider.radius = 0;
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        Explode();
    }

    void Explode()
    {
        if (!isNotExplode && timer <= 0)
        {
            if (collider.radius <= explosionArea)
            {
                collider.radius += Time.deltaTime * explodeSpeed;
            }
            _Explosion.Play();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            if(collider.radius >= explosionArea)
            {
                Destroy(this.gameObject, 0.4f);
                isNotExplode = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.CompareTag("Environment"))
            _Smoke.Play();
    }
}
