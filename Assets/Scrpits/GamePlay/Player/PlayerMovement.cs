using UnityEngine;
using System.Collections;
using GameEvents;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float jumpHeight = 80f;
    [SerializeField] private float rollingDelay = 1f;
    [SerializeField] private float gravity = 13f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    public int desiredLane = 1; // 0 = esquerda, 1 = meio, 2 = direita
    private Vector3 targetPosition;
    public bool isRolling = false;  //Usado no player animation
    public bool isGrounded = false;  //Usado no player animation
    private float verticalVelocity = 0f;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float rotation = 40;
    private Transform gfxTransform;

    private Rigidbody rb;
    private CapsuleCollider coll;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;
    private Vector3 initialPos;


    private void Awake()
    {
        initialPos = transform.position;
        gfxTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        originalColliderHeight = coll.height;
        originalColliderCenter = coll.center;
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
    }

    public void HandleMovement()
    {
        targetPosition = new Vector3((desiredLane - 1) * Const.LANE_DISTANCE, transform.position.y, transform.position.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, slideSpeed * Time.deltaTime);
    }

    public void MoveToLane(int direction)
    {
        desiredLane += direction;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2); // Garante que o jogador fique entre as 3 faixas
        
    }

    public void HandleRotation(int dir)
    {
        float rot = dir > 0 ? rotation : -rotation;
        Quaternion targetRotation = Quaternion.Euler(0, rot, 0);
        gfxTransform.rotation = Quaternion.Slerp(gfxTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        StartCoroutine(RotationCoroutine());

    }

    private IEnumerator RotationCoroutine()
    {
        yield return new WaitForSeconds(0.3f);

        gfxTransform.rotation = Quaternion.Slerp(gfxTransform.rotation, Quaternion.Euler(new Vector3(0,0,0)), rotationSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;

            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // Calcula a velocidade inicial necessária para o pulo
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, verticalVelocity, rb.linearVelocity.z); // Aplica a velocidade no eixo Y
        }
    }

    public void Roll()
    {
        if (!isRolling && isGrounded)
        {
            isRolling = true;
            StartCoroutine(RollCoroutine());
        }
    }

    private IEnumerator RollCoroutine()
    {
        coll.height = originalColliderHeight / 2;
        coll.center = originalColliderCenter / 2;

        yield return new WaitForSeconds(rollingDelay);

        coll.height = originalColliderHeight;
        coll.center = originalColliderCenter;

        isRolling = false;
    }

    private void HandleGravity()
    {
        if (!IsGrounded())
        {
            verticalVelocity += Physics.gravity.y * gravity * Time.deltaTime; // Aplica a gravidade personalizada
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, verticalVelocity, rb.linearVelocity.z); // Atualiza a velocidade no eixo Y
        }
        else if (rb.linearVelocity.y <= 0.5)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
        }
    }

    public bool IsGrounded()
    {

        // Verifica se há colisão com o chão usando Physics.OverlapSphere
        return isGrounded = Physics.CheckSphere(groundCheck.position, 1f, groundMask);
    }

    public void ResetPosition()
    {
        transform.position = initialPos;
        desiredLane = 1;
    }

    public void StopMovement()
    {
        slideSpeed = 0;
    }

    public void WinMovement()
    {
        desiredLane = 1;
        targetPosition = initialPos;
        transform.position = Vector3.Lerp(transform.position, targetPosition, slideSpeed * Time.deltaTime);
    }
}
