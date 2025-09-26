using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform gfxTransform;
    [SerializeField] private float rotationSpeed = 10f;

    public void HandleRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        gfxTransform.rotation = Quaternion.Slerp(gfxTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}