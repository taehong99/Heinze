using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject Enemy;
    public int xPos; // �����Ǵ� x ��ǥ
    public int zPos; // �����Ǵ� z ��ǥ
    public int enemyCount; // �����Ǵ� ������ ��

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(1, 50);
            zPos = Random.Range(1, 31);
            Instantiate(Enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 10;
        }
    }
}

