using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Djikstras
{
    [System.Serializable]
    public class Edge
    {
        List<Node> nodes = new List<Node>();
        float cost = 0.0f;
        bool isValid = true;


        public Edge(Node a_nodeA, Node a_nodeB, float a_cost = 1.0f)
        {
            // Adds nodes as connected nodes and adds this edge to both nodes 'edges' list
            nodes.Add(a_nodeA);
            nodes.Add(a_nodeB);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].AddEdge(this);
            }


            cost = a_cost;
            SetValid(true);

        }

        public void SetValid(bool a_valid)
        {
            isValid = a_valid;
        }

        #region Getters
        public bool IsValid()
        {
            return isValid;
        }

        public float GetCost()
        {
            return cost;
        }

        public List<Node> GetNodes()
        {
            return nodes;
        }
        #endregion
    }
}
