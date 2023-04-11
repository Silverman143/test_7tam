using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public class ShootFixedJoystick : FixedJoystick
    {
        public static new ShootFixedJoystick Instance;

        public override void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }
        }

    }
}
