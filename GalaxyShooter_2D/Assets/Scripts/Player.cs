using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // state variables
    private Vector3 _startPos = new Vector3(0, 0, 0);
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _acceleration = 1f;
    private float _minSpeed = 3.5f, _maxSpeed = 7.0f;
    private float _speedMultipler = 2f;
    [SerializeField] private int _life = 3, _score = 0, _ammo = 15;
    [SerializeField] private bool _isTripleShotActive = false, _isMultiDirectionActive = false, _isShieldActive = false,
                                  _isMissileActive = false; 
    private WaitForSeconds _powerDownTime = new WaitForSeconds(5.0f);
    [SerializeField] private int _shieldStrength = 3;

    // config variables
    [SerializeField] private float _laserSpawnOffset = 0.8f;
    private float _xMin = -11.3f, _xMax = 11.3f;
    private float _yMin = -3.8f,  _yMax = 3.8f;
    [SerializeField] private float _firingDelay = 0.15f, _thrustDelay = 1f;
    private float _nextFire = -1f, _nextThrust = -1f;
    [SerializeField] private float _shakeDuration = 0.5f;

    // cached references
    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _multiDirectionPrefab, _missilePrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _leftEngineFailure, _rightEngineFailure;
    [SerializeField] private AudioClip _laserSoundClip, _ammoOutSoundClip;
    private AudioSource _audioSource;
    private UIManager _uiManager;
    private Animator _playerAnim;
    private SpawnManager _spawnManager;
    private CameraShake _cameraShake;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startPos;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager is null)
        {
            Debug.LogError("Null object");
        }

        _playerAnim = GetComponentInChildren<Animator>();

        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_uiManager is null)
        {
            Debug.LogError("UIManager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource is null)
        {
            Debug.LogError("Player Audio Source is NULL.");
        }

        _cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        if (_cameraShake is null)
        {
            Debug.LogError("Camera Shake is NULL");
        }

        _leftEngineFailure.SetActive(false);
        _rightEngineFailure.SetActive(false);
        _shieldVisualizer.SetActive(false);
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
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"), 0);

        _playerAnim.SetFloat("turn_direction", direction.x);

        // thrusting behavior
        if (Input.GetKey(KeyCode.LeftShift) && _speed < _maxSpeed && Time.time > _nextThrust)
        {
            Accelerate();
        }
        else if (_speed > _minSpeed)
        {
            _nextThrust = Time.time + _thrustDelay;
            Decelerate();
        }


        float thrusterFillAmount = (_speed - _minSpeed)/_minSpeed;
        _uiManager.UpdateThrusterHUD(thrusterFillAmount);

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



    private void Accelerate()
    {
        _speed += _acceleration * Time.deltaTime;
    }

    private void Decelerate()
    {
        _speed -= _acceleration * Time.deltaTime;
    }

    private void Fire()
    {
        if (_ammo > 0 && Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            _ammo--;
            _audioSource.clip = _laserSoundClip;

            // Triple Shot
            if (_isTripleShotActive is true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
                return;
            }
            // Multi Direction Shot
            if (_isMultiDirectionActive is true)
            {
                Instantiate(_multiDirectionPrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
                return;
            }

            // Missile
            if (_isMissileActive is true)
            {
                Instantiate(_missilePrefab, transform.position, Quaternion.identity);
                _audioSource.Play();
                _isMissileActive = false;
                return;
            }

            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserSpawnOffset, 0), Quaternion.identity);
            _audioSource.Play();
            _nextFire = Time.time + _firingDelay;
        }

        if (_ammo <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            _audioSource.clip = _ammoOutSoundClip;
            _audioSource.Play();
        }

        _uiManager.UpdateAmmoImage(_ammo);
    }

    private void LifeVisual()
    {
        _uiManager.UpdateLifeImage(_life);

        switch (_life)
        {
            case 1:
                _leftEngineFailure.SetActive(true);
                _rightEngineFailure.SetActive(true);
                break;
            case 2:
                _leftEngineFailure.SetActive(true);
                _rightEngineFailure.SetActive(false);
                break;
            default:
                _leftEngineFailure.SetActive(false);
                _rightEngineFailure.SetActive(false);
                break;
        }
    }

    // public methods
    public void Damage()
    {
        if (_isShieldActive is true)
        {
            Renderer shieldRD = _shieldVisualizer.GetComponent<Renderer>();
            _shieldStrength--;
            switch (_shieldStrength)
            {
                case 0:
                    _isShieldActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
                case 1:
                    shieldRD.material.color = Color.red;
                    break;
                case 2:
                    shieldRD.material.color = Color.yellow;
                    break;
            }

            return; 
        }

        StartCoroutine(_cameraShake.CameraShakeRoutine(_shakeDuration));
        _life--;

        LifeVisual();

        if (_life < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject, 0.5f);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreText(_score);

    }

    // TripleShot powerup
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        _isMultiDirectionActive = false;
        _isMissileActive = false;
        StartCoroutine(SpecialWeaponPowerDownRoutine(_isTripleShotActive));
    }

    public void MultiDirectionActive()
    {
        _isMultiDirectionActive = true;
        _isTripleShotActive = false;
        _isMissileActive = false;
        StartCoroutine(SpecialWeaponPowerDownRoutine(_isMultiDirectionActive));
    }

    public void MissileActive()
    {
        _isMissileActive = true;
        _isTripleShotActive = false;
        _isMultiDirectionActive = false;
    }

    private IEnumerator SpecialWeaponPowerDownRoutine(bool isWeaponActive)
    {
        yield return _powerDownTime;
        isWeaponActive = false;
    }

    // MultiDirection powerup

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

    // Speed slowdown powerup - negative
    public void SlowDownActive()
    {
        _speed /= _speedMultipler;
        StartCoroutine(SpeedRecoverRoutine());
    }

    private IEnumerator SpeedRecoverRoutine()
    {
        yield return _powerDownTime;
        _speed *= _speedMultipler;
    }

    // Shield powerup
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldVisualizer.GetComponent<Renderer>().material.color = Color.cyan;
        _shieldStrength = 3;
    }

    public void AmmoCollected()
    {
        _ammo = 15;
    }

    public void LifeCollected()
    {
        if (_life < 3)
        {
            _life++;
            LifeVisual();
        }
    }
}
