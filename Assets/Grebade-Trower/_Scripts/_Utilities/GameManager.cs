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
    public bool ReachedGoal;
    public bool Scene1, Scene2, Scene3;

    public bool oneEnemyDead = false;

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
        if (isDead )
        {
            StartCoroutine(zoomOverDeadPlayer());
        }
        levelChanger();
    }

    IEnumerator zoomOverDeadPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        CameraTransition.Play("Zoom");
    }


    IEnumerator UILoader(float t)
    {
        yield return new WaitForSeconds(t);
        OpenSceneManager.OSM.UIAnime.Play("Fade");
    }

    void levelChanger()
    {
        if (ReachedGoal && !Scene3)
            StartCoroutine(UILoader(0.2f));

        if (ReachedGoal && Scene1)
        {
            OpenSceneManager.OSM.sceneData[0] = false;
            OpenSceneManager.OSM.sceneData[1] = true;
        }
        if (ReachedGoal && Scene2)
        {
            OpenSceneManager.OSM.sceneData[1] = false;
            OpenSceneManager.OSM.sceneData[2] = true;
        }
    }


}
