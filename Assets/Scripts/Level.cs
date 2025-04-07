using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public GameObject Level1; // Référence au canvas du menu principal
    public GameObject Level2; // Référence au canvas du jeu
    public GameObject Level3; // Référence au canvas des options

    public Button Level2Button; // Référence au bouton du niveau 2
    public Button Level3Button; // Référence au bouton du niveau 3

    public Text Level2Text; // Référence au texte du bouton du niveau 2
    public Text Level3Text; // Référence au texte du bouton du niveau 3

    public bool isLevel2Active = false; // Indicateur pour savoir si le niveau 1 est actif
    public bool isLevel3Active = false; // Indicateur pour savoir si le niveau 2 est actif

    public void Start()
    {
        if (isLevel2Active && !isLevel3Active)
        {
            Level2Text.text = "X"; // Changer le texte du bouton du niveau 2
            Level2.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 2 en rouge
            Level2Button.interactable = false; // Désactiver le bouton du niveau 2
        }
        else if (isLevel3Active && !isLevel2Active)
        {
            Level3Text.text = "X"; // Changer le texte du bouton du niveau 3
            Level3.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 3 en rouge
            Level3Button.interactable = false; // Désactiver le bouton du niveau 3
        }
        else
        {
            Level2Text.text = "X"; // Changer le texte du bouton du niveau 2
            Level2.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 2 en rouge
            Level2Button.interactable = false; // Désactiver le bouton du niveau 2
            Level3Text.text = "X"; // Changer le texte du bouton du niveau 3
            Level3.GetComponent<Image>().color = Color.red; // Mettre le canvas du niveau 3 en rouge
            Level3Button.interactable = false; // Désactiver le bouton du niveau 3
        }
    }
    public void Level1Laucher()
    {
        // Lancer le niveau 1
        SceneManager.LoadScene("Level 1");
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
            SceneManager.LoadScene("Level 2");
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
            SceneManager.LoadScene("Level 3");
        }
        
    }
}
