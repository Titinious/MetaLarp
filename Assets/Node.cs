using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [SerializeField] private Image backgroundImg;
    [SerializeField] private Image highlightImg;
    [SerializeField] private Text nodeInfo;

    public void Selected()
    {
        backgroundImg.gameObject.SetActive(false);
        highlightImg.gameObject.SetActive(true);
    }

    public void Deselected()
    {
        backgroundImg.gameObject.SetActive(true);
        highlightImg.gameObject.SetActive(false);
    }

    public Node FindParent()
    {
        if (transform.parent.gameObject.GetComponent<Node>() == null)
        {
            return null;
        }
        else
        {
            return transform.parent.gameObject.GetComponent<Node>();
        }
            
    }

    public List<Node> FindAllKins ()
    {
        Transform[] children = transform.parent.GetComponentsInChildren<Transform>();
        List<Node> allChildren = new List<Node>();

        foreach (Transform child in children)
        {
            if (child.parent == transform.parent && child.gameObject.GetComponent<Node>() != null)
            {
                allChildren.Add(child.GetComponent<Node>());
            } 
        }
        if(allChildren.Count > 0) 
        { 
            return allChildren; 
        } 
        else
        {
            return null;
        }
        
    }

    public Node FindFirstChild()
    {

            Transform[] children = transform.GetComponentsInChildren<Transform>();

            foreach (Transform child in children)
            {
                if (child.parent == transform && child.gameObject.GetComponent<Node>() != null)
                {
                    return child.gameObject.GetComponent<Node>();
                }
                    
            }

        return null;
            
    }
        
}
