using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
	public GameObject spikePrefab;
	public MoveToGoal moveToGoal;
	//public float speed = 5f; //Speed of the spike
	public GameObject spike;
	
	private void Update()
	{
		//Move the spike downwards
		//transform.localPosition += Vector3.down * speed * Time.deltaTime;
	}

    private void OnCollisionEnter(Collision collision)
    {
		//Check if the spike collided with the bottom platform
		if (collision.gameObject.CompareTag("BottomPlatform"))
		{
			//Destroy the spike
			//GetComponent<MoveToGoal>();
			Destroy(gameObject);
			moveToGoal.RemoveSpike(spike);
			//Debug.Log("Spike destroyed");
		}
    }
}
