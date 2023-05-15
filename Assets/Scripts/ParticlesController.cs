using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController: MonoBehaviour{
    public Color paintColor;

    public Material mat;
    
    public float minRadius = 0.05f;
    public float maxRadius = 0.2f;
    public float strength = 1;
    public float hardness = 1;
    [Space]
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    static public GameObject col; 

    void Start(){
        paintColor = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );

        // Texture2D texture = (Texture2D)mat.GetTexture("_BaseMap");
 
        // set the pixel values
        // var fillColorArray =  texture.GetPixels();
 
        // for(var i = 0; i < fillColorArray.Length; ++i)
        // {
        //     fillColorArray[i] = paintColor;
        // }
        // texture.SetPixels(fillColorArray);
        // texture.Apply();

        // mat.SetTexture("_BaseMap", texture);
        //mat.SetColor("_Color", paintColor);
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        //var pr = part.GetComponent<ParticleSystemRenderer>();
        //Color c = new Color(pr.material.color.r, pr.material.color.g, pr.material.color.b, .8f);
        //paintColor = c;
    }

    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        if (other.tag == "Objetive") {
            if (!ShootingSystem.reach[other.transform.GetSiblingIndex()]) {
                ShootingSystem.reached += 1;
                ShootingSystem.reach[other.transform.GetSiblingIndex()] = true;
            }
            col = other.transform.parent.Find("Canvas Objetivos").Find("Image" + other.transform.GetSiblingIndex()).gameObject;
            col.SetActive(true);
            MovementInput.locked = true;
        }

        Paintable p = other.GetComponent<Paintable>();
        if(p != null){
            for  (int i = 0; i< numCollisionEvents; i++){
                Vector3 pos = collisionEvents[i].intersection;
                float radius = Random.Range(minRadius, maxRadius);
                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            }
        }
    }
}