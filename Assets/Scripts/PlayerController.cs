using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
[DisallowMultipleComponent]

public class PlayerController : MonoBehaviour
{
    float move;
    [SerializeField]
    int Hp = 3;
    Rigidbody2D rb;
    GameManagerScript game;

    [SerializeField]
    [Range(0.1f, 10000f)]
    float RotationPower;

    [SerializeField]
    [Range(0.1f, 10)]
    float MovePower;

    [SerializeField]
    [Range(0.1f, 10000f)]
    float fireForce;

    [SerializeField]
    Transform WeaponPoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField] AudioClip FireSound;
    [SerializeField] AudioClip DamageSound;
    [SerializeField] AudioClip DeadSound;

    AudioSource Source;

    private void Start()
    {
        game = FindObjectOfType<GameManagerScript>();
        rb = GetComponent<Rigidbody2D>();
        Source = game.audioSource;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        game.Loop(transform, 0, 0);

        move += Input.GetAxis("Horizontal") * RotationPower * Time.fixedDeltaTime;

        if (Input.GetAxis("Vertical") > 0)
        {
            rb.drag = 0;
            rb.AddForce(MovePower * Time.fixedDeltaTime * transform.up, ForceMode2D.Impulse);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, .02f), Mathf.Lerp(rb.velocity.y, 0, .02f));

            rb.drag = 2;
        }
        else
        {
            rb.drag = 1;
        }

        transform.eulerAngles = new Vector3(0, 0, -move);
    }


    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, WeaponPoint.position, WeaponPoint.rotation);
        Vector2 finalVelocity = fireForce * Time.fixedDeltaTime * WeaponPoint.up;
        Source.clip = FireSound;
        Source.Play();
        bullet.GetComponent<Rigidbody2D>().AddForce(finalVelocity, ForceMode2D.Impulse);
    }

    public void DamageSelf()
    {
        Hp--;
        game.DamagePlayer(Hp);
        Source.clip = DamageSound;
        if (Hp <= 0)
        {
            Source.clip = DeadSound;
            Destroy(gameObject);
            game.GameOver();
        }
        Source.Play();
    }

    public int GetHP()
    {
        return Hp;
    }
}
