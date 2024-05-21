using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    GameManagerScript game;
    int Hp;
    Rigidbody2D rb;

    [SerializeField] AudioClip ExplosionSound;
    AudioSource Source;

    float offsetX = 1;
    float offsetY = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        game = FindObjectOfType<GameManagerScript>();
        Source = game.audioSource;

        var force = game.enemySpeed;

        Hp = Random.Range(1, 3);

        rb.AddForce(force * (Random.Range(0, 2) * 2 - 1) * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);

        rb.AddForce(force * (Random.Range(0, 2) * 2 - 1) * Time.fixedDeltaTime * transform.right, ForceMode2D.Impulse);

        rb.AddTorque((Random.Range(0, 2) * 2 - 1) * 300 * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        game.Loop(transform, offsetX, offsetY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var temp = collision.gameObject;
        if (temp.CompareTag("Bullet"))
        {
            Destroy(temp);
            DamageSelf();

        }
        else if (temp.CompareTag("Player"))
        {
            var tempController = temp.GetComponent<PlayerController>();
            tempController.DamageSelf();
            DamageSelf();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var temp = collision.gameObject;
        if (temp.CompareTag("Bullet"))
        {
            Destroy(temp);
            DamageSelf();

        }
        else if (temp.CompareTag("Player"))
        {
            var tempController = temp.GetComponent<PlayerController>();
            tempController.DamageSelf();
            DamageSelf();

        }
    }
    void DamageSelf()
    {
        Hp--;
        Source.clip = ExplosionSound;
        if (Hp <= 0)
        {
            game.EnemyNum--;
            if (game.EnemyNum <= 0)
            {
                game.SpawnMore();
            }
            Source.Play();
            game.AddScore();
            Destroy(gameObject);
        }
        Source.Play();
    }
}
