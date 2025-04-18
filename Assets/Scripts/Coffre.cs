using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public enum CoffreType
{
    Equation,   // Coffre qui génère des équations
    Expression, // Coffre qui génère des expressions mathématiques
    DifficileExpression // Coffre qui génère des expressions difficiles
}
public class Coffre : MonoBehaviour
{
    public CoffreType typeDeCoffre; // Définit le type de coffre
    public GameObject effetMagique; // Référence à l'effet magique (particules)
    public float fonduSpeed = 1f; // Vitesse de fondu pour le coffre
    private bool isPlayerInRange = false; // Vérifie si le joueur est proche
    private bool isOpened = false; // Vérifie si le coffre a été ouvert
    public Player joueur; // Référence au joueur
    public GameObject controlButtons; // Référence au controle Button

    public GameObject popupPanel;        // Le panel de la popup
    public Text equationText;            // Texte qui affiche l'équation
    public Text expressionText;          // Texte qui affiche l'expression (pour Coffre2)
    public Text scoreText;               // Texte qui affiche le score
    public InputField inputField;        // Champ de réponse
    public Button submitButton;          // Bouton valider
    public Button closeButton;           // Bouton pour fermer le panneau

    private SpriteRenderer sr; // SpriteRenderer du coffre pour le fondu
    private Player player; // Référence au script Player

    private int a, b, solution; // Variables pour les calculs
    private int score;
    private int objectif = 4;

    public int vie = 3;
    public int cléScore;

    public Text textCléScore;

    public GameObject coeur1;
    public GameObject coeur2;
    public GameObject coeur3;

    public GameObject coeurVIDE1;
    public GameObject coeurVIDE2;
    public GameObject coeurVIDE3;

    //Game Over
    public Button menuButton;        // Boutton qui renvoie vers le menu
    public Button restartButton;     // Boutton qui fais refaire le niveau 
    public GameObject popupGameOver; // Panel Game Over

    private int clésRequises; // Nombre de clés nécessaires pour ouvrir la porte

    protected void PrendreDegat()
    {
        if (vie <= 0) return;

        vie--;

        if (vie == 2)
        {
            coeur3.SetActive(false); // Masque le 3e cœur
            coeurVIDE3.SetActive(true); //Démasque le 3eme coeur vide
        }
        else if (vie == 1)
        {
            coeur2.SetActive(false); // Masque le 2e cœur
            coeurVIDE2.SetActive(true); //Démasque le 3eme coeur vide
        }
        else if (vie == 0)
        {
            coeur1.SetActive(false); // Masque le 1er cœur
            coeurVIDE1.SetActive(true); //Démasque le 3eme coeur vide
            Debug.Log(" Le joueur est mort !");
            popupPanel.SetActive(false);
            controlButtons.SetActive(false);
            popupGameOver.SetActive(true);
            

            player.Dead();

            menuButton.interactable = true;
            menuButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
            menuButton.onClick.AddListener(SceneMenu);

            restartButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
            restartButton.onClick.AddListener(SceneLvl1);

        }
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            clésRequises = 1;
        }
        else if (sceneName == "Level2")
        {
            clésRequises = 2;
        }
        else if (sceneName == "Level3")
        {
            clésRequises = 3;
        }

        player = FindObjectOfType<Player>(); // Trouve automatiquement le joueur dans la scène

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        if (popupGameOver != null)
        {
            popupGameOver.SetActive(false);
        }

        textCléScore.text = $"{cléScore} / {clésRequises}";

        sr = GetComponent<SpriteRenderer>(); // On récupère le SpriteRenderer du coffre

        if (effetMagique != null)
            effetMagique.SetActive(false); // On cache l'effet magique au début

        // Ajouter un listener pour le bouton "Fermer"
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(ClosePageCoffre);
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {


        }
    }

    public void StartInteraction()
    {
        if (typeDeCoffre == CoffreType.Equation)
        {
            StartEquation();
        }
        else if (typeDeCoffre == CoffreType.Expression)
        {
            StartExpression();
        }
        else if (typeDeCoffre == CoffreType.DifficileExpression)
        {
            StartDifficileExpression();
        }
    }

    public void StartEquation()
    {
        popupPanel.SetActive(true); // Affiche le panel quand le joueur interagit
        submitButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
        submitButton.onClick.AddListener(VerifierReponseEquation);
        LancerNouvelleEquation();
    }

    public void StartExpression()
    {
        popupPanel.SetActive(true); // Affiche le panel pour l'expression
        submitButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
        submitButton.onClick.AddListener(VerifierReponseExpression);
        GenerateExpression();
    }

    public void StartDifficileExpression()
    {
        popupPanel.SetActive(true); // Affiche le panel pour l'expression difficile
        submitButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
        submitButton.onClick.AddListener(VerifierReponseExpression);
        GenerateHardcoreExpression();
    }

    void LancerNouvelleEquation()
    {
        int type = Random.Range(0, 4); // 0 = +, 1 = *, 2 = division, 3 = équation type x + b = c
        int a = 0, b = 0, resultat = 0;
        string equation = "";

        switch (type)
        {
            case 0: // addition
                a = Random.Range(10, 100);
                b = Random.Range(10, 100);
                resultat = a + b;
                equation = $"{a} + {b} = ?";
                Debug.Log($"Réponse : {resultat}"); // Debug pour vérifier l'équation
                break;

            case 1: // multiplication
                a = Random.Range(5, 15);
                b = Random.Range(5, 15);
                resultat = a * b;
                equation = $"{a} × {b} = ?";
                Debug.Log($"Réponse : {resultat}"); // Debug pour vérifier l'équation
                break;

            case 2: // division entière
                b = Random.Range(2, 10);
                resultat = Random.Range(2, 10);
                a = resultat * b; // pour que a / b donne un résultat entier
                equation = $"{a} ÷ {b} = ?";
                Debug.Log($"Réponse : {resultat}"); // Debug pour vérifier l'équation
                break;

            case 3: // équation type "x + 7 = 21"
                int x = Random.Range(5, 30);     // la vraie valeur de x
                b = Random.Range(1, 20);
                resultat = x - b;
                a = resultat; // stocke la bonne réponse dans a
                equation = $"x + {b} = {x} | x = ?";
                Debug.Log($"Réponse : {resultat}"); // Debug pour vérifier l'équation
                break;
        }

        this.a = resultat; // on stocke la bonne réponse dans la variable `a`
        equationText.text = equation + $"\n {score} / {objectif}";
        inputField.text = "";
    }

    void GenerateExpression()
    {
        int a = Random.Range(1, 5);
        int b = Random.Range(1, 5);
        int c = Random.Range(1, 5);
        int d = Random.Range(1, 10);

        int par = a + b;
        int pow = par * par;
        int mult = pow * c;
        solution = mult - d;

        expressionText.text = $"Résous : ({a} + {b})² × {c} - {d} \n Score : {score}/{objectif}";
        inputField.text = "";
        Debug.Log($"Réponse : {solution}"); // Debug pour vérifier l'expression
    }

    void GenerateHardcoreExpression()
    {
        int a = Random.Range(2, 6);  // pour addition interne
        int b = Random.Range(2, 5);  // pour multiplication
        int c = Random.Range(2, 8);  // pour soustraction externe
        int d = Random.Range(2, 5);  // pour division

        int partieGauche = (a + 2) * b;
        int partieDroite = c / d;
        int baseCalcul = (partieGauche - partieDroite);
        int resultat = baseCalcul * baseCalcul;

        string expressionAffichee = $"(({a} + 2) × {b} - ({c} ÷ {d}))²";

        expressionText.text = $"Résous :\n{expressionAffichee}\nScore : {score}/{objectif}";
        inputField.text = "";

        solution = resultat;

        Debug.Log($"Réponse : {solution}"); // Debug pour vérifier l'expression
    }

    public void ClosePageCoffre()
    {
        popupPanel.SetActive(false); // Ferme le panel
    }

    void VerifierReponseEquation()
    {
        if (int.TryParse(inputField.text, out int userReponse))
        {
            if (userReponse == a)
            {
                score++;
                Debug.Log("Bonne réponse !");
                if (score >= objectif)
                {
                    Ouvrir();
                }
                else
                {
                    LancerNouvelleEquation();
                }
            }
            else
            {
                Debug.Log("Mauvaise réponse !");
                PrendreDegat();
            }
        }
        else
        {
            Debug.Log("Entrez un nombre valide !");
        }
    }

    void VerifierReponseExpression()
    {
        if (int.TryParse(inputField.text, out int userReponse))
        {
            if (userReponse == solution)
            {
                score++;
                Debug.Log("Bonne réponse !");
                if (score >= objectif)
                {
                    Ouvrir();
                }
                else
                {
                    GenerateExpression();
                }
            }
            else
            {
                Debug.Log("Mauvaise réponse !");
                PrendreDegat();
            }
        }
        else
        {
            Debug.Log("Entrez un nombre valide !");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si le joueur entre dans la zone du coffre
        {
            isPlayerInRange = true;

            if (textCléScore != null) // Vérifie si le texte est assigné
            {
                textCléScore.text = $"{cléScore} / {clésRequises}"; // Met à jour le texte
                Debug.Log("Texte textCléScore mis à jour : " + textCléScore.text);
            }
            else
            {
                Debug.LogWarning("Le champ textCléScore n'est pas assigné dans l'inspecteur !");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si le joueur quitte la zone du coffre
        {
            isPlayerInRange = false;
        }
    }

    // Coroutine pour gérer le fondu du coffre
    private IEnumerator FonduCoffre()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fonduSpeed; // Augmenter la valeur de t en fonction du temps
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - t); // Appliquer le fondu
            yield return null; // Attendre une frame avant de recommencer
        }
        gameObject.SetActive(false); // Après le fondu, désactiver le coffre
    }

    public void SceneMenu()
    {
        Debug.Log("Bouton menu principal clicker !");
        SceneManager.LoadScene("Menu");
    }

    public void SceneLvl1()
    {
        Debug.Log("Bouton restart !");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Ouvrir()
    {
        if (!isOpened)
        {
            Debug.Log("Coffre ouvert !");
            effetMagique.SetActive(true);
            popupPanel.SetActive(false);
            StartCoroutine(FonduCoffre());
            
            cléScore++; // Incrémente correctement le score des clés
            textCléScore.text = $"{cléScore} / {clésRequises}"; // Met à jour l'affichage des clés
            Debug.Log($"Clé obtenue : {cléScore} / {clésRequises}"); // Ajoute un log pour vérifier

            isOpened = true;

            player.AddCleScore(1); // Ajoute 1 clé au score total du joueur
        }
    }

    public int GetCleScore()
    {
        return cléScore; // Retourne le score des clés
    }


    public void setCléScore(int score)
    {
        cléScore = score; // Définit le score des clés
        if (cléScore >= clésRequises)
        {
            textCléScore.text = $"{cléScore} / {clésRequises}"; // Met à jour le texte
        }

    }

    

}