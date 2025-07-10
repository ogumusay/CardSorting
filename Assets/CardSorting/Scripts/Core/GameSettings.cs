using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSorting
{
    public class GameSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
