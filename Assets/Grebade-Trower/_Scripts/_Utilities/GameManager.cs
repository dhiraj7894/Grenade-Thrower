using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Transform endPoint;
    public Transform _ProjectileRacePoint;
    public Animator CameraTransition;

    public Rigidbody grenade;
    public int ContToKill;

    public int killed;

    public int numberOfGrenade = 2;
    public int numberOfGrenadeUsed;


    public bool isMousePressed;

    void Start()
    {
        gameManager = this;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
