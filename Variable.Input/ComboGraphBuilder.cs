using System;
using System.Collections.Generic;
using System.Linq;

namespace Variable.Input
{
    /// <summary>
    ///     Helper class to construct ComboGraphs programmatically.
    ///     Handles the complexity of flattening edges and calculating indices.
    /// </summary>
    public class ComboGraphBuilder
    {
        private class NodeDefinition
        {
            public int ActionID;
            public List<EdgeDefinition> OutgoingEdges = new();
        }

        private class EdgeDefinition
        {
            public int InputTrigger;
            public int TargetNodeID; // This refers to the index in the _nodes list
        }

        private readonly List<NodeDefinition> _nodes = new();

        /// <summary>
        ///     Adds a new node to the graph.
        /// </summary>
        /// <param name="actionID">The Action ID representing this state.</param>
        /// <returns>The index of the created node (use this for linking edges).</returns>
        public int AddNode(int actionID)
        {
            var node = new NodeDefinition { ActionID = actionID };
            _nodes.Add(node);
            return _nodes.Count - 1;
        }

        /// <summary>
        ///     Adds a transition (edge) between two nodes.
        /// </summary>
        /// <param name="fromNodeIndex">Index of the source node.</param>
        /// <param name="toNodeIndex">Index of the target node.</param>
        /// <param name="inputTrigger">The input ID that triggers this transition.</param>
        public void AddEdge(int fromNodeIndex, int toNodeIndex, int inputTrigger)
        {
            if (fromNodeIndex < 0 || fromNodeIndex >= _nodes.Count)
                throw new ArgumentOutOfRangeException(nameof(fromNodeIndex));
            if (toNodeIndex < 0 || toNodeIndex >= _nodes.Count)
                throw new ArgumentOutOfRangeException(nameof(toNodeIndex));

            _nodes[fromNodeIndex].OutgoingEdges.Add(new EdgeDefinition
            {
                InputTrigger = inputTrigger,
                TargetNodeID = toNodeIndex
            });
        }

        /// <summary>
        ///     Bakes the current definitions into flat arrays ready for the ComboGraph struct.
        /// </summary>
        /// <returns>A tuple containing the Nodes and Edges arrays.</returns>
        public (ComboNode[] nodes, ComboEdge[] edges) Build()
        {
            var builtNodes = new ComboNode[_nodes.Count];
            var allEdges = new List<ComboEdge>();

            for (int i = 0; i < _nodes.Count; i++)
            {
                var def = _nodes[i];
                
                // The start index in the global edge array is the current count of allEdges
                int edgeStartIndex = allEdges.Count;
                int edgeCount = def.OutgoingEdges.Count;

                // Create the node struct
                builtNodes[i] = new ComboNode
                {
                    ActionID = def.ActionID,
                    EdgeStartIndex = edgeStartIndex,
                    EdgeCount = edgeCount
                };

                // Flatten the edges
                foreach (var edgeDef in def.OutgoingEdges)
                {
                    allEdges.Add(new ComboEdge
                    {
                        InputTrigger = edgeDef.InputTrigger,
                        TargetNodeIndex = edgeDef.TargetNodeID
                    });
                }
            }

            return (builtNodes, allEdges.ToArray());
        }
    }
}
