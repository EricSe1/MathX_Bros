using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumping;
    public LayerMask groundLayer;
    public Transform groundCheck;
    float horizontalMove;
    SpriteRenderer sr;
    Animator animator;

    private bool isGrounded; // Variable pour gérer l'état du sol

    /// Start is called before the first frame update | FR : Début de la première image
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Mise à jour du mouvement horizontal
        rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);

        // Animation lors de l'interaction avec les touches
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); // Animation de la marche
        animator.SetBool("IsGrounded", isGrounded); // Animation du saut : Si on est au sol
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMove = context.ReadValue<Vector2>().x;

        // Lorsque le joueur se déplace, on applique l'axe où il se déplace, exemple : gauche (il regarde à gauche) ou droite (il regarde à droite)
        if (horizontalMove > 0)
        {
            sr.flipX = false; // Regarde à droite
        }
        else if (horizontalMove < 0)
        {
            sr.flipX = true; // Regarde à gauche
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded) // Vérification si le joueur est bien au sol avant de sauter
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumping); // Application du saut (force verticale)
            isGrounded = false; // On indique qu'on n'est plus au sol pendant le saut
        }
    }

    // Vérifie si le joueur est au sol
    private void Update()
    {
        isGrounded = IsGrounded(); // On met à jour l'état du sol à chaque frame
    }

    bool IsGrounded()
    {
        // Vérification de l'état du sol avec une détection via une capsule
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.1f, 0.1f), CapsuleDirection2D.Vertical, 0, groundLayer);
    }
}