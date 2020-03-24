using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string CenaParaCarregar; //Armazena a string com nome da cena para carregar
    private void OnTriggerEnter2D(Collider2D collision) //Função responsavel por carregar a cena ao bater na casa
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(CenaParaCarregar); //carrega a cena
        }
    }
}
