using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightController : MonoBehaviour {
    [SerializeField] private Material yellow;
    [SerializeField] private Material yellow_light;
    private MeshRenderer renderer;

    private void Awake() {
        renderer = GetComponent<MeshRenderer>();
        StartCoroutine(Blink());
    }

    private IEnumerator Blink() {

        while (true) {
            
            if (renderer != null) {
                renderer.materials = new Material[7] { renderer.materials[0], renderer.materials[1], yellow, renderer.materials[3], renderer.materials[4], renderer.materials[5], renderer.materials[6]};
                yield return new WaitForSeconds(0.5f);

                renderer.materials = renderer.materials = new Material[7] { renderer.materials[0], renderer.materials[1], yellow_light, renderer.materials[3], renderer.materials[4], renderer.materials[5], renderer.materials[6] };
                yield return new WaitForSeconds(0.5f);
            }   
        }
    }
}
