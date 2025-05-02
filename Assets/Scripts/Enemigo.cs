using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float ratioDisparo;
    [SerializeField] private Disparo disparoPrefab;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private TMPro.TextMeshProUGUI textPuntos;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private ObjectPool<Disparo> disparoPool;
    void Start()
    {
        textPuntos = GameObject.Find("Puntos").GetComponent<TMPro.TextMeshProUGUI>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        StartCoroutine(DisparoEnemigos());
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
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
        if (transform.position.x < -8.5f)
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator DisparoEnemigos()
    {
        while(true)
        {
            Disparo copy = disparoPool.Get();
            copy.transform.position = spawnPoint.transform.position;
            yield return new WaitForSeconds(ratioDisparo);
        }
    }
   
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.gameObject.CompareTag("DisparoPlayer"))
        {   
            audioSource.PlayOneShot(audioClip);
            string texto = textPuntos.text;
            string[] parts = texto.Split(new string[] { ":" }, StringSplitOptions.None);
            int puntos = int.Parse(parts[1]);
            textPuntos.text = "Puntos: " + (puntos + 200).ToString();	
            Destroy(gameObject);
            disparoPool.Clear();
        }
    }
}
