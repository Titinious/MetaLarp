using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    [SerializeField] private int kinIndex = 0;
    [SerializeField] private List<Node> kinList;

    [SerializeField] private Node rootNode;
    private Node CurrentNode
    {
        get
        {
            if (_currentNode == null) 
            {
                _currentNode = rootNode;
            }

            return _currentNode;
        }

        set 
        { 
            if (value != null)
            {
                if (_currentNode != null)
                {
                    // Deselect the current Node
                    _currentNode.Deselected();
                }
                _currentNode = value;
                _currentNode.Selected();
                kinList = _currentNode.FindAllKins();
            }
        }
    }
    private Node _currentNode;


    // Start is called before the first frame update
    void Start()
    {
        CurrentNode = rootNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Move to the parent Node
            CurrentNode = CurrentNode.FindParent();
            kinIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Move to the child Node
            CurrentNode = CurrentNode.FindFirstChild();
            kinIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(kinList.Count > 0)
            {
                // Move to last Node
                if (kinIndex > 0)
                {
                    kinIndex -= 1;
                }
                else
                {
                    kinIndex = kinList.Count - 1;
                }

                CurrentNode = kinList[kinIndex];
            }

        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (kinList.Count > 0)
            {
                // Move to next Node
                if (kinIndex < kinList.Count - 1)
                {
                    kinIndex += 1;
                }
                else
                {
                    kinIndex = 0;
                }

                CurrentNode = kinList[kinIndex];
            }

        }
    }
}
