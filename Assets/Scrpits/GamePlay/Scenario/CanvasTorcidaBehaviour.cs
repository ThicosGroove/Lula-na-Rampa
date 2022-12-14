using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class CanvasTorcidaBehaviour : MonoBehaviour
{
    [SerializeField] float torcidaSpeed;
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
            transform.Translate(Vector3.down * torcidaSpeed * Time.deltaTime);

            if (transform.position.z <= 0f)
            {
                hasWin = false;
                torcidaSpeed = 0f;
            }
        }
    }

    void WinBehaviour()
    {
        hasWin = true;
    }
}
