using UnityEngine;
using System.Collections;
public class Coffre : MonoBehaviour
{
    public GameObject cle; // Référence à la clé qui apparaîtra
    public GameObject effetMagique; // Référence à l'effet magique (particules)
    public float fonduSpeed = 1f; // Vitesse de fondu pour le coffre
    private bool isPlayerInRange = false; // Vérifie si le joueur est proche
    private bool isOpened = false; // Vérifie si le coffre a été ouvert
    public Player joueur; // Référence au joueur

    private SpriteRenderer sr; // SpriteRenderer du coffre pour le fondu

    void Start()
    {
        sr = GetComponent<SpriteRenderer>(); // On récupère le SpriteRenderer du coffre
        if (cle != null)
            cle.SetActive(false); // On cache la clé au début
        if (effetMagique != null)
            effetMagique.SetActive(false); // On cache l'effet magique au début
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
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
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si le joueur entre dans la zone du coffre
        {
            isPlayerInRange = true;
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

    public void Ouvrir()
{
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