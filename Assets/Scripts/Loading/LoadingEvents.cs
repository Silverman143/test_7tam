using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public static partial class LoadingEvents
    {
        public static readonly GameEvent<string, int> OnRoomLoaded = new();
        public static readonly GameEvent<string, string> OnUserConnected = new();
        public static readonly GameEvent<string> OnUserDisconnected = new();
    }
}
