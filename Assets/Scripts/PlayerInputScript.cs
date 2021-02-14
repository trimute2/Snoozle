using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityScript))]
public class PlayerInputScript : MonoBehaviour
{
	public float MaxSpeed = 7;
	public float TakeOffSpeed = 7;
	private EntityScript entityScript;


	public AttackClass[] attacks;

	public LayerMask enemylayer;

	private bool Attack1Input;
	private bool Attack2Input;


	private bool AllowAttackInput;

	private bool AllowNewAttack;

	private AttackClass CurrentAttack;

	private void OnEnable()
	{
		entityScript = GetComponent<EntityScript>();
		entityScript.ComputeTargetVelocity += ComputeVelocity;
		AllowAttackInput = true;
		Attack1Input = false;
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

	public void Update()
	{
		if(CurrentAttack != null)
		{
			CurrentAttack.Update(Time.deltaTime, this);
		}

		if (AllowAttackInput)
		{
			//buffer attack inputs
			if (Input.GetButton("fire"))
			{
				Attack1Input = true;
			}
			if (Input.GetButton("fire2"))
			{
				Attack2Input = true;
			}
		}

		if (AllowNewAttack)
		{
			//loop through all attacks and check if one is availible
			int priority = 0;
			AttackClass nextAttack = null;
			if(CurrentAttack != null)
			{
				foreach (AttackClass attack in CurrentAttack.linkedAttacks)
				{
					if (attack.TakesInput(Attack1Input, Attack2Input, priority))
					{
						nextAttack = attack;
						priority = nextAttack.Priority;
					}
				}
			}
			foreach(AttackClass attack in attacks)
			{
				if (attack.TakesInput(Attack1Input, Attack2Input, priority))
				{
					nextAttack = attack;
					priority = nextAttack.Priority;
				}
			}
			//if an attack is availible start it
			if(nextAttack != null)
			{
				StartAttack(nextAttack);
			}
		}
	}

	public void StartAttack(AttackClass attack)
	{
		CurrentAttack = attack;
		CurrentAttack.StartAttack();
		Attack1Input = false;
		Attack2Input = false;
	}

	public void UpdateAttackInputVariables(bool allowInput, bool allowAttack)
	{
		AllowAttackInput = allowInput;
		AllowNewAttack = allowAttack;
	}

	public Collider2D[] ThrowAttack(Vector2 AttackPosition, Vector2 AttackSize)
	{
		return Physics2D.OverlapBoxAll(AttackPosition+new Vector2(transform.position.x,transform.position.y), AttackSize, 0f, enemylayer);
	}
}
