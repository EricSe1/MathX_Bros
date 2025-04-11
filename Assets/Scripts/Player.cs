using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jumping;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Text interactEText;

    public GameObject popupGameOver; // Panel Game Over
    public Button menuButton;        // Bouton pour retourner au menu
    public Button restartButton;     // Bouton pour redémarrer le niveau

    private float horizontalMove;
    private SpriteRenderer sr;
    private Animator animator;

    private bool isGrounded;
    private bool isJumping;
    private bool isNearCoffre = false; // Vérifie si le joueur est proche d'un coffre
    private bool isDead = false; // Vérifie si le joueur est mort
    private bool isNearPorte = false; // Vérifie si le joueur est proche d'une porte

    private Coffre coffreProche; // Référence vers le coffre à proximité
    private Porte porteProche; // Référence vers la porte à proximité
    private int clésRequises; // Nombre de clés nécessaires pour ouvrir une porte
    private int totalCleScore = 0; // Score total des clés collectées

    public Collider2D debouteCollider; // Référence vers le collider du joueur
    public Collider2D mortCollider; // Référence vers le collider du joueur

    public GameObject popupVie;
    public GameObject popupCle;
    public GameObject popupBouton;

    private void Start()
    {
        Time.timeScale = 1;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Définir le nombre de clés requises en fonction du niveau actuel
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            clésRequises = 1;

        }
        else if (sceneName == "Level2")
        {
            clésRequises = 2;
            PlayerPrefs.SetInt("Level2Active", 1); // Verrouiller le niveau 2
            PlayerPrefs.Save();
        }
        else if (sceneName == "Level3")
        {
            clésRequises = 3;
            PlayerPrefs.SetInt("Level2Active", 1); // Verrouiller le niveau 2
            PlayerPrefs.SetInt("Level3Active", 1); // Verrouiller le niveau 3
            PlayerPrefs.Save();
        }
        // Ajoutez d'autres niveaux si nécessaire
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

    public void DesactiverCanvasAvecTag(string tag)
    {
        GameObject canvas = GameObject.FindGameObjectWithTag(tag);
        if (canvas != null)
        {
            canvas.SetActive(false); // Désactive l'objet entier
            Debug.Log($"Canvas avec le tag '{tag}' désactivé.");
        }
        else
        {
            Debug.LogWarning($"Aucun Canvas trouvé avec le tag '{tag}'.");
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
                Debug.Log("Coffre trouvé, on lance l'interaction !");
                coffreProche.StartInteraction();
            }
            else
            {
                Debug.LogWarning("Pas de coffre trouvé !");
            }
        }

        if (totalCleScore >= clésRequises)
        {
            DesactiverCanvasAvecTag("Cadenas"); // Désactiver le cadenas
        }

        if (porteProche != null)
        {
            Debug.Log($"Interaction avec la porte - Clés collectées : {totalCleScore}, Clés requises : {clésRequises}");

            if (totalCleScore >= clésRequises)
            {
                interactEText.text = "E pour ouvrir";

                if (isNearPorte && Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Ouverture de la porte...");
                    interactEText.text = "Ouverture de la porte...";

                    // Sauvegarder la progression si le joueur termine le niveau 1
                    if (SceneManager.GetActiveScene().name == "Level1")
                    {
                        PlayerPrefs.SetInt("Level2Active", 1); // Débloque le niveau 2
                        PlayerPrefs.Save(); // Sauvegarde les préférences
                        SceneManager.LoadScene("Level2");
                    }

                    if (SceneManager.GetActiveScene().name == "Level2")
                    {
                        PlayerPrefs.SetInt("Level3Active", 1); // Débloque le niveau 3
                        PlayerPrefs.Save(); // Sauvegarde les préférences
                        SceneManager.LoadScene("Level3");
                    }

                    if (SceneManager.GetActiveScene().name == "Level3")
                    {
                        SceneManager.LoadScene("Menu"); // Retour au menu
                    }

                    SceneManager.LoadScene(porteProche.prochainNiveau); // Charge le niveau suivant
                }
            }
            else
            {
                interactEText.text = "Pas assez de clés !";
                Debug.Log("Pas assez de clés pour ouvrir la porte !");
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
            Debug.Log("Coffre détecté : " + other.name); // Ajoutez ce log

            isNearCoffre = true;
            coffreProche = other.GetComponent<Coffre>(); // Stocke le script du coffre
            coffreProche.setCléScore(totalCleScore); // Met à jour le score des clés dans le coffre

            if (interactEText != null) // Vérifie si le label est assigné
            {
                interactEText.text = "E pour ouvrir"; // Met à jour le texte
                Debug.Log("Texte interactEText mis à jour pour le coffre.");
            }
            else
            {
                Debug.LogWarning("Le label interactEText n'est pas assigné !");
            }
        }

        if (other.CompareTag("Porte"))
        {
            Debug.Log($"Porte détectée : {other.name} - Clés collectées : {totalCleScore}, Clés requises : {clésRequises}");

            porteProche = other.GetComponent<Porte>();

            if (porteProche != null)
            {
                if (totalCleScore >= clésRequises)
                {
                    interactEText.text = "E pour ouvrir";
                    isNearPorte = true;
                    Debug.Log("Le joueur peut ouvrir la porte.");
                }
                else
                {
                    interactEText.text = "Pas assez de clés !";
                    isNearPorte = false; // Empêche l'accès à la porte
                    Debug.Log("Le joueur n'a pas assez de clés pour ouvrir la porte.");
                }
            }
        }

        if (other.CompareTag("Vide")) // Si le joueur tombe dans le vide
        {
            Debug.Log("Le joueur est tombé dans le vide !");
            ShowGameOver(); // Affiche l'écran de Game Over
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Coffre"))
        {
            Debug.Log("Coffre quitté : " + other.name); // Ajoutez ce log
            isNearCoffre = false;
            coffreProche = null; // On oublie le coffre
            if (interactEText != null) // Vérifie si interactEText est assigné
            {
                interactEText.text = ""; // Efface le texte d'interaction
            }
        }

        if (other.CompareTag("Porte"))
        {
            interactEText.text = "";
            Porte porte = other.GetComponent<Porte>();
            if (porte != null && porte == porteProche) // Vérifie si c'est bien la porte proche
            {
                Debug.Log("Porte quittée : " + other.name);
                isNearPorte = false;
                porteProche = null; // On oublie la porte
                if (interactEText != null) // Vérifie si interactEText est assigné
                {
                    interactEText.text = ""; // Efface le texte d'interaction
                }
            }
        }
    }

    public void Dead()
    {
        if (isDead) return; // Évite d'appeler plusieurs fois Dead()

        isDead = true; // Le joueur est mort
        animator.SetBool("isDead", isDead); // Déclenche l'animation de mort

        Debug.Log("Le joueur est mort !");

        // Désactiver les mouvements
        rb.linearVelocity = Vector2.zero; // Arrête le joueur
        //rb.isKinematic = true; // Empêche les forces physiques
        this.enabled = false; // Désactive le script Player pour bloquer les inputs
        debouteCollider.enabled = false; // Désactive le collider du joueur
        mortCollider.enabled = true; // Active le collider de mort
        if (popupBouton != null || popupVie != null || popupCle != null)
        {
            popupBouton.SetActive(false); // Affiche le bouton d'interaction
            popupVie.SetActive(false); // Affiche le canvas de vie
            popupCle.SetActive(false); // Affiche le canvas de clé
        }
        else
        {
            Debug.LogWarning("Un ou plusieurs objets sont manquants dans le script Player.");
        }

        PlayerPrefs.DeleteKey("Level2Active");
        PlayerPrefs.DeleteKey("Level3Active");
        PlayerPrefs.Save(); // Sauvegarde les préférences
    }

    // Méthode pour afficher l'écran de Game Over
    public void ShowGameOver()
    {
        Debug.Log("Game Over !");
        //Détruire les levels
        PlayerPrefs.DeleteKey("Level2Active");
        PlayerPrefs.DeleteKey("Level3Active");
        PlayerPrefs.Save(); // Sauvegarde les préférences

        

        popupGameOver.SetActive(true); // Affiche le panel Game Over

        menuButton.interactable = true;
        menuButton.onClick.RemoveAllListeners(); // Évite les doublons
        menuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu")); // Retour au menu

        restartButton.onClick.RemoveAllListeners(); // Évite les doublons
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name)); // Redémarre le niveau
    }

    public void AddCleScore(int score)
    {
        totalCleScore += score; // Ajoute le score des clés collectées
        //coffreProche.setCléScore(totalCleScore); // Met à jour le score des clés dans le coffre
        Debug.Log("Score total des clés mis à jour : " + totalCleScore);

    }

    
}