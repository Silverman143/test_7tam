using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.Test_7tam
{
    public class RoomSizeSelector : MonoBehaviour
    {
        public Button _button2;
        public int RoomSize => _roomSize;
        public static RoomSizeSelector Instance;

        private int _roomSize = 2;

        private void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _button2.Select();
            SetSize(_roomSize);
        }

        public void SetSize(int value)
        {
            _roomSize = value;
        }
    }
}

