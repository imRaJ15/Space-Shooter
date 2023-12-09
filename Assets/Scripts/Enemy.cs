using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed = 4.0f;

    [SerializeField]
    AudioClip _objectDestroySound;

    [SerializeField]
    GameObject _preFabEnemyLaser;

    //Player _player;
    GameManager _gameManager;
    Animator _animator;
    AudioSource _audioSource;
    float _fireRate;
    float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        ///transform.position = new Vector3(Random.Range(-9f, 10f), 6, 0);
        _gameManager = GameObject.Find("Game_Manager").GetComponent <GameManager>();
        //_player = GameObject.Find("Player").GetComponent<Player>();
        //if (_player == null && _gameManager.isCoOpModeActive == false)
        //{ Debug.Log("Player is Null"); }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        { Debug.LogError("Animator is Null"); }

        _audioSource = GetComponent<AudioSource>();
        if (_objectDestroySound == null)
        { Debug.Log("The Audio Source is Null"); }
        else
        { _audioSource.clip = _objectDestroySound; }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

       EnemyShooting();
    }

    void EnemyShooting()
    {

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_preFabEnemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            { lasers[i].assignEnemyLeaser(); }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4)
        {
            //float randomX = Random.Range(-9f, 10f);
            //transform.position = new Vector3(randomX, 6, 0);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) 
            {
                player.Damage(); 
            }
            _animator.SetTrigger("OneEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.37f);
            _audioSource.Play();
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_gameManager.isCoOpModeActive == false)
            {
                Player _player = GameObject.Find("Player").GetComponent<Player>();

                if (_player == null)
                { Debug.Log("Player is Null"); }
                else { _player.AddScore(10); }
            }

            _animator.SetTrigger("OneEnemyDeath");
            _speed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.37f);
            _audioSource.Play();
        }
    }
}
