using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//All AI agents in our game inheret from this basic Entity class.
//Stores all of the basic variables an entity needs, as well as implementing
//basic behaviours that most entities will share. These are virtual functions,
//so children classes can override them as neccessary. 

//Requiring several of the components that all entities will rely on.
[RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine), typeof(AudioSource))]
public class Entity : MonoBehaviour, IDamageable
{

    //Basic Attributes
    [Space, Header("Basic Entity Attributes")]
    [SerializeField] string enemyName = "Enemy";
    [SerializeField] public float HP = 100;
    [HideInInspector] public float maxHP;

    //Values between 0 and 20. Reduce the effect of relevant attack type by up to a max. 
    [Space, Header("Defensive Attributes")]
    [SerializeField, Range(0,20)] float physicalArmor = 0;
    [SerializeField, Range(0,20)] float magicalArmor = 0;
    [SerializeField] MagicDefensiveProperties[] defensiveProperties;

    //Visual Properties
    [Space, Header("Basic Visual Properties")]
    [SerializeField] Sprite damageBlip;
    [SerializeField] Color damageColor;
    [SerializeField] float damageBlipLength;
    [SerializeField] Sprite deathSprite;
    [SerializeField] public AudioClip damageNoise;

    #region Constants and local/hidden variables
    
    const float MAXIMUM_ARMOR_REDUCTION = .666f;
    const float CORPSE_DECAY_LENGTH = 10f;
    [HideInInspector] public NavMeshAgent agent; //Reference to the navmesh agent used for navigating terrain.
    [HideInInspector] public FiniteStateMachine fsm; //Refernce to the finite state machine which handles transitioning between AI behaviours.
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool alive = true;
    [HideInInspector] public bool invuln = false;
    [HideInInspector] public int id;
    #endregion

    //Store reference to objects we'll need.
    public virtual void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        fsm = GetComponent<FiniteStateMachine>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    public virtual void Start()
    {
        id = EnemyManager.instance.RegisterEnemy(this); 
        maxHP = HP;
    }

    //Called when this entity should take damage. Required by the IDamagable interface.
    public virtual void RecieveDamage(float amount, Spell spell)
    {

        //Return if this entity is already dead, or is invulnerable. 
        if (HP <= 0 || invuln)
            return;

        float multiplier = 1;

        //Below is mostly damage calculation, as well as some event triggering/vfx. 
        if (spell != null) //This first block runs if the damage is coming from a spell.
        {
            foreach (MagicDefensiveProperties p in defensiveProperties)
            {
                if (p.magicDamageType == spell.magicDamageType)
                {
                    multiplier = p.multiplier;
                    break;
                }
            }

            float armorMod = 1 - Mathf.Lerp(0, MAXIMUM_ARMOR_REDUCTION, magicalArmor / 20);

            float damage = amount * armorMod * multiplier;

            damage = Mathf.RoundToInt(damage);

            HP -= damage;

            //
            if (multiplier < 1)
            {
                PopupManager.instance.CreatePopupText("-" + damage.ToString(), this.transform.position, Color.blue);
            }
            else if (multiplier > 1)
            {
                PopupManager.instance.CreatePopupText("-" + damage.ToString(), this.transform.position, Color.red, true, 70);
            }
            else
            {
                PopupManager.instance.CreatePopupText("-" + damage.ToString(), this.transform.position, Color.red);
            }

            EventSystem.OnDamageEnemyEvent(this, damage);
        } 
        else //This block runs if the damage is coming from any other source than a spell.
        {
            float armorMod = 1 - Mathf.Lerp(0, MAXIMUM_ARMOR_REDUCTION, physicalArmor / 20);
            
            float damage = amount * armorMod * multiplier;

            damage = Mathf.RoundToInt(damage);

            HP -= damage;

            PopupManager.instance.CreatePopupText("-" + damage.ToString(), this.transform.position, Color.red);

            EventSystem.OnDamageEnemyEvent(this, damage);
        }

        audioSource.PlayOneShot(damageNoise);
    }

    //Attack function that most entites will have.
    //Expected to be overriden.
    public virtual void Attack() 
    {
        Debug.Log(enemyName + " tried to attack, but they have not implemented Attack!");
    }


    //A simple timer that behaviour states might use if they want something to happen after a delay.
    //Waits a given amount of time, before calling some kind of callback, so long as the abstrac state still exists.
    public delegate void callback();
    public IEnumerator GenericTimer(float t, AbstractState s, callback c) 
    {
        yield return new WaitForSeconds(t);
        if(s != null)
            c();
    }
    
    //Called when an enemy should die (this is usually managed by an entities global state)
    //Impliments a simple visual effect that causes the enemy to fall to the ground, before disappearing after a short time.
    public virtual void Die() 
    {
        if (alive) 
        {
            EventSystem.OnKillEnemyEvent(this);

            Destroy(gameObject, CORPSE_DECAY_LENGTH);

            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.useGravity = true;

            Destroy(rb, 2f);

            alive = false;

            EnemyManager.instance.DeregisterEnemy(this);
        }
    }

    //Used to check if an entity has reached the target it is pathfinding towards.
    //Returns true if the agent is waiting for a path, is near enough by its target, and is not moving.
    public bool pathComplete()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

//struct used to easily define weakness to different magic types.
[System.Serializable]
public struct MagicDefensiveProperties 
{
    public MagicDamageType magicDamageType;
    public float multiplier;
}
