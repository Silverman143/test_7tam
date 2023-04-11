using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Test_7tam
{
    public static partial class LobbyEvents
    {
        public static readonly GameEvent OnStartConnecting = new();
        public static readonly GameEvent OnConnError = new();
        public static readonly GameEvent OnConnectionComplete = new();
        public static readonly GameEvent OnLoadingStart = new();
        public static readonly GameEvent OnLoadingEnd = new();

        public static readonly GameEvent<string> OnCreateRoomButton = new();
        public static readonly GameEvent<string> OnJoinRoomButton = new();

    }

}


