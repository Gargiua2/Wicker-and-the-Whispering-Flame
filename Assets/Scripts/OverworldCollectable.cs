using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SphereCollider)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(ImposterSprite))]
public class OverworldCollectable : MonoBehaviour
{
    

    [SerializeField, Tooltip("This must be an item with the ICollectable interface.")] 
    public ScriptableObject collectableItem;
    [SerializeField] float range = .5f;
    [SerializeField] bool requireButtonPress = false;
    [SerializeField] LayerMask collectMask;
    [SerializeField] GameObject collectionTrigger;

    [SerializeField]
    GameObject pickupUI;

    #region Local Variables
    bool inRange = false;
    GameObject player;
    ICollectable collectable;
    TextMeshProUGUI pickupText;
    #endregion

    public void LoadLoot()
    {
        if (collectableItem as ICollectable != null)
        {
            collectable = collectableItem as ICollectable;
        }
        else 
        {
            Debug.LogError("The ScriptableObject passed to overworld collectable was not castable as an ICollectable");
        }

        player = Player.instance.gameObject;


        GetComponent<SphereCollider>().radius = range;
        GetComponent<SpriteRenderer>().sprite  = collectable.GetIcon();
        GetComponent<ImposterSprite>().sprites = new Sprite[] { collectable.GetIcon() };

        pickupUI = HUDManager.instance.collectableText;
        pickupText = pickupUI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (requireButtonPress && inRange)
        {
            

            if (Input.GetKeyDown(KeyCode.X))
            {
                CollectItem();
            }
        }
    }

    void CollectItem() 
    {
        Inventory.instance.CollectItem(collectable);
        Destroy(gameObject);
        pickupUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            if (!requireButtonPress) 
            {
                CollectItem();   
            } else 
            {
                inRange = true;
                pickupUI.SetActive(true);
                pickupText.text = "Press 'x' to pickup " + collectable.GetName();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player") 
        {
            inRange = false;
            pickupUI.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (inRange == false)
        {
            Gizmos.color = Color.red;
        } else 
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
