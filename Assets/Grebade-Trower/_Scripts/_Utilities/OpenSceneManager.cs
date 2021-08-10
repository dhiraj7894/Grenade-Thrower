using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSceneManager : MonoBehaviour
{
    public static OpenSceneManager OSM;
    public GameObject[] Scenes;
    public bool[] sceneData;

    public Animator UIAnime;


    private void Start()
    {
        OSM = this;
    }
    public void SceneLoader()
    {
        if (sceneData[0])
        {
            Scenes[0].SetActive(true);
            Scenes[1].SetActive(false);
            Scenes[2].SetActive(false);
        }
        if (sceneData[1])
        {
            Scenes[1].SetActive(true);
            Scenes[0].SetActive(false);
            Scenes[2].SetActive(false);
        }
        if (sceneData[2])
        {
            Scenes[2].SetActive(true);
            Scenes[1].SetActive(false);
            Scenes[0].SetActive(false);
        }
    }
}
