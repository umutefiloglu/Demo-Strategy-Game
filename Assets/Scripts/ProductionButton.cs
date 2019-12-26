using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ProductionButton : MonoBehaviour
{
    #region Variables
    bool canBeDragged;
    string selectedObject;
    Vector2 oldPosition;
    Vector2 startingOffset;
    Vector3 mousePosition;
    public UnityEvent UpdateTileEvent;

    protected GameObject[] tiles;
    protected GameObject affordance;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    public void Start()
    {
        canBeDragged = false;
        selectedObject = "Null";
        oldPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        tiles = GameObject.FindGameObjectsWithTag("Tile");
       
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //add listeners
        //o(n)
        foreach (var tile in tiles)
        {
            UpdateTileEvent.AddListener(tile.GetComponent<Tile>().UpdateTile);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        OnDragged();
    }
    #endregion

    #region Custom Functions
    //listener function update selected
    public void UpdateSelected()
    {
        selectedObject = GameObject.Find("GameManager").GetComponent<SelectingObject>().SelectedObject;
    }

    //Drags object
    void OnDragged()
    {
        //if object is selected..
        if (canBeDragged == true)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //drag object
            gameObject.transform.position = new Vector3(mousePosition.x - startingOffset.x, mousePosition.y - startingOffset.y, gameObject.transform.position.z);

            //show build area
            ShowBuildArea();
        }
    }

    private void OnMouseDown()
    {
        if (AStarPathFinding2D.pathfindingStatus != 1)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            startingOffset = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            canBeDragged = true;

            //design concern, not necessary
            gameObject.transform.position = new Vector3(mousePosition.x - startingOffset.x, mousePosition.y - startingOffset.y, gameObject.transform.position.z - 1.5f);
            //

            Debug.Log(gameObject.name + " can be dragged!");
        }
        else
        {
            Debug.LogWarning("Player is moving. Respect him/her!");
        }
    }

    private void OnMouseUp()
    {
        //go back button
        gameObject.transform.position = new Vector3(oldPosition.x, oldPosition.y, 0);

        //go back affordance
        if (affordance.name == "BarrackTileAffordance")
        {
            affordance.transform.position = new Vector3(-12, 0, 0);
        }
        else
        {
            affordance.transform.position = new Vector3(-12, -4, 0);
        }

        OnBuilt();

        canBeDragged = false;

        Debug.Log(gameObject.name + " can't be dragged!");
    }

    //When object is built
    void OnBuilt()
    {
        if (IsBuildable() == true)
        {
            CreateBuilding();
            UpdateTileEvent.Invoke();
        }
    }

    public abstract void ShowBuildArea();
    public abstract bool IsBuildable();
    public abstract void CreateBuilding();
    #endregion
}
