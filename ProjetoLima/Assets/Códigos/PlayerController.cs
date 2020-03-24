using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    [SerializeField] private AudioSource pegada; // variavel para armazenar audio
    private enum State {parado, correndo, pulou, caindo, ferido}; //Estados de animação
    private State state = State.parado; // Estado de animação inicial
    [SerializeField] private LayerMask ground; //Indentifica o layer que ele está, ex: chão, ar, etc..
    [SerializeField] private float velocidade = 5f; //velocidade de movimento
    [SerializeField] private float ForçaPulo = 30f; //força do pulo / quão alto pula
    private int moedas = 0; // Inicia o valor inicial das moedas em 0
    [SerializeField] private TextMeshProUGUI moedasText; //variavel para amarzenar um txt
    [SerializeField] private int vida = 2; // Inicia a vida com 2
    [SerializeField] private Text vidaText; //variavel para armazenar um txt
    [SerializeField] private float ForçaDano = 10f; // A força que ele é empurrado para trás quando recebe dano
    [SerializeField] private AudioSource moeda; // variavel para armazenar audio
    [SerializeField] private AudioSource pulando; //variavel para armazenar audio
    [SerializeField] private AudioSource levoudano; //variavel para armazenar audio



    private void Start() //Chama assim que inicia o jogo
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        vidaText.text = vida.ToString();

    }
  
   private void Update() //Dá refresh toda vez que algo acontece
    {
        if(state != State.ferido)
        {
            //Captura o botão que está sendo pressionado
            InputManager();
        }
        //muda o estado da animação
        MudarEstado();
        anim.SetInteger("estado", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D collision) //Função que manipula os coletaveis
    {
        if (collision.tag == "Coletavel") //Verifica se ele colidiu com um coletável
        {
            //Destroi o coletável, adiciona um na variavel moedas e atualiza na tela.
            moeda.Play();
            Destroy(collision.gameObject); 
            moedas += 1;
            moedasText.text = moedas.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D outro)
    {
        if (outro.gameObject.tag == "Inimigo") //Verifica se se chocou com um inimigo.
        {
            Inimigo inimigo = outro.gameObject.GetComponent<Inimigo>();

            if (state == State.caindo) //Se ele se chocou enquanto pulava, então destroi o inimigo e pula novamente.
            {
                inimigo.TriggerMorte();
                ForçaPulo += 5f;
                Pular();
                ForçaPulo = 30f;
            }
            else //Se não, vai para o estado "ferido" e aciona a animação de ferida, além de perder uma vida
            {
                levoudano.Play();
                state = State.ferido;
                vida -= 1;
                vidaText.text = vida.ToString();
                if(outro.gameObject.transform.position.x > transform.position.x) //Empurra para a esquerda ou direita
                {
                    rb.velocity = new Vector2(-ForçaDano, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(ForçaDano, rb.velocity.y);
                }
                if (vida <= 0) //Se a vida chegar a 0, reinicia a fase
                {
                SceneManager.LoadScene(3);
                }
            }
        }
    }//Função que manipula a relação com os inimigos

    private void Pular() //Função que o faz pular
    {
        rb.velocity = new Vector2(rb.velocity.x, ForçaPulo);
        pulando.Play();
        state = State.pulou;
    }

    private void InputManager()
    {
        //captura o botão que esta sendo precionado horizontalmenente
        float hDirection = Input.GetAxis("Horizontal");

        //move esquerda
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-velocidade, rb.velocity.y);
            transform.localScale = new Vector2(-9, 9);
        }
        //move direita
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(velocidade, rb.velocity.y);
            transform.localScale = new Vector2(9, 9);

        }


        //pula
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Pular();
        }
    }//Função que manipula o input das teclas


    private void MudarEstado() //função mudar estado da animação
    {

        //Colocar a animação de cair
        if(state == State.pulou)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.caindo;
            }
        }
        
        //Mudar para a animação de parado se tocar o chão
        else if (state == State.caindo)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.parado;
            }
        }

        else if(state == State.ferido)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.parado;
            }
        }
        
        //Verificar se esta se movendo para iniciar a animação de correr
       else if (Mathf.Abs(rb.velocity.x) > 5f)
        {
            //Está se movendo
            state = State.correndo;
        }

        //Se não estiver se movendo, inicia a animação de parado
       else
        {
            state = State.parado;
        }
       
    }

    private void Pegada() //Função que manipula o som de passos
    {
        pegada.Play();
    }


}
