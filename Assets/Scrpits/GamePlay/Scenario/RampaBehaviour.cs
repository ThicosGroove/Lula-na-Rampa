using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampaBehaviour : MonoBehaviour
{
	public float vel = 0.1f;
	public Renderer quad;


	void Update()
	{
		Vector2 offset = new Vector2(0, vel * Time.deltaTime);
		quad.material.mainTextureOffset += offset;
	}
}