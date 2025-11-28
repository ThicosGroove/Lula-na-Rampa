using UnityEngine;
using System.Collections;
using GameEvents;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float slideSpeed = 15f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float rollingSpeed = 1f; // Duração do rolamento

    [Header("Physics Settings")]
    [SerializeField] private float gravity = 80f;
    [SerializeField] private float maxFallSpeed = -20f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.2f;

    public int desiredLane = 1;
    public bool isRolling = false;
    public bool isGrounded = false;
    private float verticalVelocity = 0f;

    [Header("Rotation Parameters")]
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float tiltAngle = 40f;
    private Transform gfxTransform;

    private Rigidbody rb;
    private CapsuleCollider coll;
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;
    private Vector3 initialPos;

    private void Awake()
    {
        initialPos = transform.position;
        if (transform.childCount > 0) gfxTransform = transform.GetChild(0);
        else gfxTransform = transform;

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        originalColliderHeight = coll.height;
        originalColliderCenter = coll.center;
    }

    // Método chamado pelo PlayerManager ou LevelManager
    public void UpdateSpeedMovement(float newSlideSpeed, float newJumpSpeed, float newRollSpeed)
    {
        slideSpeed = newSlideSpeed;
        jumpHeight = newJumpSpeed;
        rollingSpeed = newRollSpeed;
    }

    // Sobrecarga para chamar direto pelo LevelIndex se preferir
    public void LevelChanged(int levelIndex)
    {
        if (LevelManager.Instance != null)
        {
            var data = LevelManager.Instance.GetLevelData(levelIndex);
            if (data != null)
            {
                UpdateSpeedMovement(data.player_Slide_Speed, data.player_Jump_Height, data.player_Roll_Speed);
                gravity = data.player_Gravity;
            }
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        // 1. Calcula a velocidade lateral necessária (Local)
        float xLocalVelocity = CalculateLateralMovement();

        // 2. Calcula a gravidade/pulo (Y)
        float yVelocity = CalculateVerticalVelocity();

        // 3. CONVERSÃO MÁGICA: Transforma (X, 0, 0) Local em Velocidade Global
        // Isso garante que ele vá para a direita/esquerda DO BONECO, não do mundo.
        Vector3 localMoveVector = new Vector3(xLocalVelocity, 0, 0);

        // Se o boneco tem pai (trem/plataforma), usa a rotação do pai. Se não, usa a dele.
        Vector3 globalMoveVector = transform.parent != null
            ? transform.parent.TransformVector(localMoveVector)
            : transform.TransformVector(localMoveVector);

        // 4. Aplica ao Rigidbody
        // Usamos o X e Z calculados (para permitir curvas) e o Y da gravidade
        rb.linearVelocity = new Vector3(globalMoveVector.x, yVelocity, globalMoveVector.z);

        HandleRotation();
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    private float CalculateLateralMovement()
    {
        float targetX = (desiredLane - 1) * Const.LANE_DISTANCE;
        float currentX = transform.localPosition.x;
        float distance = targetX - currentX;

        // Zona morta para estabilidade
        if (Mathf.Abs(distance) < 0.05f)
        {
            Vector3 snapPos = transform.localPosition;
            snapPos.x = targetX;
            transform.localPosition = snapPos;
            return 0f;
        }

        return distance * slideSpeed;
    }

    private float CalculateVerticalVelocity()
    {
        // Pega a velocidade atual da física
        float currentY = rb.linearVelocity.y;

        if (isGrounded)
        {
            if (verticalVelocity <= 0)
            {
                verticalVelocity = 0f;
                return 0f; // No chão, sem força Y
            }
            else
            {
                return verticalVelocity; // Iniciando pulo
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.fixedDeltaTime;
            if (verticalVelocity < maxFallSpeed) verticalVelocity = maxFallSpeed;
            return verticalVelocity;
        }
    }

    private void HandleRotation()
    {
        float targetX = (desiredLane - 1) * Const.LANE_DISTANCE;
        float currentX = transform.localPosition.x;
        float diff = targetX - currentX;

        Quaternion targetRotation = Quaternion.identity;

        if (diff > 0.1f) targetRotation = Quaternion.Euler(0, tiltAngle, 0);
        else if (diff < -0.1f) targetRotation = Quaternion.Euler(0, -tiltAngle, 0);

        gfxTransform.rotation = Quaternion.Slerp(gfxTransform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void MoveToLane(int direction)
    {
        desiredLane += direction;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    public void Jump()
    {
        if (isGrounded && !isRolling)
        {
            verticalVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            isGrounded = false;
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

        // Correção: Verifica divisão por zero
        float waitTime = rollingSpeed > 0 ? (1f / rollingSpeed) : 1f;
        yield return new WaitForSeconds(waitTime);

        coll.height = originalColliderHeight;
        coll.center = originalColliderCenter;
        isRolling = false;
    }

    public void ResetPosition()
    {
        transform.position = initialPos;
        desiredLane = 1;
        verticalVelocity = 0;
        if (rb) rb.linearVelocity = Vector3.zero;
        if (gfxTransform) gfxTransform.rotation = Quaternion.identity;
    }

    public void StopMovement() { slideSpeed = 0; rb.linearVelocity = Vector3.zero; }

    public void WinMovement()
    {
        desiredLane = 1;
        Vector3 target = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, 5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathBarrier"))
        {
            ResetPosition();
        }
    }
}