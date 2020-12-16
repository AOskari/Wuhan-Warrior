using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creating a script which keeps track of the player's position. This is only used by the EnemyController-script and the enemy nav mesh agent.

public class PlayerManager : MonoBehaviour
{

    #region Singleton

    public static PlayerManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject player;


}
