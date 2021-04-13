using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state variables
    private Vector3 _startPos = new Vector3(0, 0, 0);
    [SerializeField] private float _speed = 3.5f;
    private float _speedMultipler = 2f;
    [SerializeField] private int _life = 3;
    [SerializeField] private bool _isTripleShotActive = false, _isShieldActive = false; 
    private WaitForSeconds _powerDownTime = new WaitForSeconds(5.0f);

    // config variables
    [SerializeField] private float _laserSpawnOffset = 0.8f;
    private float _xMin = -11.3f, _xMax = 11.3f;
    private float _yMin = -3.8f,  _yMax = 0;
    [SerializeField] private float _firingDelay = 0.15f;
    private float _nextFire = -1f;

    // cached references
    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager is null)
        {
            Debug.LogError("Null object");
        }
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
            if (_isTripleShotActive is true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                return;
            }
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserSpawnOffset, 0), Quaternion.identity);
            _nextFire = Time.time + _firingDelay;
        }
    }

    // public methods
    public void Damage()
    {
        if (_isShieldActive is true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return; 
        }

        _life--;

        if (_life < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    // TripleShot powerup
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return _powerDownTime;
        _isTripleShotActive = false;
    }

    // Speed powerup
    public void SpeedBoostActive()
    {
        _speed *= _speedMultipler;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return _powerDownTime;
        _speed /= _speedMultipler;
    }

    // Shield powerup
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
}
