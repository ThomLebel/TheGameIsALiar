using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : EnemyScript
{
	public int arrows = 3;
	public GameObject arrow;

	public override void PlayTurn()
	{
		if (arrows > 0)
			Shoot();
		else
			base.PlayTurn();
	}

	void Shoot()
	{
		Vector2 direction = GetPlayerDirection();
		RotateTowardPlayer(direction);

		if (target.position.x != transform.position.x && target.position.y != transform.position.y)
		{
			AttemptMove(direction);
			return;
		}

		GameObject shot = Instantiate(arrow, transform.position, transform.rotation);
		shot.GetComponent<Arrow>().direction = direction;
		GameMaster.Instance.ActivateEnemies();

		arrows--;
	}
}
