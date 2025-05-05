using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private GameObject enemigoChaserPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textOleadas;
    [SerializeField] private GameObject gameOverPanel;
    public UnityEvent SpawnFormationEvent;
    public UnityEvent WinEvent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemigos());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator SpawnEnemigos()
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                
                SpawnFormationEvent.Invoke();
                StartCoroutine(SpawnOleada(i, j));
                for (int k = 0; k < 5; k++)
                {
                    if (gameOverPanel.activeSelf)
                    {
                        yield break;
                    }
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, UnityEngine.Random.Range(-4.5f, 4.5f), 0);
                    Instantiate(enemigoPrefab, puntoAleatorio, Quaternion.identity);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(2f);
                Vector3 puntoAleatorioChaser = new Vector3(transform.position.x, UnityEngine.Random.Range(-4.5f, 4.5f), 0);
                Instantiate(enemigoChaserPrefab, puntoAleatorioChaser, Quaternion.identity);
            }
            yield return new WaitForSeconds(4f);
        }
        WinEvent.Invoke();
    }
    IEnumerator SpawnOleada(int i, int j)
    {
        textOleadas.text = "Nivel " + (i + 1) + " - Oleada " + (j + 1);
        yield return new WaitForSeconds(2f);
        textOleadas.text = "";
    }
}
