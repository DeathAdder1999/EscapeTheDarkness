using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour {
    public const int DEADZONE = 100; //px

    private static bool created; 
    private static bool tap, swipeLeft, swipeRight;
    private Vector2 swipeDelta, startTouch; //startTouch is the place where the tap starts, change position of the drag

    public static bool Tap { get { return tap; } }
    public static bool SwipeLeft { get { return swipeLeft; } }
    public static bool SwipeRight { get { return swipeRight; } }

    private void Awake() {
        if (created) {
            DestroyImmediate(this.gameObject);
        }else {
            created = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update() {
        //Reset all the values first
        Reset();

        //Check for input

        //All simultaneous touches besides the first one are ignored , touches[0] is used
        if(Input.touches.Length != 0) { //if there are touches 
            if(Input.touches[0].phase == TouchPhase.Began) {
                tap = true;
                startTouch = Input.mousePosition;
            }else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                startTouch = swipeDelta = Vector2.zero;
            }
        }

        //Calculate the distance

        swipeDelta = Vector2.zero;

        if(startTouch != Vector2.zero) {
            if(Input.touches.Length != 0) {
                swipeDelta = Input.touches[0].position - startTouch;
            }
        }

        //Check if the touch is a swipe. That is if the touch is beyond the deadzone(100 pixels)
        if(swipeDelta.magnitude > DEADZONE) {
            //Confirmed swipe
            float x = swipeDelta.x;

            if (x < 0)  //Left
                swipeLeft = true;
            else if (x > 0)
                swipeRight = true;
            else
                return;
        }

    }

    private void Reset() {
        tap = swipeLeft = swipeRight = false;
    }
}
