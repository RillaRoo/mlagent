using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoal : Agent
{
	[SerializeField] private Transform targetTransform;
	[SerializeField] private List<GameObject> spikeList;
	public BufferSensorComponent buff;
	public override void OnEpisodeBegin()
	{
		for (int i = 0; i < spikeList.Count; i++)
		{
			Destroy(spikeList[i]);
		}
		spikeList.Clear();
		transform.localPosition = Vector3.zero;
	}
	public override void CollectObservations(VectorSensor sensor)
	{
		sensor.AddObservation(transform.position);
		foreach (GameObject spike in spikeList)
		{
			Transform spikeTransform = spike.GetComponent<Transform>();
			Rigidbody spikeRigidbody = spike.GetComponent<Rigidbody>();
			float[] obs = { transform.position.x - spikeTransform.position.x, spikeTransform.position.x, spikeTransform.position.y, spikeRigidbody.velocity.y };
			buff.AppendObservation(obs);
		}
	}

	public override void OnActionReceived(ActionBuffers actions)
	{
		base.OnActionReceived(actions);
		float moveZ = actions.ContinuousActions[0];
		float moveSpeed = 7f;
		transform.position += new Vector3(0, 0, moveZ) * Time.deltaTime * moveSpeed;
	}

	public override void Heuristic(in ActionBuffers actionsOut)
	{
		//base.Heuristic(actionsOut);
		var continuousActions = actionsOut.ContinuousActions;
		continuousActions[0] = Input.GetAxisRaw("Horizontal");

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
	public void AddSpike(GameObject spike)
	{
		spikeList.Add(spike);
	}
	public void RemoveSpike(GameObject spike)
	{
		spikeList.Remove(spike);
	}
}
