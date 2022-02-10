using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImposterSprite : MonoBehaviour
{
    #region Serialized Settings
    [Header("Sprite Mode")]
    [SerializeField] ImposterSpriteDrawMode drawMode = ImposterSpriteDrawMode.FLAT;

    [Space]
    [Header("Display Sprites")]
    public Sprite[] sprites;

    [Space]
    [Header("General Settings")]
    [SerializeField, Tooltip("Only applies when sprite mode is set to flat.")] public float zRotationalOffset = 0;
    [SerializeField] bool flattenY = false;
    [SerializeField] public bool useOnlyShadow = false;

    [Space]
    [Header("Drop Shadow Settings")]
    [SerializeField] bool castDropShadow;
    [SerializeField] Sprite dropShadow;
    [SerializeField] float dropShadowFalloff;
    [SerializeField] float dropShadowMaxSize;
    #endregion

    #region Local Variables
    GameObject displayGO;
    SpriteRenderer displayRenderer;
    Transform viewer;
    Animator anim;

    GameObject dropShadowGO;
    SpriteRenderer dropShadowRender;
    bool blip = false;
    #endregion
    
    void Awake()
    {

        anim = GetComponent<Animator>();
        viewer = Camera.main.transform;
        displayRenderer = gameObject.GetComponent<SpriteRenderer>();

        //Initialization for non-flat imposter sprites
        //Creates a new object as a child to serve as a display.
        if (drawMode != ImposterSpriteDrawMode.FLAT) 
        {
            Destroy(this.GetComponent<SpriteRenderer>());
            
            displayGO = new GameObject("DisplayObject");
            
            displayGO.transform.SetParent(transform);
            displayGO.transform.position = transform.position;
            displayGO.transform.localScale = Vector3.one;
            
            displayRenderer = displayGO.AddComponent<SpriteRenderer>();
            displayRenderer.sprite = sprites[0];

            displayRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        } else 
        {
            sprites = new Sprite[] { displayRenderer.sprite };
            displayGO = gameObject;
        }

        //Initialization for drop shadows
        if (castDropShadow) 
        {

            dropShadowGO = new GameObject("DropShadow");

            dropShadowGO.transform.SetParent(transform);
            dropShadowGO.transform.position = transform.position;
            dropShadowGO.transform.localEulerAngles = Vector3.one;

            dropShadowRender = dropShadowGO.AddComponent<SpriteRenderer>();
            dropShadowRender.sprite = dropShadow;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToView;
        float angleBetween;
        int spriteStep;

        if (!useOnlyShadow) 
        {
            if (!blip)
            {
                switch (drawMode)
                {
                    case ImposterSpriteDrawMode.FLAT:
                        Vector3 faceDirection;

                        if (flattenY)
                        {
                            faceDirection = (transform.position - new Vector3(viewer.position.x, transform.position.y, viewer.position.z)).normalized;
                        }
                        else
                        {
                            faceDirection = (transform.position - viewer.position).normalized;
                        }


                        transform.forward = faceDirection;
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotationalOffset);
                        break;

                    case ImposterSpriteDrawMode.TWO_SIDED:
                        displayGO.transform.LookAt(new Vector3(viewer.position.x, displayGO.transform.position.y, viewer.position.z));
                        directionToView = (transform.position - viewer.position).normalized;
                        angleBetween = Mathf.Repeat(Vector2.SignedAngle(new Vector2(directionToView.x, directionToView.z), new Vector2(transform.forward.x, transform.forward.z)) + 180 + 22.5f, 360);
                        spriteStep = (int)angleBetween.Remap(0, 360, 0, 2);
                        displayRenderer.sprite = sprites[spriteStep];
                        break;

                    case ImposterSpriteDrawMode.EIGHT_SIDED:
                        displayGO.transform.LookAt(new Vector3(viewer.position.x, displayGO.transform.position.y, viewer.position.z));
                        directionToView = (transform.position - viewer.position).normalized;
                        angleBetween = Mathf.Repeat(Vector2.SignedAngle(new Vector2(directionToView.x, directionToView.z), new Vector2(transform.forward.x, transform.forward.z)) + 180 + 22.5f, 360);
                        spriteStep = (int)angleBetween.Remap(0, 360, 0, 8);
                        displayRenderer.sprite = sprites[spriteStep];
                        break;
                }
            }
            else
            {
                displayGO.transform.LookAt(new Vector3(viewer.position.x, transform.position.y, viewer.position.z));
            }
        }
       

      
       
    }


     void LateUpdate()
    {
        if (castDropShadow)
        {
            Ray dropRay = new Ray(this.transform.position, Vector3.down * dropShadowFalloff);
            RaycastHit hit;

            if (Physics.Raycast(dropRay, out hit, dropShadowFalloff))
            {
                dropShadowGO.SetActive(true);
                dropShadowGO.transform.position = hit.point + hit.normal * .03f;
                dropShadowGO.transform.forward = hit.normal;

                float dropoffAmount = transform.position.y - dropShadowGO.transform.position.y;
                float scalar = Mathf.Lerp(dropShadowMaxSize, 0, dropoffAmount / dropShadowFalloff);

                dropShadowGO.transform.localScale = new Vector3(scalar, scalar);

            }
            else
            {
                dropShadowGO.SetActive(false);
            }


        }
    }

    Coroutine activeBlip = null;
    string activeAnimationName;
    public void AnimateBlip(Sprite blipSprite, float blipLength, Color blipColor)
    {

        displayRenderer.sprite = blipSprite;
        displayRenderer.color = blipColor;
        blip = true;

        if(activeBlip != null)
            StopCoroutine(activeBlip);

        activeBlip = StartCoroutine(blipManager(blipLength));
    }
    IEnumerator blipManager(float length) 
    {
        yield return new WaitForSeconds(length);
        displayRenderer.color = Color.white;
        blip = false;
        if(drawMode == ImposterSpriteDrawMode.FLAT) 
        {
            displayRenderer.sprite = sprites[0];
        }
        activeBlip = null;
    }

    public void SetImposterSprite(Sprite[] _sprites, ImposterSpriteDrawMode _drawMode, bool _useDropShadow = false, float _zSpriteOffset = 0) 
    {
        ImposterSpriteDrawMode pDrawMode = drawMode;

        sprites = _sprites;
        drawMode = _drawMode;
        zRotationalOffset = _zSpriteOffset;

        if(_drawMode == ImposterSpriteDrawMode.FLAT)
        {

            displayRenderer.sprite = _sprites[0];
            
            if (activeBlip != null)
                StopCoroutine(activeBlip);

            displayRenderer.color = Color.white;
            blip = false;
            activeBlip = null;

            if(pDrawMode != ImposterSpriteDrawMode.FLAT) 
            {
                displayGO.transform.forward = transform.forward;
            }
        }

        if(!_useDropShadow && castDropShadow) 
        {
            castDropShadow = false;
            Destroy(dropShadowGO);
        } 
        else if (_useDropShadow && !castDropShadow) 
        {
            castDropShadow = true;
            dropShadowGO = new GameObject("DropShadow");

            dropShadowGO.transform.SetParent(transform);
            dropShadowGO.transform.position = transform.position;
            dropShadowGO.transform.localEulerAngles = Vector3.one;

            dropShadowRender = dropShadowGO.AddComponent<SpriteRenderer>();
            dropShadowRender.sprite = dropShadow;
            
        }
    }


}

public enum ImposterSpriteDrawMode
{
    FLAT,
    TWO_SIDED,
    EIGHT_SIDED
}
