using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Reflection;
using System;
using System.Linq;

namespace HCZ.NBPGT.Editor
{
    public struct SearchContextElement
    {
        public object target { get; private set; }
        public string title { get; private set; }

        public SearchContextElement(object target, string title)
        {
            this.target = target;
            this.title = title;
        }
    }

    //Create a context menu with grouped elements for the user to add nodes with
    public class NBPGT_Search : ScriptableObject, ISearchWindowProvider
    {
        public NBPGT_View graph;
        public VisualElement target;
        public static List<SearchContextElement> elements;
        //Creates a search tree with all node options for the user
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            //First entry is always a title
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 0));
            elements = new List<SearchContextElement>();

            //Grab all assemblies, good for future proofing, allows project-specific additions
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.CustomAttributes.ToList() != null)
                    {
                        //If the class has the custom attribute and it's set up, add it to the list of elements
                        var attribute = type.GetCustomAttribute(typeof(ToolNodeInfoAttribute));
                        if (attribute != null)
                        {
                            ToolNodeInfoAttribute att = (ToolNodeInfoAttribute)attribute;
                            var node = Activator.CreateInstance(type);
                            if (!string.IsNullOrEmpty(att.getItem))
                            {
                                elements.Add(new SearchContextElement(node, att.getItem));
                            }
                        }
                    }
                }
            }

            //Element sorting
            elements.Sort((entry1, entry2) =>
            {
                //Create substrings and compare them individually using string.CompareTo
                string[] splits1 = entry1.title.Split('/');
                string[] splits2 = entry2.title.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                    {
                        return 1;
                    }
                    int value = splits1[i].CompareTo(splits2[i]);
                    if (value != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits1.Length - 1 || i == splits2.Length - 1))
                        {
                            return splits1.Length < splits2.Length ? 1 : -1;
                        }
                        return value;
                    }
                }
                return 0;
            });

            //Create element tree after grouping
            List<string> groups = new List<string>();
            foreach (var elemet in elements)
            {
                string[] entryTitle = elemet.title.Split('/');
                string groupName = "";

                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    //Add every unique grouping
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                    //Debug.Log(groupName);
                }
                //Last one is always the element title
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = elemet;
                tree.Add(entry);
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            //Localize mouse position for the window
            var windowMousePos = graph.ChangeCoordinatesTo(graph, context.screenMousePosition - graph.getWindow.position.position);
            var graphMousePos = graph.contentViewContainer.WorldToLocal(windowMousePos);

            SearchContextElement element = (SearchContextElement)SearchTreeEntry.userData;

            NBPGT_Node node = (NBPGT_Node)element.target;
            node.SetPosition(new Rect(graphMousePos, new Vector2()));
            graph.Add(node);

            return true;
        }
    }
}
