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

	public float damageTime;

	private float damageTimer;

	private bool Attack1Input;
	private bool Attack2Input;


	private bool AllowAttackInput;

	private bool AllowNewAttack;

	private AttackClass CurrentAttack;

	private int facing;

	private SpriteRenderer spriteRenderer;

	//Animator Stuff
	public CharacterController controller;
	public Animator animator;
	private void OnEnable()
	{
		entityScript = GetComponent<EntityScript>();
		entityScript.ComputeTargetVelocity += ComputeVelocity;
		spriteRenderer = GetComponent<SpriteRenderer>();
		AllowAttackInput = true;
		AllowNewAttack = true;
		Attack1Input = false;
	}


	// Update is called once per frame
	public void ComputeVelocity()
    {
		Vector2 move = Vector2.zero;
		move.x = Input.GetAxis("Horizontal") * MaxSpeed;
		animator.SetFloat("speed",Mathf.Abs(move.x));
		if(move.x > 0)
		{
			facing = 1;
		}else if(move.x < 0)
		{
			facing = -1;
		}

		if (Input.GetButtonDown("Jump")&&entityScript.Grounded)
		{
			Vector2 vec = entityScript.Velocity;
			vec.y = TakeOffSpeed;
			entityScript.Velocity = vec;
			animator.SetBool("IsJumping", true);
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
			if (Input.GetButton("Fire1"))
			{
				Attack1Input = true;
				animator.SetBool("isAttacking", true);
			}
			if (Input.GetButton("Fire2"))
			{
				Attack2Input = true;
				animator.SetBool("isShooting", false);
			}
		}

		if (AllowNewAttack)
		{
			//loop through all attacks and check if one is availible
			int priority = 0;
			AttackClass nextAttack = null;
			int linkValue = 0;
			if(CurrentAttack != null)
			{
				linkValue = CurrentAttack.MyLinkValue;
			}
			foreach(AttackClass attack in attacks)
			{
				if (attack.TakesInput(Attack1Input, Attack2Input, linkValue, priority))
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

		if(damageTimer > 0)
		{
			damageTimer -= Time.deltaTime;
			if(damageTimer <= 0)
			{
				spriteRenderer.color = Color.white;
			}
		}

		spriteRenderer.flipX = facing == -1;
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

	public void EndAttack()
	{
		CurrentAttack = null;
		AllowAttackInput = true;
		AllowNewAttack = true;
	}

	public Collider2D[] ThrowAttack(Vector2 AttackPosition, Vector2 AttackSize)
	{
		Debug.DrawLine(new Vector3(transform.position.x + AttackPosition.x * facing - AttackSize.x / 2, transform.position.y + AttackPosition.y), new Vector3(transform.position.x + AttackPosition.x * facing + AttackSize.x / 2, transform.position.y + AttackPosition.y),Color.red,5);
		return Physics2D.OverlapBoxAll(new Vector2(transform.position.x+AttackPosition.x*facing,transform.position.y+AttackPosition.y), AttackSize, 0f, enemylayer);
	}

	public void Damage(int damageAmount)
	{
		if (damageTimer <= 0)
		{
			damageTimer = damageTime;
			spriteRenderer.color = Color.red;
			//deal damage
		}
	}
}
