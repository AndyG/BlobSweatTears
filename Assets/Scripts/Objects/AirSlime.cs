using UnityEngine;
using System.Collections;

public class AirSlime : MonoBehaviour
{
    [SerializeField]
    private GameObject groundBloodPrefab;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void KillYourself()
    {
        var parent = gameObject;
        Destroy(parent);
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
