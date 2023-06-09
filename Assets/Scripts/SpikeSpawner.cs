using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpikeSpawner : MonoBehaviour
{
    public MoveToGoal moveToGoal;
    public GameObject spikePrefab;
    public float spawnRate = 1f; // Number of spikes to spawn per second
    public float speed = 5f; // Speed of the spikes
    public GameObject spike;

    private float lastSpawnTime;

    private void Update()
    {
        //Check if it's time to spawn a new spike
        if (Time.time - lastSpawnTime > 2f / spawnRate)
        {
            
            //Instantiate a new spike with a random z position, but the same x position as the platform
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-5f, 5f));
            GameObject spike = Instantiate(spikePrefab, spawnPosition, Quaternion.identity);
            moveToGoal.AddSpike(spike);
            //Set the spike's speed
            Spike spikeMovement = spike.GetComponent<Spike>();
            spikeMovement.moveToGoal = moveToGoal;
            //if (spikeMovement != null)
            //{
            //    spikeMovement.speed = speed;
            //}
            //Reset the last spawn time
            lastSpawnTime = Time.time;
        }
    }
}
