using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject botPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 7f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++) // Spawne 3 bots à chaque intervalle
            {
                GameObject bot = Instantiate(botPrefab, spawnPoint.position, Quaternion.identity);
                StartCoroutine(RespawnAfter(bot, 10f * 2)); // Recycle le bot après le cycle complet
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator RespawnAfter(GameObject bot, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bot != null)
        {
            bot.SetActive(false); // Désactive le bot
            yield return new WaitForSeconds(1f); // Petite pause avant de le réactiver
            bot.SetActive(true); // Réactive le bot
            bot.transform.position = spawnPoint.position; // Réinitialise sa position
        }
    }
}