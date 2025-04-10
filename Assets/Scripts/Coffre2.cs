/*using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Coffre2 : MonoBehaviour
{
    public GameObject popupPanel;
    public Text expressionText;
    public InputField inputField;
    public Button validateButton;
    public Player joueur; // Référence au script Player

    private Player player; // Référence au script Player

    private int solution;
    private int score = 0;
    private int objectif = 3;

    void Start()
    {
        
        player = FindObjectOfType<Player>(); // Trouve automatiquement le joueur dans la scène
        popupPanel.SetActive(false);


        validateButton.onClick.AddListener(VerifierReponse);
    }

    public void StartExpression()
    {
        popupPanel.SetActive(true);
        GenerationExpression();
    }

    void GenerationExpression()
    {
        int a = Random.Range(1, 5);
        int b = Random.Range(1, 5);
        int c = Random.Range(1, 5);
        int d = Random.Range(1, 10);

        int parenthèse = a + b;
        int puissance = parenthèse * parenthèse;
        int mult = puissance * c;
        solution = mult - d;

        string exp = $"({a} + {b})² × {c} - {d}";
        expressionText.text = $"Résous :\n{exp}";
        inputField.text = "";
    }

    void VerifierReponse()
    {
        if (int.TryParse(inputField.text, out int reponse))
        {
            if (reponse == solution)
            {
                score++;
                Debug.Log("✅ Bonne réponse !");
                if (score >= objectif)
                {
                    popupPanel.SetActive(false);
                    Debug.Log("🎉 Tu as tout réussi !");
                }
                else
                {
                    GenerationExpression();
                }
            }
            else
            {
                Debug.Log("❌ Mauvaise réponse !");
                GenerationExpression();
            }
        }
        else
        {
            Debug.Log("⚠️ Entre un nombre valide !");
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
    
}
*/