using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
	public float speed = 5f; // Speed of the spike

	private void Update()
	{
		// Move the spike downwards
		transform.localPosition += Vector3.down * speed * Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		// Check if the spike collided with the bottom platform
		if (other.CompareTag("BottomPlatform"))
		{
			// Destroy the spike
			Destroy(gameObject);
			Debug.Log("Spike destroyed");
		}
	}
}
