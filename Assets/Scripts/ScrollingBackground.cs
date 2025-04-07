using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
   public float speed; // Vitesse de défilement

   [SerializeField]
    private Renderer rend; // Référence au Renderer du fond

    void Update()
    {
        if (rend == null)
        {
            Debug.LogError("Renderer non assigné dans le script ScrollingBackground. Veuillez l'assigner dans l'inspecteur.");
            return;
        }

        rend.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0); // Défilement horizontal
    }
}
