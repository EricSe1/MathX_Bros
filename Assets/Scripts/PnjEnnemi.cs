using UnityEngine;

public class PnjEnnemi : MonoBehaviour
{
    public float speed = 2f; // Vitesse de déplacement de l'ennemi
    public float leftDuration = 3f; // Temps passé à aller vers la gauche
    public float rightDuration = 6f; // Temps passé à aller vers la droite (double du temps gauche)
    public GameObject particleEffect; // Effet de particules à déclencher lors de l'élimination

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool movingRight = false; // Commence par aller à gauche
    private float timer; // Timer pour gérer le changement de direction
    private float currentDuration; // Durée actuelle pour le déplacement

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Initialiser le timer et la durée
        timer = leftDuration;
        currentDuration = leftDuration;
    }

    private void Update()
    {
        // Déplacement horizontal
        rb.linearVelocity = new Vector2(movingRight ? speed : -speed, rb.linearVelocity.y);

        // Réduire le timer
        timer -= Time.deltaTime;

        // Si le timer atteint 0, changer de direction
        if (timer <= 0)
        {
            Flip();
            // Alterner entre la durée gauche et droite
            currentDuration = movingRight ? rightDuration : leftDuration;
            timer = currentDuration; // Réinitialiser le timer
        }
    }

    private void Flip()
    {
        movingRight = !movingRight; // Inverse la direction
        sr.flipX = !sr.flipX; // Retourne le sprite horizontalement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                // Vérifie si le joueur saute sur l'ennemi
                if (collision.contacts[0].normal.y < -0.5f)
                {
                    EliminerEnnemi(); // Élimine l'ennemi
                    collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 5f); // Fait rebondir le joueur
                }
                else
                {
                    // Le joueur le headshot
                    player.Dead();
                    player.ShowGameOver();
                    // Le pnj est en pause
                    rb.linearVelocity = Vector2.zero; // Arrête le mouvement de l'ennemi
                    rb.isKinematic = true; // Rend l'ennemi immobile
                    rb.simulated = false; // Désactive la simulation physique
                    enabled = false; // Désactive le script de l'ennemi
                    
                    
                }
            }
        }
    }

    private void EliminerEnnemi()
    {
        // Déclenche l'effet de particules
        if (particleEffect != null)
        {
            particleEffect.SetActive(true); // Désactive l'effet de particules pour le réinitialiser
            Instantiate(particleEffect, transform.position, Quaternion.identity);
        }

        // Détruit l'ennemi
        Destroy(gameObject);
    }
}