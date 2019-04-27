using UnityEngine;
using System.Collections;

public class AirSlime : MonoBehaviour
{
    [SerializeField]
    private GameObject groundBloodPrefab;

    [SerializeField]
    private float timeToLive = 2f;

    private float timeAlive = 0f;

    void Update() {
        this.timeAlive += Time.deltaTime;
        if (timeAlive >= timeToLive) {
            KillYourself();
        }
    }

    private void KillYourself()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var myX = Mathf.RoundToInt(gameObject.transform.position.x);
        var myY = Mathf.RoundToInt(gameObject.transform.position.y);
        SpawnMoreBloodAndDie(myX, myY);
    }

    private void SpawnMoreBloodAndDie(int xPosition, int yPosition)
    {
        Instantiate(groundBloodPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
        KillYourself();
    }
}
