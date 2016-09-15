using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class g_InfoBuddy : MonoBehaviour {

    
    private Text info;
    private LayerMask enemyLayer;
    private LayerMask interactables;
    private Collider2D hitColliders;
    public float circleSize;
    public Color color = Color.white;
    public Material material;
    public Material defaultMaterial;
    private GameObject objectTouched;



    void Awake() {
        info = GameObject.FindGameObjectWithTag("infoBuddyText").GetComponent<Text>();
        info.text = "";
        info.SetNativeSize();
        enemyLayer = 1 << 9;
        interactables = 1 << 8;
        objectTouched = GameObject.FindGameObjectWithTag("defaultInfo");
    }


    void ColliderUpdate()
    {
        hitColliders = Physics2D.OverlapCircle(transform.position, circleSize, enemyLayer | interactables);
        if (hitColliders != null) { objectTouched = hitColliders.gameObject; OnCircleCollide(hitColliders, objectTouched); }
        if (hitColliders == null) { OnCircleNotCollide(hitColliders, objectTouched); objectTouched = GameObject.FindGameObjectWithTag("defaultInfo"); }
        info.SetNativeSize();
    }


    void OnCircleCollide(Collider2D circle, GameObject obj)
    {
        if (circle != null) {
            HighlightObject(obj);
            if (info.text == "") { SetText(info, obj.GetComponent<Text>().text); }
            
        }
    }

    void OnCircleNotCollide(Collider2D circle, GameObject obj)
    {
            UnHighlightObject(obj);
        if (info.text != "") { SetText(info, ""); }
           
    }


    void HighlightObject(GameObject obj)
    {
        if (!obj.GetComponent<SpriteRenderer>()) { return; }
        obj.GetComponent<SpriteRenderer>().material = material;
    }

    void UnHighlightObject(GameObject obj)
    {
        if (!obj.GetComponent<SpriteRenderer>()) { return; }
        obj.GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

   

    void SetText(Text textToSet, string text) {
        textToSet.text = text;
    }
    

    void Update()
    {
        ColliderUpdate();
       
       
       
    }





}
