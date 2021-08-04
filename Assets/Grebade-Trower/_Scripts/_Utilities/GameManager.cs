using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Transform _ProjectileRacePoint;
    public Animator CameraTransition;

    public Rigidbody grenade;
    public int ContToKill;

    public int killed;

    public int numberOfGrenade = 2;
    public int numberOfGrenadeUsed;


    public bool isMousePressed;
    public bool overGrenade;
    public bool Scene1, Scene2, Scene3;

    [Header("")]
    public bool isDead;

    void Start()
    {
        gameManager = this;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfGrenadeUsed >= numberOfGrenade)
        {
            overGrenade = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
