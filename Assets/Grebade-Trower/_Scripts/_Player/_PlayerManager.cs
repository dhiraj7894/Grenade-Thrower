using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _PlayerManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private float turnSmoothVelocity;
    private Animator anime;
    [SerializeField]private Collider[] hitColliders;


    public _PlayerRotator RotatorScript;

    public Transform EndPoint;
    public ParticleSystem Spwan;
    public Collider triggerCollider;

    [Header("Raycasting Data")]
    public Transform NextPosition;
    public GameObject NearByEnemies;

    [Header("")]
    public LayerMask whatIsNextPoint;
    public LayerMask whatIsEnemy;

    public float maxDistanceToDetetct;
    public float rediusOfNextPos = 3;
    public float rotationSmooth = 0.2f;
    public float runToEnd = 1.5f;

    public bool isHitman, isPolice, isDoctor, isPerson;
    public bool isClothChnaged;
    public bool isCaughtByPolice;

    
    public List<GameObject> charecters = new List<GameObject>();

    [Header("")]
    [SerializeField] float DistFromDead;
    void Start()
    {
        AnimationSetup();
        agent = GetComponent<NavMeshAgent>();
        triggerCollider.enabled = false;
    }

    [SerializeField]float DistFromEnd;
    void Update()
    {
        lookAtPlayer();
        AnimationSetup();
        MouseCheck();
        projectileActivator();

        nextPointCheck();
        if(agent.velocity.magnitude<=0 && !GameManager.gameManager.oneEnemyDead)
            CheckForEnemy();

        movement();
        moveToFinalPos();
        GameManager.gameManager._ProjectileRacePoint.position = RotatorScript.PointPos;
        if (GameManager.gameManager.isDead && !isCaughtByPolice)
        {
            agent.speed = 0;
            anime.SetBool("isDie", true);
        }

        DistFromEnd = Vector3.Distance(transform.position, EndPoint.position);

        if(DistFromEnd <= 1.6f)
        {
            GameManager.gameManager.ReachedGoal = true;
        }

    }

    void moveToFinalPos()
    {
        if (!GameManager.gameManager.isDead)
        {
            if (GameManager.gameManager.Scene1 && isPolice && isClothChnaged)
            {
                agent.speed = 4f;
                agent.SetDestination(EndPoint.position);
            }
            if (GameManager.gameManager.Scene2 && isDoctor && isClothChnaged)
            {
                agent.SetDestination(EndPoint.position);
                agent.speed = 4f;
            }
            if (GameManager.gameManager.Scene3 && isPerson && isClothChnaged)
            {
                agent.SetDestination(EndPoint.position);
                agent.speed = 4f;
            }

            if (GameManager.gameManager.overGrenade && !isCaughtByPolice && !NearByEnemies.GetComponent<_EnemyManager>().isDead)
            {
                triggerCollider.enabled = true;
                agent.SetDestination(EndPoint.position);
                agent.speed = 4f;
            }
            if (isCaughtByPolice)
            {
                /*transform.LookAt(NearByEnemies.transform.position);*/
                agent.speed = 0;
                StartCoroutine(dead());
            }
        }
       
    }

    IEnumerator dead()
    {
        yield return new WaitForSeconds(.8f);
        GameManager.gameManager.isDead = true;

        if (!isPolice && !isDoctor)
        {
            anime.SetBool("isDie", true);
        }
    }
    public void nextPointCheck()
    {        
        RaycastHit hit;

        if(Physics.SphereCast(transform.position, rediusOfNextPos, Vector3.zero, out hit, maxDistanceToDetetct, whatIsNextPoint) && NextPosition==null)
        {
            if(NextPosition==null)
                NextPosition = hit.transform;
        }

        /*if (Physics.SphereCast(transform.position, rediusOfNextPos, Vector3.zero, out hit, maxDistanceToDetetct, whatIsEnemy) && NearByEnemies==null)
        {
                NearByEnemies = hit.transform.gameObject;
        }*/

        /*if (DistFromDead <= 5 && )
            NearByEnemies.GetComponent<_EnemyManager>().detectedByPlayer = true;

        else if (DistFromDead > 5)
            NearByEnemies = null;*/
    }

    void CheckForEnemy()
    {
        hitColliders = Physics.OverlapSphere(transform.position, maxDistanceToDetetct, whatIsEnemy);

        foreach(Collider hitCol in hitColliders)
        {
            if (hitCol != NearByEnemies)
                NearByEnemies = null;

            if(NearByEnemies == null )
            {
                NearByEnemies = hitCol.transform.gameObject;
                NearByEnemies.GetComponent<_EnemyManager>().detectedByPlayer = true;
            }
        }
    }

    public void movement()
    {
        if (!GameManager.gameManager.isDead && !isClothChnaged)
        {
            if (NearByEnemies == null || !NearByEnemies.GetComponent<_EnemyManager>().isDead)
                agent.SetDestination(NextPosition.position);

            if (NearByEnemies != null && NearByEnemies.GetComponent<_EnemyManager>().isDead)
                agent.SetDestination(NearByEnemies.transform.position);

            if (NearByEnemies != null && NearByEnemies.GetComponent<_EnemyManager>().isDead)
            {
                //Destroy(NextPosition.gameObject, 0.5f);
                NextPosition = null;

            }
            clothChanging();
        }
    }

    
    void clothChanging()
    {
        if(NearByEnemies!=null)
            DistFromDead = Vector3.Distance(transform.position, NearByEnemies.transform.position);

        if(NearByEnemies != null && NearByEnemies.GetComponent<_EnemyManager>().isDead && DistFromDead <= 1f)
        {
            StartCoroutine(startRunning(runToEnd));
            //Change Cloth
            //Set next Position to End Position
        }

    }
    IEnumerator startRunning(float t)
    {
        if (!isPolice && NearByEnemies.GetComponent<_EnemyManager>().isPolice)
        {
            yield return new WaitForSeconds(0.5f);
            Spwan.Play();
            isHitman = false;
            isPolice = true;
            yield return new WaitForSeconds(t);
            isClothChnaged = true;
        }
        if (!isDoctor && NearByEnemies.GetComponent<_EnemyManager>().isDoctor)
        {
            yield return new WaitForSeconds(0.5f);
            Spwan.Play();
            isHitman = false;
            isPolice = false;
            isDoctor = true;
            yield return new WaitForSeconds(t);
            isClothChnaged = true;
        }
        if (!isPerson && NearByEnemies.GetComponent<_EnemyManager>().isPerson)
        {
            yield return new WaitForSeconds(0.5f);
            Spwan.Play();
            isHitman = false;
            isPolice = false;
            isDoctor = false;
            isPerson = true;
            yield return new WaitForSeconds(t);
            isClothChnaged = true;
        }

    }
    void AnimationSetup()
    {
        if (isHitman)
        {
            anime = charecters[0].GetComponent<Animator>();
            charecters[0].SetActive(true);
            charecters[1].SetActive(false);
            charecters[2].SetActive(false);
            charecters[3].SetActive(false);
        }


        if (isPolice)
        {
            anime = charecters[1].GetComponent<Animator>();
            charecters[1].SetActive(true);
            charecters[0].SetActive(false);
            charecters[2].SetActive(false);
            charecters[3].SetActive(false);
        }

        if (isDoctor)
        {
            anime = charecters[2].GetComponent<Animator>();
            charecters[2].SetActive(true);
            charecters[1].SetActive(false);
            charecters[0].SetActive(false);
            charecters[3].SetActive(false);
        }

        if (isPerson)
        {
            anime = charecters[3].GetComponent<Animator>();
            charecters[3].SetActive(true);
            charecters[1].SetActive(false);
            charecters[2].SetActive(false);
            charecters[0].SetActive(false);
        }
    }

    void lookAtPlayer()
    {
        if (agent.velocity.magnitude > 0)
        {
            anime.SetBool("run", true);
            GameManager.gameManager.CameraTransition.Play("Cam2");
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }else if (agent.velocity.magnitude == 0 && !GameManager.gameManager.isDead)
        {
            anime.SetBool("run", false);
            GameManager.gameManager.CameraTransition.Play("Cam1");
        }
    }

    void MouseCheck()
    {
        if (Input.GetMouseButton(0) && agent.velocity.magnitude == 0)
        {
            GameManager.gameManager.isMousePressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            GameManager.gameManager.isMousePressed = false;
        }
    }
    void projectileActivator()
    {
        if (GameManager.gameManager.isMousePressed && !GameManager.gameManager.overGrenade)
        {
            RotatorScript.enabled = true;
            anime.SetBool("throw",true);
            //GetComponent<_PlayerProjectile>().lineVisual.enabled = true;
            
        }
        if (!GameManager.gameManager.isMousePressed)
        {
            RotatorScript.enabled = false;
            anime.SetBool("throw", false);
            
            //GetComponent<_PlayerProjectile>().lineVisual.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("grenade"))
        {
            GameManager.gameManager.isDead = true;
        }
    }
}
