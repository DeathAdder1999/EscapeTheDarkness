using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    [SerializeField] private Transform[] obstaclesLeftLine;
    [SerializeField] private Transform[] obstaclesMidLine;
    [SerializeField] private Transform[] obstaclesRightLine; 
    [SerializeField] private GameObject[] obstaclePrefabs;

    private void Awake() {
        SetUpObstacles();
    }

    private void SetUpObstacles() {
        SpawnObstacles(obstaclesLeftLine, 3, 0.5f);
        SpawnObstacles(obstaclesRightLine, 3, 0.5f);
        SpawnObstacles(obstaclesMidLine, 5, 0.6f);

    }

    private void SpawnObstacles(Transform[] spawns, int limit, float chanceBound) {
        int obstaclesSpawned = 0;

        for (int i = 0; i < spawns.Length; i++) {
            Transform spawn = spawns[i];
            
            if(CameraController.WaveNum == 0 && i == 0) {
                continue;
            }

            float activationChance = Random.Range(0.0f, 1.0f);

            if(obstaclesSpawned >= limit) {
                break;
            }

            if (activationChance < chanceBound) {
                int randomIndex = Random.Range(0, obstaclePrefabs.Length);
                GameObject prefabToSpawn = obstaclePrefabs[randomIndex];

                GameObject obstacle = Instantiate(prefabToSpawn, new Vector3(spawn.position.x - 1.2f, 0.1f, spawn.position.z), spawn.transform.rotation) as GameObject;
                obstacle.transform.parent = spawn.transform; //In order for the obstacles to be deleted with the environment;

                obstaclesSpawned++;
            }
        }
    }
}
