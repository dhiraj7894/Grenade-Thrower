using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerProjectile : MonoBehaviour
{
    public Transform shootPoint;

    public LineRenderer lineVisual;
    public GameObject TrajectorySpwan;
    public GameObject Radar;
    public GameObject[] Points;

    public int lineSegment;

    


    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
        lineVisual.positionCount = lineSegment;
        Points = new GameObject[lineSegment];
        Radar.SetActive(false);
        for (int i = 0; i < lineSegment; i++)
        {
            Points[i] = Instantiate(TrajectorySpwan, transform.position, Quaternion.identity);
            Points[i].transform.parent = transform.GetChild(1);
        }
    }

    private void Update()
    {
        if(!GameManager.gameManager.overGrenade)
            LaunchPorjectile();        
    }
    Vector3 Vo;
    
    public void LaunchPorjectile()
    {
        Vo = CalculateVelocity(GameManager.gameManager._ProjectileRacePoint.position, shootPoint.position, 1f);
        if (GameManager.gameManager.isMousePressed)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            Radar.SetActive(true);
            Visualize(Vo);
        }

        if (!GameManager.gameManager.isMousePressed)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            Radar.SetActive(false);
        }
    }
    public void Launch()
    {
        Rigidbody grenadeObj = Instantiate(GameManager.gameManager.grenade, shootPoint.position, Quaternion.identity);
        grenadeObj.velocity = Vo;
    }

    Vector3 CalculateVelocity (Vector3 target, Vector3 origin,float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    void Visualize(Vector3 vo)
    {
        for (int i = 0; i < Points.Length; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, i / (float)lineSegment);
            Points[i].transform.position = pos;
            lineVisual.SetPosition(i, pos); 
        }

    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;
        result.y = sY;
        return result;
    }

}
