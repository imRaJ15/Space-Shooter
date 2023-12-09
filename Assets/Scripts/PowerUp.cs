using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{ 
    [SerializeField]
    float _speed = 3.0f;

    [SerializeField]
    int _powerupID;

    [SerializeField]
    AudioClip _powerupActiveSound;

    AudioSource _audioSource;

    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerupActiveSound, transform.position);

            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActiveTripleShot();
                        break;
                    case 1:
                        player.ActiveSpeedUp();
                        break;
                    case 2:
                        player.ActiveShield();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }

}
