using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectingObject : MonoBehaviour
{
    #region Variables
    RaycastHit2D hit2D;
    string prevSelectedObject;
    Vector3 prevSelectedObjectPos;
    public UnityEvent NewObjectSelected;
    public string SelectedObject { get; set; }
    public GameObject SelectedGameObject { get; set; }
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        hit2D = new RaycastHit2D();
        prevSelectedObject = "Null";
        prevSelectedObjectPos = new Vector3(0, 0, 0);
        SelectedObject = "Null";
        SelectedGameObject = null;

        //find listeners
        GameObject[] _listenerBarrackButtons = GameObject.FindGameObjectsWithTag("BarrackButton");
        GameObject[] _listenerPowerPlantButtons = GameObject.FindGameObjectsWithTag("PowerPlantButton");

        //add listeners
        foreach (var listenerBarrack in _listenerBarrackButtons)
        {
            NewObjectSelected.AddListener(listenerBarrack.GetComponent<BarrackButton>().UpdateSelected);
        }
        foreach (var listenerPP in _listenerPowerPlantButtons)
        {
            NewObjectSelected.AddListener(listenerPP.GetComponent<PowerPlantButton>().UpdateSelected);
        }

        //add info menu listener
        NewObjectSelected.AddListener(GameObject.Find("InformationMenu").GetComponent<InformationMenu>().UpdateSelected);
    }

    // Update is called once per frame
    void Update()
    {
        if (AStarPathFinding2D.pathfindingStatus != 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit2D.collider != null)
                {
                    if (hit2D.collider.gameObject.CompareTag("UI") == false)
                    {
                        SelectedObject = hit2D.collider.gameObject.name;
                        SelectedGameObject = hit2D.collider.gameObject;
                    }
                }
                else
                {
                    SelectedObject = "Null";
                    SelectedGameObject = null;
                }

                if (prevSelectedObject != SelectedObject || (hit2D.collider != null && hit2D.collider.gameObject.transform.position != prevSelectedObjectPos && hit2D.collider.gameObject.CompareTag("UI") == false))
                {
                    NewObjectSelected.Invoke();
                    prevSelectedObject = SelectedObject;

                    if (hit2D.collider == null)
                    {
                        prevSelectedObjectPos = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        prevSelectedObjectPos = hit2D.collider.gameObject.transform.position;
                    }

                    Debug.Log(SelectedObject + " is selected");
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.LogWarning("Player is moving. Respect him/her!");
            }
        }
    }
    #endregion

    #region Custom Function
    public void NewObjectCreated(GameObject _newObject)
    {
        NewObjectSelected.AddListener(_newObject.GetComponent<Soldier>().UpdateSelected);
    }
    #endregion
}
