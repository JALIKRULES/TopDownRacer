using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarLayerHandler : MonoBehaviour
{
    public SpriteRenderer carOutlineSpriteRenderer; 

    List<SpriteRenderer> defaultLayerSpriteRenderers = new List<SpriteRenderer>();

    List<Collider2D> overpassColliderList = new List<Collider2D>();
    List<Collider2D> underpassColliderList = new List<Collider2D>();

    Collider2D carCollider;

    bool isDrivingOnOverpass = false;

    private void Awake()
    {
        foreach(SpriteRenderer spriteRenderer in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            if(spriteRenderer.sortingLayerName == "Default")
                defaultLayerSpriteRenderers.Add(spriteRenderer);
        }

        foreach (GameObject overpassColliderGameObject in GameObject.FindGameObjectsWithTag("OverpassCollider"))
        {
            overpassColliderList.Add(overpassColliderGameObject.GetComponent<Collider2D>());
        }

        foreach (GameObject underpassColliderGameObject in GameObject.FindGameObjectsWithTag("UnderpassCollider"))
        {
            underpassColliderList.Add(underpassColliderGameObject.GetComponent<Collider2D>());
        }

        carCollider = GetComponentInChildren<Collider2D>();
    }

    void Start()
    {
        UpdateSortingAndCollisionLayers();
    }

    
    void UpdateSortingAndCollisionLayers()
    {
        if (isDrivingOnOverpass)
        {
            SetSortingLayer("RaceTrackOverpass");

            carOutlineSpriteRenderer.enabled = false;
        }
        else
        {
            SetSortingLayer("Default");
            carOutlineSpriteRenderer.enabled = true;
        }

        SetCollisionWithOverpass();
    }

    void SetCollisionWithOverpass()
    {
        foreach(Collider2D collider2D in overpassColliderList)
        {
            Physics2D.IgnoreCollision(carCollider, collider2D , !isDrivingOnOverpass);
        }

        foreach (Collider2D collider2D in underpassColliderList)
        {
            if(isDrivingOnOverpass)
                Physics2D.IgnoreCollision(carCollider, collider2D, true);
            else Physics2D.IgnoreCollision(carCollider, collider2D, false);

        }
    }
    void SetSortingLayer(string layerName)
    {
        foreach(SpriteRenderer spriteRenderer in defaultLayerSpriteRenderers)
        {
            spriteRenderer.sortingLayerName = layerName;
        }
    }
   
    public bool IsDrivingOverpass()
    {
        return isDrivingOnOverpass;
    }

    private void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("UnderpassTrigger"))
        {
            isDrivingOnOverpass = false;

            UpdateSortingAndCollisionLayers();
        }
        else if (collider2d.CompareTag("OverpassTrigger"))
        {
            isDrivingOnOverpass = true;

            UpdateSortingAndCollisionLayers();
        }
    }
}
