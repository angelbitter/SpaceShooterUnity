using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textOleadas;
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
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                StartCoroutine(SpawnOleada(i, j));
                for (int k = 0; k < 5; k++)
                {
                    Vector3 puntoAleatorio = new Vector3(transform.position.x, Random.Range(-4.5f, 4.5f), 0);
                    Instantiate(enemigoPrefab, puntoAleatorio, Quaternion.identity);
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(7f);
        }
    }
    IEnumerator SpawnOleada(int i, int j)
    {
            textOleadas.text = "Nivel "+ (i+1) + " - Oleada " + (j+1);
            yield return new WaitForSeconds(2f);
            textOleadas.text = "";
    }
}
