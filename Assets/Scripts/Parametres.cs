using UnityEngine;
using UnityEngine.UI;

public class Parametres : MonoBehaviour
{
    // Références aux UI
    public Slider volumeJeuSlider;  // Slider pour le volume du jeu
    public Slider volumeMusiqueSlider; // Slider pour le volume de la musique
    public Toggle togglePleinEcran; // Référence au Toggle pour basculer le plein écran

    void Start()
    {
        // Initialiser le volume des sliders
        volumeJeuSlider.value = AudioListener.volume; // Volume du jeu
        volumeMusiqueSlider.value = 0.5f; // Volume de la musique (par exemple, 50%)

        // Ajouter un listener au Toggle pour gérer le plein écran
        togglePleinEcran.onValueChanged.AddListener(ChangerModePleinEcran);
    }

    // Fonction pour basculer entre plein écran et mode fenêtre
    public void ChangerModePleinEcran(bool estPleinEcran)
    {
        // Activer ou désactiver le mode plein écran
        Screen.fullScreen = estPleinEcran;

        // Mettre à jour la couleur du Toggle en fonction de son état
        MettreAJourCouleurToggle(estPleinEcran);

        // Optionnel : Afficher un message dans la console pour confirmer l'action
        Debug.Log("Mode plein écran : " + estPleinEcran);
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

    // Fonction pour changer le volume du jeu
    public void ChangerVolumeJeu(float volume)
    {
        AudioListener.volume = volume;
    }

    // Fonction pour changer le volume de la musique
    public void ChangerVolumeMusique(float volume)
    {
        // Ici, tu devrais avoir un gestionnaire de musique avec un volume séparé, par exemple :
        // musiqueSource.volume = volume;
    }
}