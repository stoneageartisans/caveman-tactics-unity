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
        // TODO
    }

    void OnMouseExit()
    {
        // TODO
    }

    void OnMouseOver()
    {
        if( isSelectable )
        {
            if( Input.GetMouseButtonDown( 0 ) )
            {
                if( !isOccupied )
                {
                    ApplicationManager.instance.placePlayerToken( gameObject.transform.position, id );
                }
            }

            if( Input.GetMouseButtonDown( 1 ) )
            {
                int direction = ApplicationManager.instance.player.facing;

                direction ++;

                if( direction == Constants.MAX_DIRECTIONS )
                {
                    direction = Constants.EAST;
                }

                ApplicationManager.instance.rotatePlayerToken( direction );
            }
        }

    }

    void Awake()
    {
        id = ( column + "," + row );
    }

	void Start()
    {
        currentMaterial = gameObject.GetComponent<Renderer>().material;
	}
	
	void Update()
    {
        // TODO
	}

    int getDirection( float angle )
    {
        if( angle > 30 && angle <= 90 )
        {
            return Constants.SOUTHEAST;
        }
        else if( angle > 90 && angle <= 150 )
        {
            return Constants.SOUTHEAST;
        }
        else if( angle > 150 && angle <= 210 )
        {
            return Constants.SOUTHEAST;
        }
        else if( angle > 150 && angle <= 210 )
        {
            return Constants.SOUTHEAST;
        }
        else if( angle > 150 && angle <= 210 )
        {
            return Constants.SOUTHEAST;
        }
        else
        {
            return Constants.EAST;
        }
    }
    
    public void setOverlay( Constants.HexOverlay overlay )
    {
        switch( overlay )
        {
            case Constants.HexOverlay.Blank:
                currentMaterial.SetTexture( "_MainTex", blank );
                break;
            case Constants.HexOverlay.Highlighted:
                currentMaterial.SetTexture( "_MainTex", highlighted );
                break;
            case Constants.HexOverlay.Obstructed:
                currentMaterial.SetTexture( "_MainTex", obstructed );
                break;
            default:
                currentMaterial.SetTexture( "_MainTex", blank );
                break;
        }
    }
}
