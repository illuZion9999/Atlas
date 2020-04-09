using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using Rivers;
using Rivers.Collections;

namespace Atlas.Renamer
{
    class NameGraph
    {
        readonly Graph _graph = new Graph();

        internal NodeCollection Nodes => _graph.Nodes;

        internal void Add(IMDTokenProvider def)
        {
            _graph.Nodes.Add(GetOrCreateNode(def));
        }

        internal void Remove(IMDTokenProvider def)
        {
            _graph.Nodes[def.MDToken.ToInt32().ToString()].UserData["rename"] = false;
        }
        
        internal void Link(IMDTokenProvider from, IMDTokenProvider to)
        {
            var nodeFrom = GetOrCreateNode(from);
            var nodeTo = GetOrCreateNode(to);

            _graph.Edges.Add(nodeFrom, nodeTo);
        }
        
        internal IEnumerable<Node> GetDerivedNodes(Node root)
        {
            return root.IncomingEdges.Select(incoming => incoming.Source);
        }

        internal IEnumerable<Node> GetRootNodes()
        {
            return Nodes.Where(node => node.OutgoingEdges.Count < 1);
        }

        Node GetOrCreateNode(IMDTokenProvider def)
        {
            var token = def.MDToken.ToInt32().ToString();
            if (_graph.Nodes.TryGetNode(token, out var existing)) return existing;
            
            var node = new Node(token);
            node.UserData["def"] = def;
            node.UserData["rename"] = true;
            _graph.Nodes.Add(node);

            return node;
        }
    }
}