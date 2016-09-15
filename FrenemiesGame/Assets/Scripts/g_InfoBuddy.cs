using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class g_InfoBuddy : MonoBehaviour {

    
    public Text info;
    private LayerMask enemyLayer;
    private LayerMask interactables;
    private Collider2D hitColliders;
    public float circleSize;
    public Color color = Color.white;
    public Material material;
    public Material defaultMaterial;



    void Awake() {
        info.text = "";
        enemyLayer = 1 << 9;
        interactables = 1 << 8;
    
    }

    void ColliderUpdate() {


        hitColliders = Physics2D.OverlapCircle(transform.position, circleSize, enemyLayer | interactables);
        

        if (hitColliders != null)
        {
            if (info.text == "")
            {
                
                if (hitColliders.gameObject.GetComponent<Text>())
                {
                    
                    info.text = hitColliders.gameObject.GetComponent<Text>().text;
                    hitColliders.gameObject.GetComponent<SpriteRenderer>().material = material;
                    

                }
                else
                {
                    info.text = "";
                }
            }
        }
        else
        {
            info.text = "";


        }
    }

    void OnTriggerExit2D() {
        hitColliders.gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
    }
    

    void Update()
    {
        ColliderUpdate();
    }





}
