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

	protected float currentRotation = 0f;

	protected Transform target;
	protected CharacterInformation characterInfo;

    // Start is called before the first frame update
    void Start()
    {
		target = GameObject.FindGameObjectWithTag(GameConstants.TAG_Player).transform;
		GameMaster.Instance.AddToList(this);
    }

	public virtual void PlayTurn()
	{
		Vector2 direction = GetPlayerDirection();
		RotateTowardPlayer(direction);
		AttemptMove(direction);
	}

	protected Vector2 GetPlayerDirection()
	{
		Vector2 direction = Vector2.zero;

		if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
		{
			direction.y = target.position.y > transform.position.y ? 1 : -1;
		}
		else
		{
			direction.x = target.position.x > transform.position.x ? 1 : -1;
		}
		
		return direction;
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
		else if (hit.transform.tag == GameConstants.TAG_Player)
		{
			hit.transform.GetComponent<PlayerScript>().Hit(damage);
			GameMaster.Instance.ActivateEnemies();
		}
	}

	public void RotateTowardPlayer(Vector2 direction)
	{
		float rotationZ = currentRotation;

		if (direction.y != 0)
		{
			rotationZ = direction.y == 1 ? 0f : 180f;
		}
		else
		{
			rotationZ = direction.x == 1 ? -90f : 90f;
		}

		if (rotationZ != currentRotation)
		{
			currentRotation = rotationZ;
			transform.rotation = Quaternion.Euler(0f,0f,currentRotation);
		}
	}

	public void Hit(float damageTaken)
	{
		if (!active)
		{
			Kill();
			return;
		}

		health -= damageTaken;
		if (health <= 0)
		{
			health = 0;
			Kill();
		}
	}

	public void Kill()
	{
		GameMaster.Instance.RemoveFromList(this);
		Destroy(gameObject);
	}
}
