using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform arrowPoint;
    public GameObject arrowPrefab;
	private PlayerInputScript PIS;

	private void Start()
	{
		PIS = GetComponent<PlayerInputScript>();
	}

	// Update is called once per frame
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShootArrow();
        }
    }
    void ShootArrow ()
    {
		//shooting logic
		Vector3 pos = arrowPoint.position;
		Quaternion rot = Quaternion.identity;
		if (PIS.Facing == -1)
		{
			float x = transform.position.x - pos.x;
			pos.x += 2 * x;
			rot= Quaternion.Euler(0, 0, 180);
		}

		//Quaternion rot = Quaternion.identity;
        Instantiate(arrowPrefab, pos, rot);
    }
}
