using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumping;
    public LayerMask groundLayer;
    public Transform groundCheck;

    private float horizontalMove;
    private SpriteRenderer sr;
    private Animator animator;

    private bool isGrounded;
    private bool isJumping;
    private bool isNearCoffre = false; // Vérifie si le joueur est proche d'un coffre

    private Coffre coffreProche; // Référence vers le coffre à proximité

    public Transform clePosition; // L’endroit sur la tête du joueur

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Mettre à jour isGrounded dans FixedUpdate
        isGrounded = IsGrounded();

        // Appliquer le mouvement horizontal
        rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);

        // Définir la direction du personnage
        if (horizontalMove > 0) sr.flipX = false;
        else if (horizontalMove < 0) sr.flipX = true;

        // Si le personnage touche le sol après un saut
        if (isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
                animator.ResetTrigger("JumpTrigger"); // Réinitialise l'animation de saut
            }

            animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); // Animation de course ou d'arrêt
        }
        else
        {
            animator.SetFloat("Speed", 0); // Animation stable en l'air
        }

        // Mettre à jour l'état "IsGrounded" dans l'Animator
        animator.SetBool("IsGrounded", isGrounded);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // Vérifie si le saut est déclenché et si le personnage est au sol
        if (context.started && isGrounded && !isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumping); // Applique la vélocité de saut
            isGrounded = false; // Le joueur n'est plus au sol
            isJumping = true;   // Le joueur est en train de sauter
            animator.SetTrigger("JumpTrigger"); // Déclenche l'animation de saut
        }
    }

    private void Update()
    {
        // On met à jour isGrounded à chaque frame
        isGrounded = IsGrounded();

       if (isNearCoffre && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interaction avec le coffre"); // 👈 tu dois voir ça
            if (coffreProche != null)
            {
                Debug.Log("Coffre trouvé, on lance l'équation !");
                coffreProche.StartEquation();
            }
            else
            {
                Debug.LogWarning("Pas de coffre trouvé !");
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMove = context.ReadValue<Vector2>().x;
    }

    private bool IsGrounded()
    {
        // Augmentez la hauteur de la capsule si nécessaire (par exemple, 0.2f au lieu de 0.1f)
        Collider2D collider = Physics2D.OverlapCapsule(
            groundCheck.position, 
            new Vector2(0.5f, 0.2f), // Augmentez la hauteur ici
            CapsuleDirection2D.Vertical, 
            0, 
            groundLayer
        );

        // Retourne true uniquement si un objet autre que le joueur est détecté
        return collider != null && collider.gameObject != gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Coffre"))
    {
        isNearCoffre = true;
        coffreProche = other.GetComponent<Coffre>(); // Stocke le script du coffre
    }
}

    private void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Coffre"))
    {
        isNearCoffre = false;
        coffreProche = null; // On oublie le coffre
    }
}
}