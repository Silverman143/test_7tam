using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIHandler : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;
    [SerializeField] private GameObject _shootButton;

    [SerializeField] private GameObject _finishGamePanel;
    [SerializeField] private GameObject _winGameObj;
    [SerializeField] private GameObject _gameOverObj;

    [SerializeField] private TextMeshProUGUI _positionTMP;



    private void ShowEndGamePanel(int place)
    {
        _joystick.SetActive(false);
        _shootButton.SetActive(false);

        _finishGamePanel.SetActive(true);

        if(place == 1)
        {
            _winGameObj.SetActive(true);
            _gameOverObj.SetActive(false);

        }
        else
        {
            _winGameObj.SetActive(false);
            _gameOverObj.SetActive(true);
        }

        _positionTMP.text = $"{place} th";
    }
}
