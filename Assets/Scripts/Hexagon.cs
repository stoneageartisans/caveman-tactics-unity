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
        if(isSelectable)
        {
            if( Input.GetMouseButtonDown(Constants.LEFT_MOUSE) )
            {
                if(!isOccupied)
                {
                    ApplicationManager.instance.placePlayerToken(id);

                    if(ApplicationManager.instance.newGameStarted)
                    {
                        ApplicationManager.instance.newGameStarted = false;
                    }
                }
            }

            if( Input.GetMouseButtonDown(Constants.RIGHT_MOUSE) )
            {
                if( isOccupied && id.Equals(ApplicationManager.instance.player.currentHexId) )
                {
                    int direction = ApplicationManager.instance.player.facing;

                    direction ++;

                    if(direction == Constants.MAX_DIRECTIONS)
                    {
                        direction = Constants.EAST;
                    }

                    ApplicationManager.instance.rotatePlayerToken(direction);
                }
            }
        }
    }

    void Awake()
    {
        id = (column + "," + row);
    }

	void Start()
    {
        currentMaterial = gameObject.GetComponent<Renderer>().material;
	}
	
	void Update()
    {
        // TODO
	}
    
    public void setOverlay(Constants.HexOverlay overlay)
    {
        switch(overlay)
        {
            case Constants.HexOverlay.Blank:
                currentMaterial.SetTexture("_MainTex", blank);
                break;
            case Constants.HexOverlay.Highlighted:
                currentMaterial.SetTexture("_MainTex", highlighted);
                break;
            case Constants.HexOverlay.Obstructed:
                currentMaterial.SetTexture("_MainTex", obstructed);
                break;
            default:
                currentMaterial.SetTexture("_MainTex", blank);
                break;
        }
    }
}
