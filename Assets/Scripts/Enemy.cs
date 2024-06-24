using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject hitVFX;

    [Header("Tunable Settings")]
    [SerializeField] int pointsPerHit = 2;
    [SerializeField] int hitPoints = 3;

    ScoreBoard scoreBoard;
    GameObject parentGameObject;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("SpawnAtRuntime");

        AddRigidbody();
    }

    private void AddRigidbody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnParticleCollision(GameObject collision)
    {
        IncreaseScore();

        if (hitPoints - pointsPerHit < 1)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        IncreaseScore();
        GameObject vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(gameObject);
    }

    private void IncreaseScore()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        hitPoints--;
        scoreBoard.IncreaseScore(pointsPerHit);
    }
}
