using System;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    private ObjectPool<Disparo> disparoPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private TMPro.TextMeshProUGUI textVidas;

    private float vidas = 6;
    private float temporizadorDisparo;
    void Start()
    {
        textVidas.text = "Vidas: " + vidas;
    }
    private void Awake()
    {
        disparoPool = new ObjectPool<Disparo>(CreateDisparo, OnGetDisparo, OnReleaseDisparo, OnDestroyDisparo, false, 10, 20);
    }

    private Disparo CreateDisparo()
    {
        Disparo disparoCopy = Instantiate(disparoPrefab);
        disparoCopy.MyPool = disparoPool;
        return disparoCopy;
    }
    private void OnGetDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(true);
    }
    private void OnReleaseDisparo(Disparo disparo)
    {
        disparo.gameObject.SetActive(false);
    }
    private void OnDestroyDisparo(Disparo disparo)
    {
        Destroy(disparo.gameObject);
    }
    void Update()
    {
        // Movimiento constante hacia la izquierda del jugador
        transform.Translate(new Vector2(1, 0).normalized * -1 * Time.deltaTime);

        Move();
        DelimitarMovimiento();
        Disparar();

    }

    void Move()
    {
        float iNputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector2(iNputX, inputY).normalized * speed * Time.deltaTime);
    }

    void DelimitarMovimiento()
    {
        float x = Mathf.Clamp(transform.position.x, -8.5f, 8.5f);
        float y = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = new Vector3(x, y, 0);
    }

    void Disparar()
    {
        
        temporizadorDisparo += 1 * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && temporizadorDisparo > ratioDisparo)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
            Disparo copy = disparoPool.Get();
            copy.transform.position = spawnPoints[i].position;
            }
            temporizadorDisparo = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.gameObject.CompareTag("Enemigo") || otro.gameObject.CompareTag("DisparoEnemigo"))
        {
            vidas--;
            textVidas.text = "Vidas: " + vidas;
            if (vidas <= 0)
            {
                Muerte();
            }
        }
    }
    void Muerte()
    {
        Destroy(gameObject);
        // Aquí puedes agregar la lógica para reiniciar el juego o mostrar una pantalla de Game Over
    }
}
