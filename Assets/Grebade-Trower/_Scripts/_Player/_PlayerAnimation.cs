 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerAnimation : MonoBehaviour
{
    private Animator anime;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }
    public void Launch()
    {
        FindObjectOfType<_PlayerProjectile>().Launch();
        GameManager.gameManager.numberOfGrenadeUsed++;
    }
}
