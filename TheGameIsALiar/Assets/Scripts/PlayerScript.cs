using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float health = 10f;
	public float damage = 5f;

	public CameraShake cameraShake;
	public float shakeDuration = 0.15f;
	public float shakeMagnitude = .2f;
	public bool canPlay = true;
	public LayerMask blockingLayer;

	private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!GameMaster.Instance.playerTurn) return;

		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (input.x == 0 && input.y == 0)
		{
			canPlay = true;
		}
		else
		{
			AttemptMove(input);
		}
	}

	void AttemptMove(Vector2 input)
	{
		if (!canPlay)
		{
			return;
		}

		Vector3 originalPos = transform.position;
		RaycastHit2D hit;
		Vector2 start = transform.position;
		Vector2 end = start + input;

		transform.GetComponent<BoxCollider2D>().enabled = false;

		hit = Physics2D.Linecast(start, end, blockingLayer);

		transform.GetComponent<BoxCollider2D>().enabled = true;

		if (hit.transform == null)
		{
			transform.position = new Vector3(originalPos.x + input.x, originalPos.y + input.y, originalPos.z);
		}
		else
		{
			if (hit.transform.tag == "Enemy")
			{
				hit.transform.GetComponent<EnemyScript>().Hit(damage);
			}

			StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
		}

		GameMaster.Instance.playerTurn = false;
		canPlay = false;
	}

	public void Hit(float damageTaken)
	{
		health -= damageTaken;
		if (health <= 0)
		{
			health = 0;
		}
	}
}
