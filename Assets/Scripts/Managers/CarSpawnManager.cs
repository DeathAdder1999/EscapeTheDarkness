using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnManager : MonoBehaviour {
    [SerializeField] private GameObject[] cars;
    private float maxTimeToSpawn;
    private float minTimeToSpawn;

    private bool stopped;

    private void Awake() {

        if (Inventory.construction > 0) this.gameObject.SetActive(false); //Disable CarSpawnManager

        maxTimeToSpawn = 1.0f;
        minTimeToSpawn = 3.0f;

        if(Inventory.fixTheLight > 0) {
            maxTimeToSpawn = 1.0f;
            minTimeToSpawn = 1.5f;
        }

        spawnedCars = new CarController[cars.Length * 5];

        for (int i = 0; i < spawnedCars.Length; i++) {
           spawnedCars[i] = Instantiate(cars[i % 3], new Vector3(30 * i, 30, -30), new Quaternion(0.0f, 0.1f, 0.0f, 0.0f)).GetComponent<CarController>();
         }
    }

    private void Start() {
        stopped = false;
        StartCoroutine(SpawnCars());
    }

    private void Update() {
        if (ScoreManager.playerIsDead && !stopped) {
            StopAllCoroutines();
            stopped = true;
        }
    }

    //OPTIMIZED
    private CarController[] spawnedCars;
    private short[] carsUsed;

    private IEnumerator SpawnCars() {

        yield return new WaitForSeconds(0.5f);

        while (true) {
            int randomIndex = Random.Range(0, spawnedCars.Length);
            CarController car = spawnedCars[randomIndex];
            
            while(car.onRoad) {
                if (randomIndex < spawnedCars.Length - 1)
                    randomIndex++;
                else
                    randomIndex = (++randomIndex) % spawnedCars.Length;
                car = spawnedCars[randomIndex];
               
            }

            if (Environment.GetCarSpawn() == null) yield return null;
            else {
                Transform spawn = Environment.GetCarSpawn();
                car.transform.position = spawn.position;
                car.onRoad = true;
                car.ResetVelocity();
                //TODO create an array of CarControllers instead
            }


            float randomWaitTime = Random.Range(minTimeToSpawn, maxTimeToSpawn);

            yield return new WaitForSeconds(randomWaitTime);
        }
    }

    


}
