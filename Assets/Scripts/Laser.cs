using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    bool _isEnemyLaser = false;
    const int speed = 8; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (_isEnemyLaser == false)
       { GoesUp(); }
       else { GoesDown(); }

    }

    void GoesUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);


        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void GoesDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);


        if (transform.position.y <= -5)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void assignEnemyLeaser()
    { _isEnemyLaser = true; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
