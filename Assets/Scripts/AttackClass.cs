using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass 
{
	public int Priority;
	public int MyLinkValue;
	public int RequiredLinkValue;
	public bool takesAttackInput1;
	public bool takesAttackInput2;


	public Vector2 attackPosition;
	public Vector2 attackSize;

	public float blockAttackInputTime;
	public float blockNewAttackTime;
	public float damageTime;
	public float attackLength;

	public int damage;

	private float AttackTimer;

	private bool hasDamaged;
	
	public void Update(float deltaTime, PlayerInputScript player)
	{
		AttackTimer += deltaTime;
		player.UpdateAttackInputVariables(AttackTimer >= blockAttackInputTime, AttackTimer > -blockNewAttackTime);
		if(!hasDamaged && AttackTimer>= damageTime)
		{
			Collider2D[] hits = player.ThrowAttack(attackPosition, attackSize);
			foreach (Collider2D hit in hits)
			{
				HealthComponent healthComponent = hit.GetComponent<HealthComponent>();
				if (healthComponent != null)
				{
					healthComponent.Damage(damage, Vector2.zero);
					
				}
				EnemyHand enemyHand = hit.GetComponent<EnemyHand>();
				if(enemyHand != null)
				{
					Debug.Log("Test");
					enemyHand.Damage(1);
				}
			}
			hasDamaged = true;
		}

		if(AttackTimer >= attackLength)
		{
			player.EndAttack();
		}
	}
	
	public void StartAttack()
	{
		AttackTimer = 0;
		hasDamaged = false;
	}

	public bool TakesInput(bool AttackInput1, bool AttackInput2, int linkValue, int currentPriority)
	{
		bool canAttack = Priority > currentPriority&& (MyLinkValue == 0 || RequiredLinkValue == linkValue);
		
		if(takesAttackInput1 && !AttackInput1)
		{
			canAttack = false;
		}
		if(takesAttackInput2 && !AttackInput2)
		{
			canAttack = false;
		}

		return canAttack;
	}
}
