using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{
	[SerializeField] private Transform targetTransform;
	public override void OnEpisodeBegin()
	{
		transform.position = Vector3.zero;
	}
	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		sensor.AddObservation(targetTransform.position);
	}
	public override void OnActionReceived(float[] vectorAction)
	{
		float moveZ = vectorAction[0];
		float moveSpeed = 7f;
		transform.position += new Vector3(0, 0, moveZ) * Time.deltaTime * moveSpeed;
	}

	public override void Heuristic(float[] actionsOut)
	{
		actionsOut[0] = Input.GetAxisRaw("Horizontal");
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Spike>(out Spike spike))
		{
			SetReward(-1f);
			EndEpisode();
		}

		if (other.TryGetComponent<Wall>(out Wall wall))
		{
			SetReward(-1f);
			EndEpisode();
		}
	}
}
