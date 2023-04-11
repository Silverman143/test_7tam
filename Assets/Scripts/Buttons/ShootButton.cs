using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace com.Test_7tam
{
    public class ShootButton : MonoBehaviour, IPointerDownHandler
    {
        public UnityEvent OnShootButtonDown = new UnityEvent();
        public static ShootButton Instance;

        private void Awake()
        {
            if (Instance != this)
            {
                Instance = this;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnShootButtonDown.Invoke();
        }
    }
}
