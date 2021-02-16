using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D projectileBody;
    public int dmg = 10;
    public GameObject EnemyHand1;
    public GameObject EnemyHand2;

    // Start is called before the first frame update
    void Start()
    {
        //rigidbody move right according to speed
        projectileBody.velocity = transform.right * speed;
        Destroy(gameObject, 2f);
    }

    //when bullet hits enemy
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
		EnemyHand enemyHand = hitInfo.GetComponent<EnemyHand>();
		if (enemyHand != null)
		{
			//Debug.Log("Test");
			enemyHand.Damage(dmg);
		}
		//gets info of object we hit
		Debug.Log(hitInfo.name);
        //once we have an enemy time this will call it's take damage function
        //it takes damage
        

        Destroy(gameObject);
    }
}
