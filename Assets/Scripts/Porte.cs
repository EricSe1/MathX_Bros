using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Porte : MonoBehaviour
{
    public int clésRequises; // Nombre de clés nécessaires pour ouvrir la porte
    private bool isPlayerInRange = false; // Vérifie si le joueur est proche
    public string prochainNiveau = "Niveau2"; // Nom de la scène du prochain niveau
    public Text interactEText; // Texte pour afficher "E pour ouvrir"
    public GameObject player; // Référence au controle Button

    private Coffre coffreProche; // Référence vers le coffre à proximité

    private void Start()
    {
        // Définir le nombre de clés requises en fonction du niveau actuel
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level 1")
        {
            clésRequises = 1;
        }
        else if (sceneName == "Level 2")
        {
            clésRequises = 2;
        }
        else if (sceneName == "Level 3")
        {
            clésRequises = 3;
        }
        // Ajoutez d'autres niveaux si nécessaire
    }

    private void Update()
    {
        if (isPlayerInRange)
        {
            interactEText.text = "E pour ouvrir"; // Affiche le texte d'interaction
        }
        else
        {
            interactEText.text = ""; // Efface le texte d'interaction
        }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Player joueur = FindObjectOfType<Player>(); // Trouve le joueur dans la scène
            int cleScore = coffreProche != null ? coffreProche.GetCleScore() : 0; // Vérifie si coffreProche est null
            if (joueur != null && cleScore >= clésRequises)
            {
                Debug.Log("Porte ouverte, passage au niveau suivant !");
                SceneManager.LoadScene(prochainNiveau); // Charge le prochain niveau
            }
            else
            {
                Debug.Log("Pas assez de clés pour ouvrir la porte !");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Le joueur est proche de la porte.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactEText.text = ""; // Efface le texte d'interaction
            Debug.Log("Le joueur s'est éloigné de la porte.");
        }
    }
}