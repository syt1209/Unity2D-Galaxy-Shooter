using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state variables
    private Vector3 _startPos = new Vector3(0, 0, 0);
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private int _life = 3;

    // config variables
    [SerializeField] private float _laserSpawnOffset = 0.8f;
    private float _xMin = -11.3f, _xMax = 11.3f;
    private float _yMin = -3.8f,  _yMax = 0;
    [SerializeField] private float _firingDelay = 0.15f;
    private float _nextFire = -1f;

    // cached references
    [SerializeField]
    private GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Fire();
    }

    // private methods
    private void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        // bound in the x-direction
        if (transform.position.x < _xMin)
        {
            transform.position = new Vector3(_xMax, transform.position.y, 0);
        }
        else if (transform.position.x > _xMax)
        {
            transform.position = new Vector3(_xMin, transform.position.y, 0);
        }

        // bound in the y-direction
        transform.position = new Vector3(transform.position.x,
            Mathf.Clamp(transform.position.y, _yMin, _yMax), 0);
    }

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserSpawnOffset, 0), Quaternion.identity);
            _nextFire = Time.time + _firingDelay;
        }
    }

    // public methods
    public void Damage()
    {
        _life--;

        if (_life < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
