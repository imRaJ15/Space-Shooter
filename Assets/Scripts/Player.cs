 using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialize Field

    [SerializeField]
    GameObject _preFabLaser;

    [SerializeField]
    float _speed = 5f;
    float _speedMultiplier = 2; 

    [SerializeField]
    float _fireRate = 0.5f;
    float _canFire = -1f;

    [SerializeField]
    int _playerLives = 3;

    [SerializeField]
    GameObject _preFabTripleShot;

    [SerializeField]
    GameObject _shieldVisualizer;

    [SerializeField]
    GameObject _leftEngine;

    [SerializeField]
    GameObject _rightEngine;

    [SerializeField]
    AudioClip _laserSoundClip;

    #endregion

    #region Fields

    int _score = 0;

    float horizontalInput;
    float verticalInput;
    SpawnManager _spawnManager;
    bool _tripleShotActive = false;
    bool _speedupActive = false;
    bool _shieldActive = false;
    UiManager _uiManager;
    AudioSource _audioSource1;
    GameManager _gameManager;
    public bool isPlayer1;
    public bool isPlayer2;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _shieldVisualizer.SetActive(false);
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        { Debug.LogError(" the Spawn Manger is Null"); }

        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
        if (_uiManager == null)
        { Debug.LogError("The UI Manager is Null"); }

        _audioSource1 = GetComponent<AudioSource>();
        if (_audioSource1 == null)
        { Debug.LogError("The Audio Source is Null"); }
        else
        { _audioSource1.clip = _laserSoundClip; }

       /* if (_spawnManager != null)
        { _spawnManager.StartSpawning(); } */

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager.isCoOpModeActive == false)
        {
            transform.position = new Vector3(0, -4, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.isCoOpModeActive == false)
        {
            isPlayer1 = true;
            isPlayer2 = true;
        }
        if (isPlayer1 == true)
        {
            Player1Movement();

            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            { LaserShoot(); }
        }
        else if (isPlayer2 == true) 
        {
            Player2Movement();

            if (Input.GetKeyDown(KeyCode.RightControl) && Time.time > _canFire)
            { LaserShoot(); }
        }
    }

    void Player1Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (_speedupActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 6), 0);

        if (transform.position.x >= 9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
        else if (transform.position.x <= -9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
    }

    void Player2Movement()
    {
        horizontalInput = Input.GetAxis("Player2_Horizontal_Controls");
        verticalInput = Input.GetAxis("Player2_Vertical_Controls");
        if (_speedupActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4, 6), 0);

        if (transform.position.x >= 9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }
        else if (transform.position.x <= -9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
    }

    void LaserShoot()
    {
        if (_tripleShotActive == false)
        {
            _canFire = Time.time + _fireRate;
            float positionY = transform.position.y + 1.0f;
            Instantiate(_preFabLaser, new Vector3(transform.position.x, positionY, 0), Quaternion.identity);
        }
        

        if (_tripleShotActive == true)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_preFabTripleShot, transform.position, Quaternion.identity);
        }

        _audioSource1.Play();
    }

    public void Damage()
    {
        if (_shieldActive == false)
        {
            _playerLives--;
            _uiManager.UpdateLive(_playerLives);
        }
        if (_playerLives == 1)
        { _leftEngine.SetActive(true); }

        if(_playerLives == 2)
        { _rightEngine.SetActive(true); }   

        if (_playerLives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.IfPlayerDie();
            _uiManager.UpdateLive(0);
        }
    }

    public void ActiveTripleShot()
    {
        _tripleShotActive = true;
        Debug.Log("Triple Lesar Mode is Acivited!");
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void ActiveSpeedUp()
    {
        _speedupActive = true;
        StartCoroutine(SpeedUpRoutine());
    }

    IEnumerator SpeedUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedupActive = false;
    }

    public void ActiveShield()
    {
        _shieldActive = true;
        Debug.Log("Shield Activeted!");
        _shieldVisualizer.SetActive(true);
        StartCoroutine(ShieldRoutine());
    }

    IEnumerator ShieldRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _shieldVisualizer.SetActive(false);
        _shieldActive = false;
    }

    public void AddScore(int _point) 
    { 
        _score += _point;
        _uiManager.UpdateScore(_score);
    }

    public void CheckForBestScore(int _highScore = 0)
    {
        if (_highScore < _score)
        {
            _highScore = _score;
            _uiManager.UpdateHighScorew(_highScore);
        }
    }
}

