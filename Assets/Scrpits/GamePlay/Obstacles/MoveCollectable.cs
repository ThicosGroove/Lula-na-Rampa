using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCollectable : MoveBase
{
    [Header("Star parameters")]

    [Header("positions parameters")]
    [SerializeField] float freqPosition;
    [SerializeField] float ampPosition;

    [Header("rotation parameters")]
    [SerializeField] float freqRotationZ;
    [SerializeField] float ampRotationZ;

    protected override void DieBehaviour()
    {
        throw new System.NotImplementedException();
    }

    protected override void MoveBehaviour()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time * freqPosition) * ampPosition), transform.position.z);

        float rotationZ = Mathf.Sin(Time.time * freqRotationZ) * ampRotationZ;
        Vector3 Rotation = new Vector3(0f, 0f, rotationZ);
        transform.Rotate(Rotation, Space.Self);
    }
}
