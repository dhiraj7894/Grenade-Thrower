using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _EnemyManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private float turnSmoothVelocity;
    private Animator anime;
    public float rotationSmooth = 0.2f;
    public Transform pos1, pos2;
    private GameObject Mesh1, Mesh2;
    public bool isDead = false; 


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = transform.GetChild(0).GetComponent<Animator>();
        Mesh1 = transform.GetChild(0).gameObject;
        Mesh2= transform.GetChild(1).gameObject;

        Mesh1.SetActive(true);
        Mesh2.SetActive(false);
    }


    void Update()
    {
        lookAtPlayer();
        petrolling(pos1.position, pos2.position);
    }

    bool reachPos1 = false;
    void petrolling(Vector3 pos1, Vector3 pos2)
    {
        if (reachPos1)
        {
            agent.SetDestination(pos2);
        }
        if (!reachPos1)
        {
            agent.SetDestination(pos1);
        }
    }
    void lookAtPlayer()
    {
        if (agent.velocity.magnitude > 0)
        {
            anime.SetBool("walk", true);
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else if (agent.velocity.magnitude <= 0)
        {
            anime.SetBool("walk", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EP1"))
            reachPos1 = true;

        if (other.gameObject.CompareTag("EP2"))
            reachPos1 = false;

        if (other.gameObject.CompareTag("grenade"))
        {
            agent.speed = 0;
            Mesh1.SetActive(false);
            Mesh2.SetActive(true);
            isDead = true;
        }
    }

}
