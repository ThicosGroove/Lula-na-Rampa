using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class TorcidaBehaviour : MonoBehaviour
{
    [SerializeField] float Speed;
    private bool hasWin;

    private void OnEnable()
    {
        GameplayEvents.Win += WinBehaviour;
    }

    private void OnDisable()
    {
        GameplayEvents.Win -= WinBehaviour;
    }

    void Update()
    {
        if (hasWin)
        {
            transform.Translate(Vector3.back * Speed * Time.deltaTime);

            if (transform.position.z <= -700f)
            {
                hasWin = false;
                Speed = 0f;
            }
        }
    }

    void WinBehaviour()
    {
        hasWin = true;
    }
}
