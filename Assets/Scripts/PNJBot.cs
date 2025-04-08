using UnityEngine;

public class PNJBot : MonoBehaviour
{
    public float speed = 2f;
    public float startDelay = 2f; // Délai avant de commencer à bouger
    public float moveDuration = 60f; // Durée de déplacement dans une direction avant de changer
    private float timer = 0f;
    private bool hasStarted = false;
    private bool movingRight = true; // Indique si le PNJ se déplace vers la droite

    private void OnEnable()
    {
        timer = 0f;
        hasStarted = false; // Réinitialise l'état de démarrage
        movingRight = true; // Par défaut, le PNJ commence à aller vers la droite
    }

    void Update()
    {
        // Attendre le délai de commencement avant de commencer à bouger
        if (!hasStarted)
        {
            timer += Time.deltaTime;
            if (timer >= startDelay)
            {
                hasStarted = true;
                timer = 0f; // Réinitialise le timer pour le mouvement
            }
            return; // Ne pas continuer tant que le délai n'est pas écoulé
        }

        // Déplacement et changement de direction après une durée
        timer += Time.deltaTime;
        if (timer >= moveDuration)
        {
            Flip(); // Inverse la direction visuelle
            movingRight = !movingRight; // Change la direction
            timer = 0f; // Réinitialise le timer
        }

        // Déplacement dans la direction actuelle
        Move(movingRight ? Vector3.right : Vector3.left);
    }

    void Move(Vector3 direction)
    {
        // Déplace le PNJ dans la direction donnée
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void Flip()
    {
        // Inverse l'échelle sur l'axe X pour changer la direction de la tête
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Inverse simplement l'axe X
        transform.localScale = localScale;
    }
}