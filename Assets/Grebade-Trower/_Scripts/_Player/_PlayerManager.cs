using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _PlayerManager : MonoBehaviour
{
    private NavMeshAgent agent;

    private float turnSmoothVelocity;
    private Animator anime;

    public List<GameObject> charecters = new List<GameObject>();

    public _PlayerRotator RotatorScript;

    public float rotationSmooth = 0.2f;
    public bool isHitman, isPolice, isDoctor, isPerson;
    

    public bool Temp;
    void Start()
    {
        AnimationSetup();
        agent = GetComponent<NavMeshAgent>();        
    }

    void Update()
    {
        lookAtPlayer();
        AnimationSetup();
        MouseCheck();
        projectileActivator();
        GameManager.gameManager._ProjectileRacePoint.position = RotatorScript.PointPos;
        if (Temp)
            agent.SetDestination(GameManager.gameManager.endPoint.position);
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
        }else if (agent.speed <= 0)
        {
            anime.SetBool("run", false);
            GameManager.gameManager.CameraTransition.Play("Cam1");
        }
    }

    void MouseCheck()
    {
        if (Input.GetMouseButton(0))
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
        if (GameManager.gameManager.isMousePressed && GameManager.gameManager.numberOfGrenadeUsed <= GameManager.gameManager.numberOfGrenade )
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
}
