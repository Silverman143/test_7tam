using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace com.Test_7tam
{
    public class UserCardHandlerMono : MonoBehaviour
    {
        [SerializeField] private protected TextMeshProUGUI _userNameTMP;

        public virtual void SetName(string name)
        {
            _userNameTMP.text = name;
        }
    }
}


