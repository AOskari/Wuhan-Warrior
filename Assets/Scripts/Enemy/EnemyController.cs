using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

// This class controls the enemy's movement and attacking the player. This is a separate class from the Enemy-class, because this controls also the Nav Mesh Agent.
public class EnemyController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private AudioClip clip3;

    private int damage = 10;
    private float elapsedTime = 0f;
    private float timeBetweenHits = 2f;

    private Transform target;
    private NavMeshAgent agent;
    private ThirdPersonMovement player;
    private Enemy enemy;
    private List<AudioClip> clips;

    void Start()
    {
        clips = new List<AudioClip>();

        clips.Add(clip1);
        clips.Add(clip2);
        clips.Add(clip3);

        // Defining a few components.
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("ThirdPersonPlayer").GetComponent<ThirdPersonMovement>();
        enemy = gameObject.transform.GetChild(0).GetComponent<Enemy>();
        // Getting the player's position from the PlayerManager script.
        target = PlayerManager.instance.player.transform;

     
    }

    void Update()
    {
        // Adding to the attack timer.
        elapsedTime += Time.deltaTime;

        // Getting the distance between the player and the enemy.
        float distance = Vector3.Distance(target.position, transform.position);
      
        // start following the player.
        agent.SetDestination(target.position);

        // Rotating the enemy towards the player.
        FaceTarget();

        // If the time between hits cooldown has passed,
        if (elapsedTime >= timeBetweenHits && enemy.GetEnemyHealth() > 0 && distance <= 3f)
        {
            // play the attack sound effect and animation, and deal damage to the player.
            source.PlayOneShot(clips[Random.Range(0, 2)], 0.5f);
            animator.Play("enemyAttack");
            player.TakeDamage(damage);
            elapsedTime = 0f;
        }      

    }

    // Creating a method which turns the enemy towards the player.
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position);

        // Creating a variable which stores the wanted direction,
        Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // finally rotate the enemy towards the wanted direction.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.05f);

    }

}
