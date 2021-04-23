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

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player is null)
        {
            Debug.LogError("Player is NULL");
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
            _player.AddScore(_enemyPoints);

            Destroy(this.gameObject);
        }

        if (other.tag is "Laser")
        {
            Destroy(other.gameObject);

            _player.AddScore(_enemyPoints);

            Destroy(this.gameObject);
        }
    }
}
