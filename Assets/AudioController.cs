﻿using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    #region awake function

	void Awake() 
    {
        AudioListener.volume = 0f;

	}

    #endregion

    #region public functions

    public void Mute(bool on)
    {
        if (on)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }

    #endregion
}
