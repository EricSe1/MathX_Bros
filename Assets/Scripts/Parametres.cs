using UnityEngine;
using UnityEngine.UI;

public class Parametres : MonoBehaviour
{
    // Références aux UI
    public Toggle volumeMusiqueToogle; // Slider pour le volume de la musique
    public Toggle togglePleinEcran; // Référence au Toggle pour basculer le plein écran
    

    void Start()
    {

        // Ajouter un listener au Toggle pour gérer le plein écran
        togglePleinEcran.onValueChanged.AddListener(ChangerModePleinEcran);
        // Ajout
        volumeMusiqueToogle.onValueChanged.AddListener(ChangerVolumeMusique);

        // Charger le volume sauvegardé
        if (PlayerPrefs.HasKey("Volume"))
        {
            float volume = PlayerPrefs.GetFloat("Volume");
            AudioListener.volume = volume;
            volumeMusiqueToogle.isOn = volume > 0.5f; // Met à jour le Toggle en fonction du volume
            MettreAJourCouleurToggleVolume(volumeMusiqueToogle.isOn);
        }
    }

    // Fonction pour basculer entre plein écran et mode fenêtre
    public void ChangerModePleinEcran(bool estPleinEcran)
    {
        Debug.Log("ChangerModePleinEcran appelé avec : " + estPleinEcran);
        Screen.fullScreen = estPleinEcran;
        MettreAJourCouleurToggle(estPleinEcran);
    }

    // Fonction pour mettre à jour la couleur du Toggle
    private void MettreAJourCouleurToggle(bool estActif)
    {
        // Récupérer l'image du fond du Toggle (Background)
        Image toggleBackgroundImage = togglePleinEcran.targetGraphic as Image;

        if (toggleBackgroundImage != null)
        {
            // Changer la couleur en fonction de l'état
            if (estActif)
            {
                toggleBackgroundImage.color = new Color32(0, 255, 37, 255); // Couleur verte (#00FF25)
                Debug.Log("Le mode plein écran est activé et couleur verte.");
            }
            else
            {
                toggleBackgroundImage.color = new Color32(253, 44, 0, 255); // Couleur rouge (#FD2C00)
                Debug.Log("Le mode plein écran est désactivé et couleur rouge.");
            }
        }
        else
        {
            Debug.LogWarning("Impossible de trouver l'image du Toggle.");
        }
    }

    private void MettreAJourCouleurToggleVolume(bool estActif)
    {
        // Récupérer l'image du fond du Toggle (Background)
        Image toggleBackgroundImageVol = volumeMusiqueToogle.targetGraphic as Image;

        if (toggleBackgroundImageVol != null)
        {
            // Changer la couleur en fonction de l'état
            if (estActif)
            {
                toggleBackgroundImageVol.color = new Color32(0, 255, 37, 255); // Couleur verte (#00FF25)
                Debug.Log("Le Volume est activé et couleur verte.");
                
            }
            else
            {
                toggleBackgroundImageVol.color = new Color32(253, 44, 0, 255); // Couleur rouge (#FD2C00)
                Debug.Log("Le Volume est désactivé et couleur rouge.");
            }
        }
        else
        {
            Debug.LogWarning("Impossible de trouver l'image du Toggle.");
        }
    }

    // Fonction pour changer le volume de la musique
    public void ChangerVolumeMusique(bool estActif)
    {
        Debug.Log("ChangerVolumeMusique appelé avec : " + estActif);
        float volume = volumeMusiqueToogle.isOn ? 1.0f : 0.0f;
        AppliquerVolumeMusique(volume);

        // Sauvegarder l'état du volume (activé ou désactivé)
        PlayerPrefs.SetInt("VolumeState", estActif ? 1 : 0);
        PlayerPrefs.Save();

        MettreAJourCouleurToggleVolume(estActif);
    }

    // Fonction pour appliquer le volume de la musique
    private void AppliquerVolumeMusique(float volume)
    {
        // Appliquer le volume à la musique (exemple)
        AudioListener.volume = volume; // Ajuster le volume global de l'audio
        // Sauvegarder le volume dans PlayerPrefs
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save(); // Assurez-vous que les données sont sauvegardées
        // Afficher un message dans la console pour confirmer l'action
        Debug.Log("Volume Sauvegarder dans le PlayerPrefs : " + volume);
        Debug.Log("Volume de la musique appliqué : " + volume);
    }

    void OnApplicationQuit()
    {
        // Supprimer la clé "Volume" de PlayerPrefs
        if (PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.DeleteKey("Volume");
            Debug.Log("Clé 'Volume' supprimée de PlayerPrefs à la fermeture de l'application.");
        }
    }

}