using System;
using System.Text;

namespace Variable.Input
{
    /// <summary>
    ///     Extension methods for visualizing the combo graph and state.
    /// </summary>
    public static class ComboVisualizationExtensions
    {
        /// <summary>
        ///     Generates a Mermaid diagram string representing the current state of the combo graph.
        ///     Useful for debugging and visualization.
        /// </summary>
        /// <param name="state">The current runtime state.</param>
        /// <param name="graph">The static graph structure.</param>
        /// <param name="getActionName">Optional function to resolve Action IDs to names.</param>
        /// <param name="getInputName">Optional function to resolve Input IDs to names.</param>
        /// <returns>A string containing the Mermaid diagram definition.</returns>
        public static string ToMermaid(
            this ComboState state, 
            ComboGraph graph, 
            Func<int, string>? getActionName = null, 
            Func<int, string>? getInputName = null)
        {
            if (graph.NodeCount == 0) return "graph TD\n    Error[Graph Empty]";

            var sb = new StringBuilder();
            sb.AppendLine("graph TD");

            var nodes = graph.NodesSpan;
            var edges = graph.EdgesSpan;

            // 1. Define Nodes and Edges
            for (int i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                bool isCurrent = i == state.CurrentNodeIndex;
                
                // Resolve Name
                string name = getActionName?.Invoke(node.ActionID) ?? $"Action {node.ActionID}";
                
                // Add Status if current
                if (isCurrent)
                {
                    string status = state.IsActionBusy ? "BUSY" : "WAITING";
                    name = $"{name}<br/>[{status}]";
                }

                // Define Node
                sb.AppendLine($"    N{i}[\"{name}\"]");

                // Define Outgoing Edges
                if (node.EdgeCount > 0)
                {
                    for (int e = 0; e < node.EdgeCount; e++)
                    {
                        int edgeIdx = node.EdgeStartIndex + e;
                        if (edgeIdx < edges.Length)
                        {
                            var edge = edges[edgeIdx];
                            string input = getInputName?.Invoke(edge.InputTrigger) ?? $"Input {edge.InputTrigger}";
                            
                            // Highlight valid transitions from current node if not busy
                            string linkStyle = (isCurrent && !state.IsActionBusy) ? "==>" : "-->";
                            
                            sb.AppendLine($"    N{i} {linkStyle}|{input}| N{edge.TargetNodeIndex}");
                        }
                    }
                }
            }

            // 2. Style the Current Node
            sb.AppendLine();
            sb.AppendLine($"    %% Current State Styling");
            string color = state.IsActionBusy ? "#ff4444" : "#44ff44"; // Red for busy, Green for waiting
            sb.AppendLine($"    style N{state.CurrentNodeIndex} fill:{color},stroke:#333,stroke-width:4px,color:black");

            return sb.ToString();
        }


    }
}
