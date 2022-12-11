using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    private void GoToPlace(Vector3 newPlace)
    {
        transform.Translate(newPlace);
    }
}
