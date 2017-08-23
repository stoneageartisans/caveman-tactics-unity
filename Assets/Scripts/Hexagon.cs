using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexagon : MonoBehaviour
{
    public int column;
    public int row;
    public Texture blank;
    public Texture highlighted;
    public Texture obstructed;

    Material currentMaterial;

    [HideInInspector]
    public bool isSelectable = false;

    [HideInInspector]
    public bool isHighlighted = false;

    [HideInInspector]
    public bool isObstructed = false;

    [HideInInspector]
    public bool isOccupied = false;

    [HideInInspector]
    public string id;

    void OnMouseEnter()
    {
        /*if( isSelectable )
        {
            if( !isObstructed && !isOccupied )
            {
                isHighlighted = true;

                currentMaterial.SetTexture( "_MainTex", highlighted ); 
            }
        }*/
    }

    void OnMouseExit()
    {
        /*if( isSelectable )
        {
            if( isHighlighted )
            {
                isHighlighted = false;

                currentMaterial.SetTexture( "_MainTex", blank ); 
            }
        }*/
    }

    void OnMouseOver()
    {
        if( isSelectable )
        {
            if( Input.GetMouseButtonDown( 0 ) )
            {
                if( !isOccupied )
                {
                    ApplicationManager.instance.placePlayerToken( gameObject.transform.position );
                }
            }
        }
    }

	void Start()
    {
        id = column + ", " + row;

        currentMaterial = gameObject.GetComponent<Renderer>().material;
	}
	
	void Update()
    {
        // TODO
	}
}
