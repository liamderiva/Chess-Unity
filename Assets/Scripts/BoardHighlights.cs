using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour {

	public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;  //prefab object for highlights
    private List<GameObject> Highlights;    //list of highlights

    private void Start()
    {
        Instance = this;    
        Highlights = new List<GameObject>();    //instantiate list of highlights as list of GameObjects
    }

    private GameObject GetHighlightObject() 
    {
        GameObject go = Highlights.Find(g => !g.activeSelf);    //look for active objects 

        if(go == null)
        {
            go = Instantiate(highlightPrefab);  //instantiate a prefab for the object (floor of board)
            Highlights.Add(go); //add the object to the highlights list
        }

        return go;  //return the object
    }

    public void HighlightAllowedMoves(bool[,] moves)    //highlight floor when looking for possible moves
    {
        for (int i = 0; i < 8; i++) //loop through x position
        {
            for (int j = 0; j < 8; j++) //loop through y position
            {
                if (moves[i, j])    //check if moves exist at those positions
                {
                    GameObject go = GetHighlightObject();
                    go.SetActive(true); //set object to active
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f); //tansform highlight to possible move position
                }
            }
        }
    }

    public void HideHighlights()    //hiding highlights when not looking for move
    {
        foreach(GameObject go in Highlights)    //for each object in list of highlights
        {
            go.SetActive(false);    //no longer active objects
        }
    }

}
