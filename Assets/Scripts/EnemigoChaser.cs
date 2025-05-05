using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemigoChaser : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private TMPro.TextMeshProUGUI textPuntos;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private LifePickUp lifePickUpPrefab;
    [SerializeField] private IncreaseAttackRatePU increaseAttackPrefab;
    [SerializeField] private SpreadShotPU spreadShotPrefab;

    [SerializeField] private int pickUpSpawnRate;
    private bool canMove = true;
    private Vector3 targetPosition;
    private ObjectPool<Disparo> disparoPool;
    private Player player;
    void Start()
    {
        textPuntos = GameObject.Find("Puntos").GetComponent<TMPro.TextMeshProUGUI>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(MoveTowardsPlayer());
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
        transform.Translate(new Vector3(-1, 0, 0) * 1 * Time.deltaTime);
    }

    private IEnumerator MoveTowardsPlayer()
    {
        StartCoroutine(MoveTimer());
        while (true)
        {
        
        if (player != null && canMove)
        {
            transform.Translate(targetPosition * velocidad * Time.deltaTime);
            
        }
        yield return null;
        }
    }
    private IEnumerator MoveTimer()
    {
        while (true)
        {   
            targetPosition = (player.transform.position - transform.position).normalized;
            canMove = true;
            yield return new WaitForSeconds(2f);
            canMove = false;
            yield return new WaitForSeconds(0.5f);
            DisparoRadial();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void DisparoRadial()
    {
        for (int i = 0; i < 8; i++)
        {
            Disparo copy = disparoPool.Get();
            copy.transform.position = spawnPoint.transform.position;
            copy.transform.rotation = Quaternion.Euler(0, 0, i * 45);
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.gameObject.CompareTag("DisparoPlayer"))
        {
            audioSource.PlayOneShot(audioClip);
            string texto = textPuntos.text;
            string[] parts = texto.Split(new string[] { ":" }, StringSplitOptions.None);
            int puntos = int.Parse(parts[1]);
            textPuntos.text = "Puntos: " + (puntos + 200).ToString();
            int rate = UnityEngine.Random.Range(0, pickUpSpawnRate);
            switch (rate)
            {
                case 0:
                    Instantiate(lifePickUpPrefab, transform.position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(increaseAttackPrefab, transform.position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(spreadShotPrefab, transform.position, Quaternion.identity);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
            disparoPool.Clear();
        }
    }
}
