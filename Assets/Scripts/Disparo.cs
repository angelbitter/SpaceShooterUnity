using UnityEngine;
using UnityEngine.Pool;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direccion;
    public ObjectPool<Disparo> MyPool { get; set; }

    void Update()
    {
        transform.Translate(direccion * speed * Time.deltaTime);
        if (transform.position.x > 8.5f || transform.position.x < -8.5f || transform.position.y > 4.5f || transform.position.y < -4.5f)
        {
            MyPool.Release(this);
        }
    }
}
