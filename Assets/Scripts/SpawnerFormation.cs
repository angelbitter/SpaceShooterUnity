using System.Collections;
using UnityEngine;

public class SpawnerFormation : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textOleadas;
    [SerializeField] private GameObject gameOverPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemigos());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);

        if (transform.position.y > 4.5f || transform.position.y < -4.5f)
        {
            speed *= -1;
        }
    }
    IEnumerator SpawnEnemigos()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (gameOverPanel.activeSelf)
                {
                    yield break;
                }
                Vector3 newPosition = transform.position;
                newPosition.x = transform.position.x;
                newPosition.y = Random.Range(-4.5f, 4.5f);
                transform.position = newPosition;

                for (int k = 0; k < 5; k++)
                {
                    Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.4f);
                }
                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(6f);
        }
    }
}
