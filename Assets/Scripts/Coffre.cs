using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class Coffre : MonoBehaviour
{
    public GameObject cle; // Référence à la clé qui apparaîtra
    public GameObject effetMagique; // Référence à l'effet magique (particules)
    public float fonduSpeed = 1f; // Vitesse de fondu pour le coffre
    private bool isPlayerInRange = false; // Vérifie si le joueur est proche
    private bool isOpened = false; // Vérifie si le coffre a été ouvert
    public Player joueur; // Référence au joueur

    public GameObject popupPanel;        // Le panel de la popup
    public Text equationText;            // Texte qui affiche l'équation
    public InputField inputField;        // Champ de réponse
    public Button submitButton;          // Bouton valider

    private SpriteRenderer sr; // SpriteRenderer du coffre pour le fondu

    
        private int a, b;
        private int score = 0;
        private int objectif = 4;

    void Start()
    {
         if (popupPanel != null)
        popupPanel.SetActive(false);

        sr = GetComponent<SpriteRenderer>(); // On récupère le SpriteRenderer du coffre
        if (cle != null)
            cle.SetActive(false); // On cache la clé au début
        if (effetMagique != null)
            effetMagique.SetActive(false); // On cache l'effet magique au début
    }

    void Update(){
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {     
            
            /*
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
            isOpened = true;
            */
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
        a = Random.Range(1, 10);
        b = Random.Range(1, 10);
        equationText.text = $"Résous : {a} + {b} = ?";
        inputField.text = "";
        popupPanel.SetActive(true);
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
                    Debug.Log("✅ Objectif atteint !");
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
                Debug.Log("❌ Mauvaise réponse !");
                // ici ton bloc `else`
                LancerNouvelleEquation();
            }
        }
        else
        {
            Debug.Log("⚠️ Entrez un nombre valide !");
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

    public void Ouvrir(){
    if (!isOpened)
    {
        Debug.Log("Coffre ouvert !");
        effetMagique.SetActive(true);
        if (cle != null && joueur != null && joueur.clePosition != null)
            {
                cle.transform.SetParent(joueur.clePosition); // Attache la clé au joueur
                cle.transform.localPosition = Vector3.zero;  // Elle se place pile au-dessus de la tête
                cle.SetActive(true);
                Debug.Log("Clé attachée au joueur ! " + cle);
            }   
        StartCoroutine(FonduCoffre());
        isOpened = true;
    }
}

    

}