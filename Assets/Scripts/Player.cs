using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    private ObjectPool<Disparo> disparoPool;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Image vida1, vida2, vida3;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;


    private float vidas = 6;
    private int puntosDisparo = 2;
    private bool dead = false;
    private float temporizadorDisparo;
    private bool isVulnerable = true; // Tiempo de invulnerabilidad
    private float vulnerableTime = 1f; // Tiempo de invulnerabilidad
    void Start()
    {

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

        if (!dead)
        {
            Move();
            DelimitarMovimiento();
            Disparar();
        }
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
            audioSource.PlayOneShot(audioClip[0]); // Sonido de disparo
            for (int i = 0; i < puntosDisparo; i++)
            {
                Disparo copy = disparoPool.Get();
                copy.transform.position = spawnPoints[i].position;
                copy.transform.rotation = spawnPoints[i].rotation;
            }
            temporizadorDisparo = 0;
        }
    }
    public void ActualizarVidas(int vida)
    {
        vidas += vida;
        if (vidas > 6)
        {
            vidas = 6;
        }
        switch (vidas)
        {
            case 0:
                vida1.fillAmount = 0f;
                vida2.fillAmount = 0f;
                vida3.fillAmount = 0f;
                Muerte();
                break;
            case 1:
                vida1.fillAmount = 0.5f;
                vida2.fillAmount = 0f;
                vida3.fillAmount = 0f;
                break;
            case 2:
                vida1.fillAmount = 1f;
                vida2.fillAmount = 0f;
                vida3.fillAmount = 0f;
                break;
            case 3:
                vida1.fillAmount = 1f;
                vida2.fillAmount = 0.5f;
                vida3.fillAmount = 0f;
                break;
            case 4:
                vida1.fillAmount = 1f;
                vida2.fillAmount = 1f;
                vida3.fillAmount = 0f;
                break;
            case 5:
                vida1.fillAmount = 1f;
                vida2.fillAmount = 1f;
                vida3.fillAmount = 0.5f;
                break;
            case 6:
                vida1.fillAmount = 1f;
                vida2.fillAmount = 1f;
                vida3.fillAmount = 1f;
                break;

        }
    }
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if ((otro.gameObject.CompareTag("Enemigo") || otro.gameObject.CompareTag("DisparoEnemigo"))
        && isVulnerable)
        {
            ActualizarVidas(-1);
            audioSource.PlayOneShot(audioClip[1]); // Sonido de daño
            StartCoroutine(invulnerable());
        }
    }
    IEnumerator invulnerable()
    {
        isVulnerable = false;
        StartCoroutine(InvulnerableVisual());
        yield return new WaitForSeconds(vulnerableTime);
        isVulnerable = true;
    }
    IEnumerator InvulnerableVisual()
    {
        // playerSprite.color = Color.red;
        while (!isVulnerable)
        {
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        // playerSprite.color = Color.white;
    }
    void Muerte()
    {
        audioSource.PlayOneShot(audioClip[2]);
        dead = true;
        playerSprite.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(WaitForKeyToRestart());
    }
    private IEnumerator WaitForKeyToRestart()
    {
        yield return new WaitForSeconds(2f);
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
    public void RecuperarVida(int vida)
    {
        audioSource.PlayOneShot(audioClip[3]); // Sonido de recoger pickup
        ActualizarVidas(vida);
    }
    public void PickedPowerUp(string type)
    {
        audioSource.PlayOneShot(audioClip[3]); // Sonido de recoger pickup
        switch (type)
        {
            case "IncreaseAttackRate":
                StartCoroutine(IncreaseAttackRate());
                break;
            case "SpreadShot":
                StartCoroutine(SpreadShot());
                break;
            default:
                break;
        }
    }
    private IEnumerator IncreaseAttackRate()
    {
        ratioDisparo /= 2f;
        yield return new WaitForSeconds(5f);
        ratioDisparo *= 2f;
    }
    private IEnumerator SpreadShot()
    {
        puntosDisparo = 4;
        yield return new WaitForSeconds(5f);
    }
}
