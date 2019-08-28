using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public float damage = 4f;
	public float speed = 6f;
	public Vector2 direction;

    // Update is called once per frame
    void Update()
    {
		transform.Translate(Vector2.up * speed* Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == GameConstants.TAG_Enemy)
		{
			
			return;
		}

		if (collision.tag == GameConstants.TAG_Player)
		{
			collision.GetComponent<PlayerScript>().Hit(damage);
		}

		Destroy(gameObject);
	}
}
