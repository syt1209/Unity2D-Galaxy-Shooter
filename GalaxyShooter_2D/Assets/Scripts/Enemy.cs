using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // state variables
    [SerializeField] protected float _enemySpeed = 4.0f;
    [SerializeField] protected int _enemyPoints = 10;
    protected int _enemyLife = 1;
    protected float _yMin = -5f, _yMax = 7f, _xMin = -8f, _xMax = 8f;

    // config variables
    [SerializeField] protected Transform _path;
    protected int _currentPointOnPath = 0;
    protected Vector3 _currentTarget;

    // cached reference
    protected Player _player;
    protected Animator _anim;
    protected Collider _collider;
    private AudioSource _audioSource;

    protected virtual void Start()
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
    protected virtual void Update()
    {
        MoveAlongPath(_path);
    }

    protected virtual void MoveAlongPath(Transform path)
    {
        List<Transform> pointsOnPath = new List<Transform>();
        foreach (Transform child in path)
        {
            pointsOnPath.Add(child);
        }

        Vector3 currentPos = transform.position;
        _currentTarget = pointsOnPath[_currentPointOnPath].position;
        transform.position = Vector3.MoveTowards(currentPos, _currentTarget, _enemySpeed * Time.deltaTime);
    }

    public void UpdateCurrentPointOnPath(int pointID)
    {
        if (pointID == (_path.childCount - 1))
        { _currentPointOnPath = 0; }
        else
        { _currentPointOnPath = pointID + 1; }
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag is "Player") 
        {
            _player.Damage();
            _enemyLife--;

            if (_enemyLife < 1)
            {
                EnemyDeathSequence();
            }
        }

        if (other.tag is "Laser")
        {
            Destroy(other.gameObject);
            _enemyLife--;

            if (_enemyLife < 1)
            {
                EnemyDeathSequence();
            }
        }

        if (other.tag is "Missile")
        {
            Destroy(other.gameObject);
            _enemyLife--;

            if (_enemyLife < 1)
            {
                EnemyDeathSequence();
            }
        }
    }

    protected void EnemyDeathSequence()
    {
        _player.AddScore(_enemyPoints);

        _anim.SetTrigger("OnEnemyDeath");
        Destroy(_collider);

        _audioSource.Play();
        Destroy(this.gameObject, 1f);
    }
}
