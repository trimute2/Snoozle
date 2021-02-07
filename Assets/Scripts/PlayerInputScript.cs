using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityScript))]
public class PlayerInputScript : MonoBehaviour
{
	public float MaxSpeed = 7;
	public float TakeOffSpeed = 7;
	private EntityScript entityScript;

	private void OnEnable()
	{
		entityScript = GetComponent<EntityScript>();
		entityScript.ComputeTargetVelocity += ComputeVelocity;
	}


	// Update is called once per frame
	public void ComputeVelocity()
    {
		Vector2 move = Vector2.zero;
		move.x = Input.GetAxis("Horizontal") * MaxSpeed;


		if (Input.GetButtonDown("Jump")&&entityScript.Grounded)
		{
			Vector2 vec = entityScript.Velocity;
			vec.y = TakeOffSpeed;
			entityScript.Velocity = vec;
		}

		entityScript.targetVelocity = move;

	}
}
