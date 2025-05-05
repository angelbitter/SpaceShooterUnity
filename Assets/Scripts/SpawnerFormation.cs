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
    
    public void OnSpawnFormationEvent()
    {
        StartCoroutine(SpawnFormation());
    }
    IEnumerator SpawnFormation()
    {
        float delay = UnityEngine.Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(delay);
        Vector3 newPosition = transform.position;
        newPosition.x = transform.position.x;
        newPosition.y = UnityEngine.Random.Range(-4.5f, 4.5f);
        transform.position = newPosition;

        for (int k = 0; k < 5; k++)
        {
            Instantiate(enemigoPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.4f);
        }

    }
}
