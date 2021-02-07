using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//bassed off of an old platfromer movement tutorial: https://learn.unity.com/tutorial/live-session-2d-platformer-character-controller#5c7f8528edbc2a002053b695
//and an old script i made based off said tutorial: https://github.com/trimute2/FightToTheTop/blob/master/Assets/None%20Plugins/Scripts/ComponentScripts/EntityControllerComp.cs
[RequireComponent(typeof(Rigidbody2D))]
public class EntityScript : MonoBehaviour
{

	public float minMoveDistance;

	[HideInInspector]
	public Vector2 targetVelocity;

	private Vector2 velocity;
	private Vector2 groundNormal;

	private ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


	private bool grounded;
	public bool Grounded
	{
		get
		{
			return grounded;
		}
	}

	//other components
	private Rigidbody2D rigidbody;

    void OnEnable()
    {
		rigidbody = GetComponent<Rigidbody2D>();
    }

	void Start()
	{
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void FixedUpdate()
	{
		velocity += Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		grounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;

		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

		Vector2 move = moveAlongGround * deltaPosition.x;

		Movement(move, false);

		move = Vector2.up * deltaPosition.y;

		Movement(move, true);
	}

	void Movement(Vector2 move, bool yMovement)
	{
		float distance = move.magnitude;

		if (distance > minMoveDistance)
		{
			int count = rigidbody.Cast(move, contactFilter, hitBuffer, distance + 0.01f);
			hitBufferList.Clear();
			for (int i = 0; i < count; i++)
			{
				hitBufferList.Add(hitBuffer[i]);
			}

			for (int i = 0; i < hitBufferList.Count; i++)
			{
				Vector2 currentNormal = hitBufferList[i].normal;
				if (currentNormal.y > 0.65f)
				{
					grounded = true;
					if (yMovement)
					{
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if (projection < 0)
				{
					velocity = velocity - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList[i].distance - 0.01f;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}
		rigidbody.position = rigidbody.position + move.normalized * distance;
	}
}
