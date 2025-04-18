using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGameplay : MonoBehaviour
{
    public GameObject Canvas_Pause; // Référence au canvas de pause
    public GameObject Canvas_Volume; // Référence au canvas de volume
    public GameObject Canvas_Resume; // Référence au canvas de reprise

    public void Start()
    {
        Time.timeScale = 1;

        // Vérifier l'état du volume sauvegardé
        if (PlayerPrefs.HasKey("VolumeState"))
        {
            bool volumeActif = PlayerPrefs.GetInt("VolumeState") == 1;
            AudioListener.volume = volumeActif ? 1.0f : 0.0f;
            Debug.Log("Volume appliqué depuis PlayerPrefs : " + AudioListener.volume);
            Volume();
        }
        else
        {
            // Si aucune valeur n'est sauvegardée, activer le volume par défaut
            AudioListener.volume = 1.0f;
            Debug.Log("Aucune valeur de volume trouvée, activation par défaut.");
        }
    }

    public void MenuGame() {
        // Sauvegarder une variable avant de changer de scène
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.Save(); // Assurez-vous que les données sont sauvegardées
        // Afficher le menu principal
        SceneManager.LoadScene("Menu");
    }

    public void PauseGame()
    {
        // Mettre le jeu en pause
        Canvas_Pause.SetActive(false); // Activer le canvas de pause
        Canvas_Resume.SetActive(true); // Désactiver le canvas de reprise
        Time.timeScale = 0; // Arrêter le temps
    }
    public void ResumeGame()
    {
        // Reprendre le jeu
        Canvas_Pause.SetActive(true); // Désactiver le canvas de pause
        Canvas_Resume.SetActive(false); // Activer le canvas de reprise
        Time.timeScale = 1; // Reprendre le temps
    }

    public void Volume()
    {
        Debug.Log("Bouton cliqué, exécution de la méthode Volume()."); // Confirme que le clic est détecté
        if (Canvas_Volume != null)
        {
            CanvasGroup canvasGroup = Canvas_Volume.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup n'est pas attaché à l'objet Canvas_Volume. Ajout automatique du composant.");
                canvasGroup = Canvas_Volume.AddComponent<CanvasGroup>(); // Ajouter dynamiquement CanvasGroup
            }

            if (AudioListener.volume > 0f)
            {
                // Si le volume est activé, le désactiver
                VolumeOff();
            }
            else
            {
                // Si le volume est désactivé, l'activer
                VolumeOn();
            }

            // Sauvegarder l'état du volume dans PlayerPrefs
            PlayerPrefs.SetInt("VolumeState", AudioListener.volume > 0f ? 1 : 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Canvas_Volume n'est pas assigné dans l'inspecteur Unity.");
        }
    }

    public void VolumeOff()
    {
        // Désactiver le son
        CanvasGroup canvasGroup = Canvas_Volume.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.3f; // Mettre l'opacité à 30%
            AudioListener.volume = 0; // Mettre le volume à 0
            Debug.Log("Volume désactivé : " + AudioListener.volume);
        }
        else
        {
            Debug.LogError("Exécution VolumeOff(), CanvasGroup n'est pas attaché à l'objet Canvas_Volume.");
        }
    }
    public void VolumeOn()
    {
        // Activer le son
        CanvasGroup canvasGroup = Canvas_Volume.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; // Mettre l'opacité à 100%
            AudioListener.volume = 1; // Remettre le volume à 1
            Debug.Log("Volume activé : " + AudioListener.volume);
        }
        else
        {
            Debug.LogError("Exécution VolumeOn(), CanvasGroup n'est pas attaché à l'objet Canvas_Volume.");
        }
    }
    public void ReloadGame()
    {
        // Relancer la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnApplicationQuit()
    {
        // Réinitialiser les paramètres (par exemple, supprimer la clé "Volume")
        if (PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.DeleteKey("Volume");
            Debug.Log("Clé 'Volume' supprimée de PlayerPrefs à la fermeture de l'application.");
        }
    }
}
