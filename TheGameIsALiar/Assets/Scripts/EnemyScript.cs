using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : CharacterScript
{
	public bool active;
	public float actionTime = 0.2f;
	public GameObject healthBarPrefab;

	private GameObject enemyHealthBar;
	private float currentRotation = 0f;
	private Transform target;

    // Start is called before the first frame update
    public override void Start()
	{
		base.Start();
		target = GameObject.FindGameObjectWithTag(GameConstants.TAG_Player).transform;
		GameMaster.Instance.AddToList(this);
		currentWeapon.weapon.owner = GameConstants.TAG_Enemy;
		enemyHealthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform);
		healthBar = enemyHealthBar.transform.Find("bgHealthBar/healthBarProgress").GetComponent<Image>();
		enemyHealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
		//Debug.Log(enemyHealthBar.transform.position);
		enemyHealthBar.SetActive(false);
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
		enemyHealthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
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
		if (health == maxHealth)
		{
			enemyHealthBar.SetActive(true);
		}

		base.TakeDamage(damageTaken);
		UpdateHealthBar();
		if (health <= 0)
		{
			Kill();
		}
	}

	public void Kill()
	{
		GameMaster.Instance.RemoveFromList(this);
		Destroy(enemyHealthBar);
		Destroy(gameObject);
	}
}
