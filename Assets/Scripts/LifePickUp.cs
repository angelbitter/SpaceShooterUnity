using UnityEngine;

public class LifePickUp : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0) * -100 * Time.deltaTime);
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.gameObject.CompareTag("Player"))
        {
            otro.gameObject.GetComponent<Player>().RecuperarVida(2);
            Destroy(gameObject);
        }
    }
}
