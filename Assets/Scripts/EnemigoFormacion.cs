using System.Collections;
using System;
using UnityEngine;

public class EnemigoFormacion : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject disparoPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textPuntos;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    [SerializeField] private int pickUpSpawnRate;
    [SerializeField] private LifePickUp lifePickUpPrefab;
    [SerializeField] private IncreaseAttackRatePU increaseAttackPrefab;
    void Start()
    {
        textPuntos = GameObject.Find("Puntos").GetComponent<TMPro.TextMeshProUGUI>();
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
        if (transform.position.x < -9f)
        {
            Destroy(gameObject);
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
            textPuntos.text = "Puntos: " + (puntos + 50).ToString();

            int rate = UnityEngine.Random.Range(0, pickUpSpawnRate);
            switch (rate)
            {
                case 0:
                    Instantiate(lifePickUpPrefab, transform.position, Quaternion.identity);
                    break; 
                case 1:
                    Instantiate(increaseAttackPrefab, transform.position, Quaternion.identity);
                    break;
            default:
                break;
            }
                Destroy(otro.gameObject);
                Destroy(gameObject);
        }
    }
}