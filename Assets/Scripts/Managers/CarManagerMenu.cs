using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManagerMenu : MonoBehaviour {

    public GameObject[] cars;
    private CarController[] spawnedCars;

    private void Awake() {
        spawnedCars = new CarController[cars.Length * 3];

        for (int i = 0; i < spawnedCars.Length; i++) {
            spawnedCars[i] = Instantiate(cars[i % 3], new Vector3(-10 * i, -10, -10), new Quaternion(0.0f, 0.1f, 0.0f, 0.0f)).GetComponent<CarController>();
        }
    }

    private void Start() {
        StartCoroutine(SpawnCars());
    }

    private IEnumerator SpawnCars() {

        yield return new WaitForSeconds(0.5f);

        while (true) {
            int randomIndex = Random.Range(0, spawnedCars.Length);
            CarController car = spawnedCars[randomIndex];

            while (car.onRoad) {
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

            float randomWaitTime = Random.Range(2.0f, 4.0f);

            yield return new WaitForSeconds(randomWaitTime);
        }
    }
}
