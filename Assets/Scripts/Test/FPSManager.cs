using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour {
    [SerializeField] private Text text;
	
	// Update is called once per frame
	void Update () {
        int fps = (int) (1.0f / Time.smoothDeltaTime);
        text.text = fps + "";
	}
}
