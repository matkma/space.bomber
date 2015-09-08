using UnityEngine;
using System.Collections;

public class CameraSetup : MonoBehaviour 
{
	void Start() 
    {
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
	}
}
