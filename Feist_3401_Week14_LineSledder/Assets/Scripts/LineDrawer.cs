using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public float minimumSegmentLength = 0.5f;


    private Vector3 _previousMousePosition;
    private LineRenderer _currentLine;
    private List <GameObject> _undoBuffer;

    //public LineRenderer line;
    //public EdgeCollider2D edge;


    // Start is called before the first frame update
    void Start()
    {
        _undoBuffer = new List <GameObject> ();
    }


    // Update is called once per frame
    void Update()
    {
        // Spawn line prefab when we click mouse
        if (Input.GetMouseButtonDown (0))
        {
            _currentLine = Instantiate (linePrefab).GetComponent <LineRenderer> ();
            _previousMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        }


        // If we have a current line, update it
        if (_currentLine != null)
        {
            // Get current mouse position
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            currentMousePosition.z = 0;

            // Has the mouse moved far enough?
            if (Vector3.Distance (_previousMousePosition, currentMousePosition) > minimumSegmentLength)
            {
                // Add new position to line where the mouse currently is
                _currentLine.positionCount += 1;
                _currentLine.SetPosition(_currentLine.positionCount - 1, currentMousePosition);

                // Copy line points into array
                EdgeCollider2D currentEdge = _currentLine.GetComponent <EdgeCollider2D> ();
                Vector2 [] edgePoints = new Vector2 [_currentLine.positionCount];
                for (int i = 0; i < edgePoints.Length; i++)
                {
                    edgePoints[i] = _currentLine.GetPosition (i);
                }

                // Assign collider points
                currentEdge.points = edgePoints;


                // Set prev mouse pos for next frame
                _previousMousePosition = currentMousePosition;
            }


        }



        // Stop drawing when we release
        if (Input.GetMouseButtonUp (0))
        {
            // If our line is too short, delete it
            if (_currentLine.positionCount < 2)
            {
                Destroy (_currentLine.gameObject);
            }
            else
            { 
                _undoBuffer.Add (_currentLine.gameObject);
            }

            _currentLine = null;
        }


        if (Input.GetKeyDown (KeyCode.U))
        {
            Destroy (_undoBuffer [_undoBuffer.Count - 1]);
            _undoBuffer.RemoveAt(_undoBuffer.Count - 1);
        }

        /*
        if (Input.GetMouseButtonDown (0))
        {
            // Get world position of mouse cursor
            Vector3 clickPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            clickPoint.z = 0;

            // Add a new point to our line
            line.positionCount += 1;

            // Set the last position (the one we just added) to our mouse position
            line.SetPosition (line.positionCount - 1, clickPoint);

            // Add line points to array for collider
            Vector2[] edgePoints = new Vector2 [line.positionCount];
            for (int i = 0; i < edgePoints.Length; i++)
            {
                edgePoints [i] = line.GetPosition (i);
            }

            // Set collider points
            edge.points = edgePoints;
        }
        */


    }
}
