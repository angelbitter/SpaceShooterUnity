using System.Collections;
using System;
using UnityEngine;

public class EnemigoFormacion : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private GameObject disparoPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI textPuntos;
    void Start()
    {
        textPuntos = GameObject.Find("Puntos").GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        transform.Translate(new Vector3(-1, 0, 0) * velocidad * Time.deltaTime);
        if (transform.position.x < -8.5f)
        {
            Destroy(gameObject);
        }
    }
    
   
    private void OnTriggerEnter2D(Collider2D otro)
    {
        if(otro.gameObject.CompareTag("DisparoPlayer"))
        {
            string texto = textPuntos.text;
            string[] parts = texto.Split(new string[] { ":" }, StringSplitOptions.None);
            int puntos = int.Parse(parts[1]);
            textPuntos.text = "Puntos: " + (puntos + 50).ToString();	
            Destroy(otro.gameObject);
            Destroy(gameObject);
        }
    }
}
