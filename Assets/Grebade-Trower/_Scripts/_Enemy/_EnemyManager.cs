using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _EnemyManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private LevelManager lManager;
    private float turnSmoothVelocity;
    private Animator anime;


    public float rotationSmooth = 0.2f;
    public Transform pos1, pos2;
    private GameObject Mesh1, Mesh2;
    public bool isDead = false;
    public bool chagePlayer = false, detectedByPlayer;

    public List<Rigidbody> bones = new List<Rigidbody>();

    void Start()
    {
        lManager = LevelManager.levelManager;
        agent = GetComponent<NavMeshAgent>();
        anime = transform.GetChild(0).GetComponent<Animator>();
        Mesh1 = transform.GetChild(0).gameObject;
        Mesh2= transform.GetChild(1).gameObject;

        Mesh1.SetActive(true);
        Mesh2.SetActive(false);
    }


    void Update()
    {
            lookAtWalkPoint();

            chagePlayerMathod();

        if (GameManager.gameManager.isDead)
        {
            chagePlayer = false;
        }
            petrolling(pos1.position, pos2.position);


        if (GameObject.Find("Player").GetComponent<_PlayerManager>().isCaughtByPolice)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            agent.speed = 0;
            transform.LookAt(GameObject.Find("Player").transform.position);
            anime.SetBool("run", false);
            anime.SetBool("walk", false);
            anime.SetTrigger("shoot");
        }
        if (GameObject.Find("Player").GetComponent<_PlayerManager>().isClothChnaged){
            Mesh2.GetComponent<Animator>().enabled = true;
        }

        if (isDead)
            die();
    }


    void die()
    {
        agent.speed = 0;
        transform.GetChild(2).gameObject.SetActive(false);
        Mesh1.SetActive(false);
        Mesh2.SetActive(true);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        StartCoroutine(stopBone(0.5f));
    }

    bool reachPos1 = false;
    void petrolling(Vector3 pos1, Vector3 pos2)
    {
        if (!chagePlayer )
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
        if (chagePlayer)
        {
            agent.SetDestination(GameObject.Find("Player").transform.position);
        }

    }

    void chagePlayerMathod()
    {
        if (!GameManager.gameManager.isDead)
        {
            if (!isDead && GameManager.gameManager.overGrenade && detectedByPlayer)
            {
                agent.speed = 2.5f;
                anime.SetBool("run", true);
                StartCoroutine(chageTrue());
            }
        }

        if (!chagePlayer && !isDead)
        {
            agent.speed = 1.5f;
            anime.SetBool("run", false);
        }
    }


    IEnumerator chageTrue()
    {
        yield return new WaitForSeconds(0.5f);
        chagePlayer = true;
    }
    void lookAtWalkPoint()
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
            anime.SetBool("run", false);

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
            isDead = true;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Player").GetComponent<_PlayerManager>().isCaughtByPolice = true;
        }
    }
    IEnumerator stopBone(float t)
    {
        yield return new WaitForSeconds(t);
        for(int i = 0; i < bones.Count - 1; i++)
        {
            bones[i].constraints = RigidbodyConstraints.FreezeAll;
        }
    }

}
