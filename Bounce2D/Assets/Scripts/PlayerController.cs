using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento en tierra")]
    public float speed = 5f;
    public float acceleration = 10f;
    public float jumpForce = 12f;
    public float friction = 1f;

    [Header("Agua")]
    public float waterGravityScale = 0.5f;   // gravedad más suave en agua

    [Tooltip("Capa donde está el agua (se rellena en Start con la layer 'Water')")]
    public LayerMask waterMask;

    [Tooltip("Radio del círculo para detectar agua")]
    public float waterCheckRadius = 0.3f;

    [Tooltip("Offset desde la posición del player para comprobar agua")]
    public Vector2 waterCheckOffset = new Vector2(0f, -0.3f);

    [Header("Suelo")]
    public float rayLength = 1f;
    public LayerMask rayMask;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;
    private float _input;
    private bool _grounded;

    private bool _inWater = false;
    private float _defaultGravityScale;

    // vidas / checkpoints
    private int _health = 5;
    private int _checkpoints = 0;
    private int _maxCheckpoints = 0;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            Debug.LogError("PlayerController: NO hay Rigidbody2D en este GameObject.");
            return;
        }

        _defaultGravityScale = _rigidbody.gravityScale;

        // Coge todos los checkpoints de la escena
        Checkpoint[] checkponts = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        _maxCheckpoints = checkponts.Length;
        _checkpoints = _maxCheckpoints;

        // Forzamos que el waterMask sea la capa "Water"
        waterMask = LayerMask.GetMask("Water");
    }


    private void Update()
    {
        // 1) DETECTAR SI ESTOY EN AGUA (OverlapCircle)
        Vector2 waterCheckPos = (Vector2)transform.position + waterCheckOffset;
        bool wasInWater = _inWater;
        bool check = Physics2D.OverlapCircle(waterCheckPos, waterCheckRadius, waterMask);
        _inWater = check;

        // Cambiar gravedad solo al entrar / salir del agua
        if (_inWater && !wasInWater)
        {
            _rigidbody.gravityScale = waterGravityScale;
            Debug.Log("ENTRO en agua");
        }
        else if (!_inWater && wasInWater)
        {
            _rigidbody.gravityScale = _defaultGravityScale;
            Debug.Log("SALGO de agua");
        }

        // 2) Input horizontal
        _input = UnityEngine.Input.GetAxisRaw("Horizontal");

        // 3) Grounded solo fuera del agua
        if (!_inWater)
        {
            _grounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, rayMask);
        }
        else
        {
            _grounded = false;
        }

        // 4) SALTO
        // - Si NO estoy en agua → salto normal solo si estoy en suelo
        // - Si SÍ estoy en agua → salto infinito
        if (UnityEngine.Input.GetButtonDown("Jump"))
        {
            if (!_inWater && _grounded)
            {
                // Salto normal en tierra
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if (_inWater)
            {
                // Salto infinito en agua
                Vector2 vel = _rigidbody.linearVelocity;
                vel.y = 0f; // opcional: para que cada impulso se sienta claro
                _rigidbody.linearVelocity = vel;

                _rigidbody.AddForce(Vector2.up * jumpForce * 0.7f, ForceMode2D.Impulse);
            }
        }
    }


    private void FixedUpdate()
    {
        _velocity = _rigidbody.linearVelocity;

        // Movimiento horizontal igual siempre (tierra y agua)
        if (_input != 0)
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, _input * speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, friction * Time.fixedDeltaTime);
        }

        _rigidbody.linearVelocity = _velocity;
    }


    // ========= VIDA / RESPAWN =========

    public void Respawn()
    {
        if (Checkpoint.current != null)
        {
            transform.position = Checkpoint.current.transform.position;
            Time.timeScale = 1f;
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    public void Kill()
    {
        _health--;

        Debug.Log("Me han matado. Vida restante: " + _health);

        if (_health > 0)
        {
            Respawn();
        }
        else
        {
            Debug.Log("Game Over definitivo.");
        }
    }

    public int GetCheckpointCount()
    {
        return _maxCheckpoints;
    }

    public int GetCheckpointObtained()
    {
        return _checkpoints;
    }

    public void SetCheckpoint(Checkpoint chk)
    {
        _checkpoints = _checkpoints - 1;
        if (_checkpoints < 0)
            _checkpoints = 0;
    }

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }


    // (Opcional) ver el círculo de detección de agua en la escena
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector2 waterCheckPos = (Vector2)transform.position + waterCheckOffset;
        Gizmos.DrawWireSphere(waterCheckPos, waterCheckRadius);
    }
}
