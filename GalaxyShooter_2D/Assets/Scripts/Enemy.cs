using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // state variables
    [SerializeField] private float _enemySpeed = 4.0f;
    [SerializeField] private int _enemyPoints = 10;
    private float _yMin = -5f, _yMax = 7f, _xMin = -8f, _xMax = 8f;

    // cached reference
    private Player _player;
    private Animator _anim;
    private Collider _collider;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player is null)
        {
            Debug.LogError("Player is NULL");
        }

        _anim = GetComponentInChildren<Animator>();
        if (_anim is null)
        {
            Debug.LogError("EnemyExplosion Animator is NULL");
        }

        _collider = GetComponent<Collider>();
        if (_collider is null)
        {
            Debug.LogError("Collider is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource is null)
        {
            Debug.LogError("Enemy Audio Source is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < _yMin)
        {
            float _randomX = Random.Range(_xMin, _xMax);
            transform.position = new Vector3(_randomX, _yMax, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player") 
        {
            _player.Damage();

            EnemyDeathSequence();
        }

        if (other.tag is "Laser")
        {
            Destroy(other.gameObject);

            EnemyDeathSequence();
        }
    }

    private void EnemyDeathSequence()
    {
        _player.AddScore(_enemyPoints);

        _anim.SetTrigger("OnEnemyDeath");
        Destroy(_collider);

        _audioSource.Play();
        Destroy(this.gameObject, 1f);
    }
}
