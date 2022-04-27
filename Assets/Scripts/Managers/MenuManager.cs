using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    [SerializeField] private GameObject settingsPanel;
    private float originalVolume;

    public GameObject[] menus;

    private void Awake() {
  
        settingsPanel.SetActive(false);
    }

    public void SettingsClicked() {
        settingsPanel.SetActive(true);
    }

    private void Start() {
        SpawnMenuEnvironment();
    }

    private void SpawnMenuEnvironment() {
        int randomIndex = Random.Range(0, menus.Length);
        GameObject menuToSpawn = menus[randomIndex]; 

        Instantiate(menuToSpawn, menuToSpawn.transform.position, menuToSpawn.transform.rotation);
    }
}
