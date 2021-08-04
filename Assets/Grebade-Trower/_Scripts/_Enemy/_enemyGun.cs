using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _enemyGun : MonoBehaviour
{
    public GameObject Gun;

    public void enableGun()
    {
        Gun.SetActive(true);
    }
}
