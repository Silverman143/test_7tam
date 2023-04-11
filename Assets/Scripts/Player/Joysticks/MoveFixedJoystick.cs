using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public class MoveFixedJoystick : FixedJoystick
    {
        public static new MoveFixedJoystick Instance;

        public override void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }
        }
    }
}
