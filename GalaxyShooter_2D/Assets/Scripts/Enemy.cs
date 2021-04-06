using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // state variables
    [SerializeField] private float _enemySpeed = 4.0f;
    private float _yMin = -5f, _yMax = 7f, _xMin = -8f, _xMax = 8f;

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
            Player player = other.transform.GetComponent<Player>() as Player;
            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }

        if (other.tag is "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
