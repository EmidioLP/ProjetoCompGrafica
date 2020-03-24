using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SapoAI : Inimigo
{
    [SerializeField] private float esquerdaMax;  // Até onde ele pula pro lado esquerdo
    [SerializeField] private float direitaMax; // Até onde ele pula pro lado direito
    [SerializeField] private float DistanciaPulo = 5f; // Distância do pulo
    [SerializeField] private float AlturaPulo = 5f; // Altura do pulo
    [SerializeField] private LayerMask ground; //Indentificar que ele está pisando no chão


    private bool viradoEsq = true; //Ele sempre inicia virado para a esquerda
   [SerializeField] private Collider2D coll; //
   

    protected override void Start()
    {
        base.Start(); // Pega o Start do código base de Inimigos
        GetComponent<Collider2D>();
     
    }

    private void Update()
    {
        if (anim.GetBool("Pulando")) //Verifica se ele está pulando
        {
            if(rb.velocity.y < .1f) // Verifica se ele já pulou e agora está caindo
            {
                anim.SetBool("Caindo", true);
                anim.SetBool("Pulando", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("Caindo")) // Verifica se ele terminou de caiu e tocou no chão
        {
            anim.SetBool("Caindo", false);
        }
    }

    private void Mover()
    {
        if (viradoEsq) //Verifica se iniciou corretamente olhando para a esquerda
        {
            if (transform.position.x > esquerdaMax) //Verifica se ele não passou do máximo que pode ir pra esquerda
            {
                if (transform.localScale.x != 1) //Reprinta ele na tela com o tamanho padrão
                {
                    transform.localScale = new Vector3(9, 9);
                }

                if (coll.IsTouchingLayers(ground)) //Se estiver tocando no chão, então pula
                {
                    rb.velocity = new Vector2(-DistanciaPulo, AlturaPulo);
                    anim.SetBool("Pulando", true);
                }
            }
            else
            {
                viradoEsq = false;
            }
        }
        else
        {
            if (transform.position.x < direitaMax) //Verifica se passou do máximo pra direita
            {
                if (transform.localScale.x != -1) // Reprinta
                {
                    transform.localScale = new Vector3(-9, 9);
                }

                if (coll.IsTouchingLayers(ground)) //Verifica se tocou o chão
                {
                    rb.velocity = new Vector2(DistanciaPulo, AlturaPulo);
                    anim.SetBool("Pulando", true);

                }
            }
            else
            {
                viradoEsq = true;
            }
        }
    }

}