using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _enemyGun : MonoBehaviour
{
    public GameObject Gun;
    public ParticleSystem PlayerPartical, GunSpark;


    private void Start()
    {
        PlayerPartical = GameObject.Find("Player").transform.GetChild(4).GetComponent<ParticleSystem>();
    }
    public void enableGun()
    {
        Gun.SetActive(true);
    }

    public void StartSpark()
    {
        PlayerPartical.Play();
        GunSpark.Play();
    }
}
