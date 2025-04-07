using UnityEngine;

public class LogoZoomLoop : MonoBehaviour
{
    // Variables pour contrôler l'agrandissement et la réduction
    public float minScale;   // Taille minimale
    public float maxScale;   // Taille maximale
    public float speed;        // Vitesse du zoom (plus grand, plus rapide)

    private bool isZoomingOut = true;  // Si le logo doit se réduire ou agrandir

    // Référence à l'enfant (le logo)
    public Transform childTransform;

    void Update()
    {
        if (childTransform == null)
        {
            Debug.LogWarning("Aucun enfant spécifié dans 'childTransform'!");
            return;
        }

        // On alterne entre zoom avant et arrière
        if (isZoomingOut)
        {
            // Réduire la taille de l'enfant
            childTransform.localScale = Vector3.Lerp(childTransform.localScale, new Vector3(minScale, minScale, 1), Time.deltaTime * speed);

            // Si la taille minimale est atteinte, commencer à agrandir
            if (childTransform.localScale.x <= minScale + 0.01f)
            {
                isZoomingOut = false;
            }
        }
        else
        {
            // Agrandir la taille de l'enfant
            childTransform.localScale = Vector3.Lerp(childTransform.localScale, new Vector3(maxScale, maxScale, 1), Time.deltaTime * speed);

            // Si la taille maximale est atteinte, commencer à réduire
            if (childTransform.localScale.x >= maxScale - 0.01f)
            {
                isZoomingOut = true;
            }
        }
    }
}