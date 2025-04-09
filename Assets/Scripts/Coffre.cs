using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Coffre : MonoBehaviour
{
    public GameObject cle; // Référence à la clé qui apparaîtra
    public GameObject effetMagique; // Référence à l'effet magique (particules)
    public float fonduSpeed = 1f; // Vitesse de fondu pour le coffre
    private bool isPlayerInRange = false; // Vérifie si le joueur est proche
    private bool isOpened = false; // Vérifie si le coffre a été ouvert
    public Player joueur; // Référence au joueur
    public GameObject controlButtons; // Référence au controle Button

    public GameObject popupPanel;        // Le panel de la popup
    public Text equationText;            // Texte qui affiche l'équation
    public Text scoreText;            // Texte qui affiche le score
    public InputField inputField;        // Champ de réponse
    public Button submitButton;          // Bouton valider

    private SpriteRenderer sr; // SpriteRenderer du coffre pour le fondu

    
    private int a, b;
    private int score = 0;
    private int objectif = 4;

    public int vie = 3;
    public int cléScore = 0 ;

    public Text textCléScore;

    public GameObject coeur1;
    public GameObject coeur2;
    public GameObject coeur3;

    public GameObject coeurVIDE1;
    public GameObject coeurVIDE2;
    public GameObject coeurVIDE3;

//Game Over
    public Button menuButton;        // Boutton qui renvoie vers le menu
    public Button restartButton;      // Boutton qui fais refaire le niveau 
    public GameObject popupGameOver; // Panel Game Over

    // Appelle cette méthode quand le joueur prend un dégât
    public void PrendreDegat()
    {
        if (vie <= 0) return;

        vie--;

        if (vie == 2){
            coeur3.SetActive(false); // Masque le 3e cœur
            coeurVIDE3.SetActive(true); //Démasque le 3eme coeur vide
        }
        else if (vie == 1){
            coeur2.SetActive(false); // Masque le 2e cœur
            coeurVIDE2.SetActive(true); //Démasque le 3eme coeur vide
        }
        else if (vie == 0){
            coeur1.SetActive(false); // Masque le 1er cœur
            coeurVIDE1.SetActive(true); //Démasque le 3eme coeur vide
            Debug.Log(" Le joueur est mort !");
            popupPanel.SetActive(false);
            controlButtons.SetActive(false);
            popupGameOver.SetActive(true);


            menuButton.interactable = true;
            menuButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
            menuButton.onClick.AddListener(SceneMenu);

            restartButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
            restartButton.onClick.AddListener(SceneLvl1);
          
        }
    }

    void Start()
    {
         if (popupPanel != null)
        {popupPanel.SetActive(false);}

        if(popupGameOver!=null)
        {popupGameOver.SetActive(false);}

        textCléScore.text = $"{cléScore} / 2";

        sr = GetComponent<SpriteRenderer>(); // On récupère le SpriteRenderer du coffre
        if (cle != null)
            cle.SetActive(false); // On cache la clé au début
        if (effetMagique != null)
            effetMagique.SetActive(false); // On cache l'effet magique au début
    }

    void Update(){
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {     
            
           
        }
    }
    public void StartEquation()
    {      
    popupPanel.SetActive(true); //  Affiche le panel quand le joueur interagit
    submitButton.onClick.RemoveAllListeners(); // Pour éviter les doublons
    submitButton.onClick.AddListener(VerifierReponse);
    LancerNouvelleEquation();
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
            break;

        case 1: // multiplication
            a = Random.Range(5, 15);
            b = Random.Range(5, 15);
            resultat = a * b;
            equation = $"{a} × {b} = ?";
            break;

        case 2: // division entière
            b = Random.Range(2, 10);
            resultat = Random.Range(2, 10);
            a = resultat * b; // pour que a / b donne un résultat entier
            equation = $"{a} ÷ {b} = ?";
            break;

        case 3: // équation type "x + 7 = 21"
            int x = Random.Range(5, 30);     // la vraie valeur de x
            b = Random.Range(1, 20);
            resultat = x - b;
            Debug.Log("Resultat dans le switch :" + resultat);  
            a = resultat; // stocke la bonne réponse dans a

            equation = $"x + {b} = {x} | x = ?";
            break;
    }

    
    this.a = resultat; // on stocke la bonne réponse dans la variable `a`
    Debug.Log("Resultat en dehors du switch :" + this.a);  
    equationText.text = equation + $"\n {score} / {objectif}";
    inputField.text = "";
    popupPanel.SetActive(true);
        //scoreText.text = $"Score : {score} / {objectif}";
    }

    void VerifierReponse()
    {
        int resultAttendu = a + b;
        int userReponse;

        if (int.TryParse(inputField.text, out userReponse))
        {
            if (userReponse == resultAttendu)
            {
                score++;
                Debug.Log("Bonne réponse ! Score : " + score);

                if (score >= objectif)
                {
                    Debug.Log(" Objectif atteint !");
                    // ici ton bloc `if` de succès
                    popupPanel.SetActive(false);

                 // Lancer l'effet magique
            if (effetMagique != null)
            {
                effetMagique.SetActive(true); // Afficher l'effet magique
            }

            // Faire apparaître la clé
            if (cle != null)
            {
                cle.SetActive(true);
            }

            // Lancer le fondu du coffre
            StartCoroutine(FonduCoffre());

            // Marquer le coffre comme ouvert pour éviter de réagir plusieurs fois
                Ouvrir();
            isOpened = true;

                }
                else
                {
                    LancerNouvelleEquation();
                    
                }
            }
            else
            {

                
                Debug.Log(" Mauvaise réponse !");  
                LancerNouvelleEquation();
                PrendreDegat();
            }
        }
        else
        {
            Debug.Log(" Entrez un nombre valide !");
        }
    }


    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player")) // Si le joueur entre dans la zone du coffre
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")) // Si le joueur quitte la zone du coffre
        {
            isPlayerInRange = false;
        }
    }

    // Coroutine pour gérer le fondu du coffre
    private IEnumerator FonduCoffre(){
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fonduSpeed; // Augmenter la valeur de t en fonction du temps
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - t); // Appliquer le fondu
            yield return null; // Attendre une frame avant de recommencer
        }
        gameObject.SetActive(false); // Après le fondu, désactiver le coffre
    }

    public void SceneMenu(){
        Debug.Log("Bouton menu principal clicker !");
        SceneManager.LoadScene("Menu");
    }

    public void SceneLvl1(){
        Debug.Log("Bouton restart !");
        SceneManager.LoadScene("Level 1");   
    }

    public void Ouvrir(){
    if (!isOpened)
    {
        Debug.Log("Coffre ouvert !");
        effetMagique.SetActive(true);
       /* if (cle != null && joueur != null && joueur.clePosition != null)
            {
                cle.transform.SetParent(joueur.clePosition); // Attache la clé au joueur
                cle.transform.localPosition = Vector3.zero;  // Elle se place pile au-dessus de la tête
                cle.SetActive(true);
                Debug.Log("Clé attachée au joueur ! " + cle);
            }*/   
        StartCoroutine(FonduCoffre());
        cléScore++;
        textCléScore.text = $"{cléScore} / 2";
        isOpened = true;
    }
}

    

}