using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject _preFabEnemy;

    [SerializeField]
    GameObject[] _powerups;

    [SerializeField]
    GameObject _enemyContainer;

    bool _stopSpawnning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnerEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnerEnemyRoutine()
    {
        yield return new WaitForSeconds(2.5f);

        while (_stopSpawnning == false)
        {
            Vector3 position = new Vector3(Random.Range(-9, 10), 7, 0);
            GameObject newEnemy = Instantiate(_preFabEnemy, position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(2.0f);
        }
    }

    public void IfPlayerDie()
    {
        _stopSpawnning = true;
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(5.0f);

        while (_stopSpawnning == false)
        {
            int _powerupNumber = Random.Range(0, 3);
            Vector3 position = new Vector3(Random.Range(-8, 9), 7, 0);
           switch (_powerupNumber)
            {
                case 0:
                    GameObject powerUp = Instantiate(_powerups[0], position, Quaternion.identity);
                    break;

                case 1:
                    powerUp = Instantiate(_powerups[1], position, Quaternion.identity);
                    break;

                case 2:
                    powerUp = Instantiate(_powerups[2], position, Quaternion.identity);
                    break;
            }
            
            yield return new WaitForSeconds(Random.Range(6,11));
        }
    }
}
