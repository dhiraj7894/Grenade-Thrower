using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public List<Material> _enemyMaterials = new List<Material>();



    void Start()
    {
        levelManager = this;
    }

}
