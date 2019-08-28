﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterScript : MonoBehaviour
{
	public string characterName = "";
	public float health = 10f;
	public Weapon currentWeapon;
	public LayerMask blockingLayer;

	protected CharacterInformation characterInfo = new CharacterInformation();

	public virtual void Start()
	{
		characterInfo.ownCollider = GetComponent<BoxCollider2D>();
	}

	public virtual void AttemptMove(Vector2 direction)
	{
		Vector3 originalPos = transform.position;
		Transform targetCell = CheckNextCell(direction, blockingLayer);

		if (targetCell == null)
		{
			transform.position = new Vector3(originalPos.x + direction.x, originalPos.y + direction.y, originalPos.z);
		}
		else
		{
			CantMove(targetCell);
		}
	}

	public abstract void CantMove(Transform target);

	public virtual void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			health = 0;
		}
	}

	protected Transform CheckNextCell(Vector2 direction, LayerMask mask)
	{
		RaycastHit2D hit;
		Vector2 start = transform.position;
		Vector2 end = start + direction;

		characterInfo.ownCollider.enabled = false;

		hit = Physics2D.Linecast(start, end, mask);

		characterInfo.ownCollider.enabled = true;

		return hit.transform;
	}
}