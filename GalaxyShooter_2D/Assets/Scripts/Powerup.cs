using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // state variables
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int _powerupID;

    // config variables
    private float _yMin = -4.5f;

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _yMin)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("ID" + _powerupID + "not found");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
