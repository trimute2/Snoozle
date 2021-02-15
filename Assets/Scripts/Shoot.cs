using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform arrowPoint;
    public GameObject arrowPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShootArrow();
        }
    }
    void ShootArrow ()
    {
        //shooting logic
        Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation);
    }
}
