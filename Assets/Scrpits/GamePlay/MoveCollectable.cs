using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCollectable : MoveBase
{
    [SerializeField] float amp;
    [SerializeField] float freq;

    protected override void MoveBehaviour()
    {
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.time * freq) * amp), transform.position.z);

        // + transform.position.y
    }
}
