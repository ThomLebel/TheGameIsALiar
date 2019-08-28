using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
	public bool active;
	public float actionTime = 0.2f;

	private float currentRotation = 0f;

	private Transform target;

    // Start is called before the first frame update
    public override void Start()
	{
		base.Start();
		target = GameObject.FindGameObjectWithTag(GameConstants.TAG_Player).transform;
		GameMaster.Instance.AddToList(this);
		currentWeapon.weapon.owner = GameConstants.TAG_Enemy;
	}

	public void PlayTurn()
	{
		characterInfo.origin = transform;
		characterInfo.direction = GetPlayerDirection();
		characterInfo.target = CheckNextCell(characterInfo.direction, blockingLayer);

		RotateTowardPlayer(characterInfo.direction);

		if (currentWeapon.weapon.ammo > 0)
		{
			currentWeapon.Attack(characterInfo, GameConstants.TAG_Player);
		}
		else
		{
			AttemptMove(characterInfo.direction);
		}
	}

	public override void AttemptMove(Vector2 direction)
	{
		base.AttemptMove(direction);
	}

	public override void CantMove(Transform target)
	{
		if (target.tag == GameConstants.TAG_Player)
		{
			Attack();
		}
	}

	private void Attack()
	{
		currentWeapon.Attack(characterInfo, GameConstants.TAG_Player);
		GameMaster.Instance.ActivateEnemies();
	}


	private Vector2 GetPlayerDirection()
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

	private void RotateTowardPlayer(Vector2 direction)
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

	public override void TakeDamage(float damageTaken)
	{
		if (!active)
		{
			Kill();
			return;
		}

		base.TakeDamage(damageTaken);
		if (health <= 0)
		{
			Kill();
		}
	}

	public void Kill()
	{
		GameMaster.Instance.RemoveFromList(this);
		Destroy(gameObject);
	}
}
