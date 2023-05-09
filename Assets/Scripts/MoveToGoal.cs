using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoal : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject floor;
    [SerializeField] private List<GameObject> spikeList;
    public BufferSensorComponent buff;
    private Vector3 initialPos;
    private void Start()
    {
        initialPos = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        for (int i = 0; i < spikeList.Count; i++)
        {
            Destroy(spikeList[i]);
        }
        spikeList.Clear();
        transform.localPosition = initialPos;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        foreach (GameObject spike in spikeList)
        {
            Transform spikeTransform = spike.transform;
            Rigidbody spikeRigidbody = spike.GetComponent<Rigidbody>();
            float[] obs = { transform.localPosition.z - spikeTransform.localPosition.z, spikeTransform.localPosition.z, spikeTransform.localPosition.y, spikeRigidbody.velocity.y };
            buff.AppendObservation(obs);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        float moveZ = actions.ContinuousActions[0];
        float moveSpeed = 7f;
        transform.localPosition += new Vector3(0, 0, moveZ) * Time.deltaTime * moveSpeed;
        if (transform.localPosition.y < floor.transform.localPosition.y - 2)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
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
