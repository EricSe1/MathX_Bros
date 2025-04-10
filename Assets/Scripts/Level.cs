using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public GameObject Level2; // Référence au canvas du jeu
    public GameObject Level3; // Référence au canvas des options

    public Button Level2Button; // Référence au bouton du niveau 2
    public Button Level3Button; // Référence au bouton du niveau 3

    public Text Level2Text; // Référence au texte du bouton du niveau 2
    public Text Level3Text; // Référence au texte du bouton du niveau 3

    public bool isLevel2Active; // Indicateur pour savoir si le niveau 1 est actif
    public bool isLevel3Active; // Indicateur pour savoir si le niveau 2 est actif

    public void Start()
    {
        /* isLevel2Active = PlayerPrefs.GetInt("Level2Active", 0) == 1; // Vérifier si le niveau 2 est actif
        isLevel3Active = PlayerPrefs.GetInt("Level3Active", 0) == 1; // Vérifier si le niveau 3 est actif 
        isLevel2Active = true; // Activer le niveau 2 pour le test
        isLevel3Active = true; // Désactiver le niveau 3 pour le test*/

        // Vérifier si le niveau 2 est actif
        if (isLevel2Active && !isLevel3Active)
        {
            Level3Text.text = "X"; // Changer le texte du bouton du niveau 2
            Level3.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 2 en rouge
            Level3Button.interactable = false; // Désactiver le bouton du niveau 2
            Debug.Log("Le niveau 2 est déverrouillé mais le niveau 3 est bloqué");
        }
        else if (!isLevel2Active && !isLevel3Active)
        {
            Level2Text.text = "X"; // Changer le texte du bouton du niveau 2
            Level2.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 2 en rouge
            Level2Button.interactable = false; // Désactiver le bouton du niveau 2
            Level3Text.text = "X"; // Changer le texte du bouton du niveau 3
            Level3.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 3 en rouge
            Level3Button.interactable = false; // Désactiver le bouton du niveau 3
            Debug.Log("Le niveau 2 et le niveau 3 sont bloqués");
        }
        else
        {
            Level2Text.text = "2"; // Changer le texte du bouton du niveau 2
            Level2.GetComponent<Image>().color = Color.green; // Mettre le canvas du niveau 2 en rouge
            Level3Text.text = "3"; // Changer le texte du bouton du niveau 3
            Level3.GetComponent<Image>().color = Color.green; // Mettre le canvas du niveau 3 en rouge
            Debug.Log("Le niveau 2 et le niveau 3 sont déverrouillés");
        }
    }
    public void Level1Laucher()
    {
        // Lancer le niveau 1
        SceneManager.LoadScene("Level1");
    }
    public void Level2Laucher()
    {
        if (Level2.GetComponent<Image>().color == Color.red)
        {
            Level2Button.interactable = false; // Désactiver le bouton du niveau 2
            Debug.Log("Le niveau 2 est bloqué.");
        }
        // Lancer le niveau 2
        else
        {
            Level2Button.interactable = true; // Activer le bouton du niveau 2
            SceneManager.LoadScene("Level2");
        }
        
    }
  
    public void Level3Laucher()
    {
        if (Level3.GetComponent<Image>().color == Color.red)
        {
            Level3Button.interactable = false; // Désactiver le bouton du niveau 3
            Debug.Log("Le niveau 3 est bloqué.");
        }
        // Lancer le niveau 3
        else
        {
            Level3Button.interactable = true; // Activer le bouton du niveau 3
            SceneManager.LoadScene("Level3");
        }
        
    }
}
