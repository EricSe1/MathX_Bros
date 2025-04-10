using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Coffre2 : Coffre
{
    public GameObject panelExpression;
    public Text expressionText;
    public InputField reponseExpression;
    public Button validateButton;
    public Button closeButton2; // Bouton pour fermer le panneau
    public GameObject effetMagique2; // Référence à l'effet magique (particules)
    private bool isOpened2 = false; // Vérifie si le coffre a été ouvert
    private SpriteRenderer sr2; // SpriteRenderer du coffre pour le fondu

    private int clésRequises2; // Nombre de clés nécessaires pour ouvrir la porte
    private Player player2; // Référence au script Player


    private int score2 = 0;
    private int objectif2 = 3;
    private int solution;

    void Start()
    {
        
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            clésRequises2 = 1;
        }
        else if (sceneName == "Level2")
        {
            clésRequises2 = 2;
            Debug.Log($"CleRequise : {clésRequises2}");
        }
        else if (sceneName == "Level3")
        {
            clésRequises2 = 3;
        }

        panelExpression.SetActive(false);
        effetMagique2.SetActive(false);
        validateButton.onClick.AddListener(VerifierReponse);

         if (closeButton2 != null)
        {
            closeButton2.onClick.AddListener(ClosePageCoffre2);
        }
    }

    public void StartExpression()
    {
        panelExpression.SetActive(true);
        GenerateExpression();
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

        expressionText.text = $"Résous : ({a} + {b})² × {c} - {d} \n Score : {score2}/{objectif2}";
        reponseExpression.text = "";    
        Debug.Log($"Réponse : {solution}");
        }

    void VerifierReponse()
    {
        if (int.TryParse(reponseExpression.text, out int reponse))
        {
            if (reponse == solution)
            {
                score2++;
                if (score2 >= objectif2)
                {
                    Debug.Log(" Objectif atteint !");
                    // ici ton bloc `if` de succès
                    panelExpression.SetActive(false);

                    // Lancer l'effet magique
                    if (effetMagique2 != null)
                    {
                        effetMagique2.SetActive(true); // Afficher l'effet magique
                    }

                    // Lancer le fondu du coffre
                    StartCoroutine(FonduCoffre2());

                    // Marquer le coffre comme ouvert pour éviter de réagir plusieurs fois
                    Ouvrir2();
                    isOpened2 = true;
                }
                else
                {
                    GenerateExpression();
                }
            }
            else
            {
                PrendreDegat(); // hérité de Coffre.cs
                
                 if (vie == 0)
                {panelExpression.SetActive(false);
                player2.Dead();
                }else GenerateExpression();
            }
        }
        else
        {
            Debug.Log("Entrée invalide");
        }
    }
        public void ClosePageCoffre2()
    {
        panelExpression.SetActive(false); // Ferme le panel
    }

    public void Ouvrir2()
    {
        if (!isOpened2)
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
            StartCoroutine(FonduCoffre2());
            cléScore++;
            textCléScore.text = $"{cléScore} / {clésRequises2}";
            isOpened2 = true;

            player2.AddCleScore(cléScore);
        }
    }

     private IEnumerator FonduCoffre2()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fonduSpeed; // Augmenter la valeur de t en fonction du temps
            sr2.color = new Color(sr2.color.r, sr2.color.g, sr2.color.b, 1 - t); // Appliquer le fondu
            yield return null; // Attendre une frame avant de recommencer
        }
        gameObject.SetActive(false); // Après le fondu, désactiver le coffre
    }
}