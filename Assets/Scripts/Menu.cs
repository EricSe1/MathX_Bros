using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject Canvas_Menu; // Référence au canvas du menu principal
    public GameObject Canvas_Level; // Référence au canvas du jeu
    public GameObject Canvas_Options; // Référence au canvas des options

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartGame()
    {
        // Canva 
        Canvas_Menu.SetActive(false); // Désactiver le menu principal
        Canvas_Level.SetActive(true); // Activer le canvas du jeu
    }

    public void ShowMenu()
    {
        // Afficher le menu principal
        Canvas_Menu.SetActive(true); // Activer le menu principal
        Canvas_Level.SetActive(false); // Désactiver le canvas du jeu
    }

    public void ShowOptions()
    {
        Canvas_Options.SetActive(true); // Activer le canvas des option
        Canvas_Menu.SetActive(false); // Désactiver le menu principal
        
    }
   public void QuitGame()
{
    Application.Quit();  // Ferme l'application
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Si tu es dans l'éditeur Unity, arrête la simulation
    #endif
}
}
