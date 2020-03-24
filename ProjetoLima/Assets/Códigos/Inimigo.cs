using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected AudioSource morte;

    protected virtual void Start()
    {
        GetComponent<Animator>();
        GetComponent<Rigidbody2D>();
        GetComponent<AudioSource>();
    }
    public void TriggerMorte()
    {
        anim.SetTrigger("Morto");
        morte.Play();
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Morte()
    {
        Destroy(this.gameObject);
    }

}
