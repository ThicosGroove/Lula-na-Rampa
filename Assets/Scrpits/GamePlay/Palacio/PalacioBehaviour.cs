using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PalacioBehaviour : MonoBehaviour
{
    [SerializeField] float palacioSpeed;
    private bool hasWin;

    private void OnEnable()
    {
        GameplayEvents.Win += WinBehaviour;
    }

    private void OnDisable()
    {
        GameplayEvents.Win -= WinBehaviour;        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasWin)
        {
            transform.Translate(Vector3.back * palacioSpeed * Time.deltaTime);

            if (transform.position.z <= 300f)
            {
                hasWin = false;
                palacioSpeed = 0f;

                GameplayEvents.OnReachPalace();
            }
        }
    }

    void WinBehaviour()
    {
        hasWin = true;
    }
}
