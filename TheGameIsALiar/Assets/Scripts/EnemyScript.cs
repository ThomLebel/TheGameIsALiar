using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	public float health;
	public float damage;
	public bool active;
	public float actionTime = 0.2f;

	public LayerMask blockingLayer;

	private Transform player;

    // Start is called before the first frame update
    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

	public virtual void PlayTurn()
	{
		Move();
	}

    protected virtual void Move()
	{
		Vector2 direction = Vector2.zero;
		
		if (Mathf.Abs(player.position.x - transform.position.x) < float.Epsilon)

			direction.y = player.position.y > transform.position.y ? 1 : -1;

		else
			direction.x = player.position.x > transform.position.x ? 1 : -1;

		AttemptMove(direction);
	}

	public void AttemptMove(Vector2 direction)
	{
		Vector3 originalPos = transform.position;
		RaycastHit2D hit;
		Vector2 start = transform.position;
		Vector2 end = start + direction;

		transform.GetComponent<BoxCollider2D>().enabled = false;

		hit = Physics2D.Linecast(start, end, blockingLayer);

		transform.GetComponent<BoxCollider2D>().enabled = true;

		if (hit.transform == null)
		{
			transform.position = new Vector3(originalPos.x + direction.x, originalPos.y + direction.y, originalPos.z);
		}
		else if (hit.transform.tag == "Player")
		{
			hit.transform.GetComponent<PlayerScript>().Hit(damage);
		}
	}

	public void Hit(float damageTaken)
	{
		Debug.Log("hit !");
		health -= damageTaken;
		if (health <= 0)
		{
			health = 0;
		}
	}
}
