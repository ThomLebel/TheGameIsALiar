﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public GameObject player;
	public List<EnemyScript> enemies;
	public bool playerTurn = true;
	public bool enemiesTurn = false;
	public float turnDelay = 0.1f;
	

	public static GameMaster Instance;

	private void Awake()
	{
		Instance = this;
		enemies = new List<EnemyScript>();
	}

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindGameObjectWithTag(GameConstants.TAG_Player);
    }

    // Update is called once per frame
    void Update()
    {
		if (playerTurn || enemiesTurn)
		{
			return;
		}

		StartCoroutine(EnemiesTurn());
    }

	IEnumerator EnemiesTurn()
	{
		enemiesTurn = true;

		yield return new WaitForSeconds(turnDelay);

		if (enemies.Count == 0)
		{
			yield return new WaitForSeconds(turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++)
		{
			EnemyScript enemy = enemies[i];

			if (enemy.active)
			{
				enemy.PlayTurn();
				yield return new WaitForSeconds(enemy.actionTime);
			}
		}

		playerTurn = true;
		enemiesTurn = false;
	}

	public void AddToList(EnemyScript enemy)
	{
		enemies.Add(enemy);
	}

	public void RemoveFromList(EnemyScript enemy)
	{
		enemies.Remove(enemy);
	}

	public void ActivateEnemies()
	{
		foreach(EnemyScript enemy in enemies)
		{
			enemy.active = true;
		}
	}
}
