using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Word_Generator.Editor
{
    /// <summary>
    /// Editor window for grid debugging.
    /// It was developed assuming a 2D structure for the grid and ignoring the Y axis.
    /// </summary>
    public class GridDebugger : EditorWindow
    {
        private const int CellSize = 30;
        private const string Title = "Grid Debugger";
        private const string SpawnIcon = "Avatar Icon";
        private const string NormalIcon = "MeshFilter Icon";
        private const string BossIcon = "redLight";
        private const string TrapIcon = "LensFlare Gizmo";
        private int selection = 0;
        private Vector2 scrollPos;
        private Grid _grid;

        private Dictionary<RNode_Type, GUIContent> nodeIconsByType;

        [MenuItem("Tools/World/Grid Debugger")]
        public static void OpenWindow()
        {
            var w = GetWindow<GridDebugger>();
            TryGetGrid(out w._grid);
            w.titleContent.text = Title;
            w.Show();
        }

        private void OnEnable()
        {
            nodeIconsByType = new()
            {
                { RNode_Type.Empty ,new GUIContent
                    {
                        tooltip = RNode_Type.Empty.ToString(),
                    }
                },
                { RNode_Type.Spawn , new GUIContent(EditorGUIUtility.IconContent(SpawnIcon))
                    {
                        tooltip = RNode_Type.Spawn.ToString(),
                    }
                },
                { RNode_Type.Trap , new GUIContent(EditorGUIUtility.IconContent(TrapIcon))
                    {
                        tooltip = RNode_Type.Trap.ToString(),
                    }
                },
                { RNode_Type.Normal , new GUIContent(EditorGUIUtility.IconContent(NormalIcon))
                    {
                        tooltip = RNode_Type.Normal.ToString(),
                    }
                },
                { RNode_Type.Boss , new GUIContent(EditorGUIUtility.IconContent(BossIcon))
                    {
                        tooltip = RNode_Type.Boss.ToString(),
                    }
                },
            };
        }

        private void OnGUI()
        {
            if (_grid == null && !TryGetGrid(out _grid))
                return;

            #region Info

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector2IntField("Selection Coords",
                new Vector2Int(selection % _grid.Size.x + 1,
                    selection / _grid.Size.x + 1),
                GUILayout.ExpandWidth(true));

            EditorGUILayout.Vector2IntField("Grid Size", _grid.Size,
                GUILayout.ExpandWidth(true));
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            #endregion
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (!Application.isPlaying)
            {
                GUILayout.Label("Only available in Play Mode", EditorStyles.centeredGreyMiniLabel);
                return;
            }
            
            GUILayout.BeginHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            GUIContent[] contents = new GUIContent[_grid.Size.x * _grid.Size.y];
            for (int y = 0; y < _grid.Size.y; y++)
            {
                for (int x = 0; x < _grid.Size.x; x++)
                {
                    var content = nodeIconsByType[RNode_Type.Empty];
                    try
                    {
                        if (nodeIconsByType.ContainsKey(_grid[x, y].type))
                            content = nodeIconsByType[_grid[x, y].type];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.LogError($"Coordinates out of grid range!\nx: {x} y: {y}");
                    }
                    
                    var contentsCoords = x + y * _grid.Size.x;
                    contents[contentsCoords] = content;
                }
            }

            var gridLayoutDimensions = GetGridLayoutDimensions(CellSize, _grid.Size);
            selection = GUILayout.SelectionGrid(selection, contents, _grid.Size.x,
                                                GUILayout.Width(gridLayoutDimensions.x),
                                                GUILayout.Height(gridLayoutDimensions.y));
            GUILayout.EndScrollView();
            GUILayout.BeginVertical(GUILayout.Width(200));
            var labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Options", labelStyle);

            // Type popup
            var normalWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            var nodeSelection = _grid[selection % _grid.Size.y, selection/_grid.Size.x];
            nodeSelection.type = (RNode_Type) EditorGUILayout.EnumPopup("Type", nodeSelection.type);
            EditorGUIUtility.labelWidth = normalWidth;

            // State Popup
            normalWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            nodeSelection.state = (Node_States)EditorGUILayout.EnumPopup("State", nodeSelection.state);
            EditorGUIUtility.labelWidth = normalWidth;

            // State Popup
            normalWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.IntField("Possible_Types",nodeSelection.possible_Types.Count);
            EditorGUIUtility.labelWidth = normalWidth;

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Finds an instance of the Grid in the scene
        /// </summary>
        /// <param name="grid">out value</param>
        /// <returns>if the grid was found</returns>
        private static bool TryGetGrid(out Grid grid)
        {
            grid = FindObjectOfType<Grid>();
            return grid;
        }

        /// <summary>
        /// Calculates the dimensions for a visual representation of the grid
        /// </summary>
        /// <param name="cellSize">Size for every square that conforms the grid</param>
        /// <param name="dimensions">The dimensions of the grid</param>
        /// <returns></returns>
        private static Vector2 GetGridLayoutDimensions(int cellSize, Vector2Int dimensions)
        {
            var gridDimensions = new Vector2
            {
                x = dimensions.x * cellSize,
                y = dimensions.y * cellSize,
            };
            return gridDimensions;
        }
    }
}
