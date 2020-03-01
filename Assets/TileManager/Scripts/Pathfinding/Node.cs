using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Djikstras
{
    public class Node : MonoBehaviour
    {
        [SerializeField] float gScore = 0.0f;
        Node previousNode = null;
        [SerializeField] List<Edge> edges = new List<Edge>();
        [SerializeField] IntVector2 gridCoordinates;

        public void AddEdge(Edge a_edge)
        {
            edges.Add(a_edge);
        }

        public void RemoveEdge(Edge a_edge)
        {
            if (edges.Contains(a_edge))
                edges.Remove(a_edge);
        }

        void ResetNode()
        {
            gScore = 0.0f;
            previousNode = null;
        }

        public void SetValid(bool a_isValid)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                edges[i].SetValid(a_isValid);
            }
        }

        void ToggleValidity()
        {

        }

        // @brief Returns true if any connected edges are valid
        public bool IsValid()
        {
            for (int i = 0; i < edges.Count; i++)
            {
                // If any connected edges are valid, return true.
                if (edges[i].IsValid())
                    return true;
            }

            return false;
        }

        #region Getters
        public float GetGScore()
        {
            return gScore;
        }

        public List<Edge> GetEdges()
        {
            return edges;
        }

        public Node GetPreviousNode()
        {
            return previousNode;
        }

        public IntVector2 GetGridCoordinates()
        {
            return gridCoordinates;
        }

        #endregion

        #region Setters
        void SetGScore(float a_gScore)
        {
            gScore = a_gScore;
        }

        void SetEdges(List<Edge> a_edges)
        {
            edges = a_edges;
        }

        void SetPreviousNode(Node a_node)
        {
            previousNode = a_node;
        }

        public void SetGridCoordinates(IntVector2 a_coords)
        {
            gridCoordinates = a_coords;
        }
        #endregion
    }
}
