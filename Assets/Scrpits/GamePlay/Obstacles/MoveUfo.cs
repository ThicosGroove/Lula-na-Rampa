using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUfo : MonoBehaviour
{
    [Header("UFO parameters")]

    [Header("positions parameters")]
    [SerializeField] float freqPosition;
    [SerializeField] float ampPosition;

    [Header("rotation parameters")]
    [SerializeField] float freqRotationZ;
    [SerializeField] float ampRotationZ;
    [SerializeField] float freqRotationY;
    [SerializeField] float ampRotationY;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * freqPosition) * ampPosition), transform.position.z);

        float rotationZ = Mathf.Sin(freqRotationZ) * ampRotationZ;
        float rotationY =  freqRotationY * ampRotationY;
        Vector3 Rotation = new Vector3(0f, rotationY, rotationZ);
        transform.Rotate(Rotation, Space.Self);
    }
 
}
