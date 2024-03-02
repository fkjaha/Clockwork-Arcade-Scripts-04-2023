using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

#pragma warning disable
public enum FuncMode
{
    Renaming,
    Align,
    ReplaceClonesWithPrefabs,
    SelectRandomFromSelected,
    GridSpawn
}
public class FK_script : EditorWindow
{

    private FuncMode _mode;

    private GUIStyle _poputStyle = new GUIStyle();
    
    
    //Renaming
    private string _nameToAppendIndex = "";
    private int _startIndex;

    private SerializedObject a;
    
    //Align
    private bool _alignX;
    private bool _alignY;
    private bool _alignZ;

    private Transform _alignPointA;
    private Transform _alignPointB;

    private Vector3 _alignOffset;
    
    
    //Replace clones
    private bool _findClonesFromSelected = true;
    private bool _ignorePrefabName = false;
    private string _clonePostfix = "blablabla(RandomStuff)";
    private GameObject _clonesPrefab;

    //Random selection
    // private string _selectionName = "";
    private int _selectionChance = 50;
    
    //GridSpawn
    private Vector3Int _gridSize;
    private Vector3 _gridSpacing;
    private GameObject _gridPrefab;
    private Transform _gridOriginAndParent;
    private List<GameObject> _gridObjects;
    private bool _gridSpawnAsPrefab = true;


    [MenuItem("Window/FK-HELPER")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FK_script), false, "FK-HELPER");
    }
    
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(5);

        if (GUILayout.Button("EXECUTE"))
        {
            ActivateScript();
        }
        _mode = (FuncMode)EditorGUILayout.EnumPopup("Script mode: ", _mode);
        
        GUILayout.Space(12);
        GUILayout.Box("Selected mode variables: ");
        GUILayout.Space(5);
        
        switch (_mode)
        {
            case FuncMode.Renaming:
                try
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Start index:");
                    _startIndex = Int32.Parse(GUILayout.TextField("" + _startIndex, 6));
                    GUILayout.EndHorizontal();
                }
                catch (Exception)
                {
                    Debug.Log("Wrong <start index>");
                }
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name to append index:");
                _nameToAppendIndex = GUILayout.TextField(_nameToAppendIndex, 50);
                GUILayout.EndHorizontal();
                
                break;
            case FuncMode.Align:
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Axis:");
                _alignX = GUILayout.Toggle(_alignX, "X");
                _alignY = GUILayout.Toggle(_alignY, "Y");
                _alignZ = GUILayout.Toggle(_alignZ, "Z");
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Point A:");
                _alignPointA = EditorGUILayout.ObjectField(_alignPointA, typeof(Transform)) as Transform;
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Point B:");
                _alignPointB = EditorGUILayout.ObjectField(_alignPointB, typeof(Transform)) as Transform;
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Align Offset:");
                _alignOffset = EditorGUILayout.Vector3Field("", _alignOffset);
                GUILayout.EndHorizontal();

                break;
            case FuncMode.ReplaceClonesWithPrefabs:
                
                _findClonesFromSelected = GUILayout.Toggle(_findClonesFromSelected, "Find Clones from selected objects");
                _ignorePrefabName = GUILayout.Toggle(_ignorePrefabName, "Ignore Prefab Name");
                if(!_findClonesFromSelected)
                    GUILayout.Box("If 'Find Clones from selected objects' is disabled execution might crush Unity in case you have too many objects on scene!");

                GUILayout.BeginHorizontal();
                GUILayout.Label("Clones name postfix:");
                _clonePostfix = GUILayout.TextField(_clonePostfix, 25);
                GUILayout.EndHorizontal();

                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Clones prefab:");
                _clonesPrefab = EditorGUILayout.ObjectField(_clonesPrefab, typeof(GameObject)) as GameObject;
                GUILayout.EndHorizontal();

                
                break;
            case FuncMode.SelectRandomFromSelected:
                try
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Selection chance:");
                    _selectionChance = Int32.Parse(GUILayout.TextField("" + _selectionChance, 3));
                    GUILayout.EndHorizontal();
                }
                catch (Exception e)
                {
                    Debug.Log("Wrong <start index>");
                }
                
                // GUILayout.BeginHorizontal();
                // GUILayout.Label("SelectionName:");
                // _selectionName = GUILayout.TextField(_selectionName, 50);
                // GUILayout.EndHorizontal();
                
                break;
            case FuncMode.GridSpawn:
                GUILayout.BeginHorizontal();
                GUILayout.Label("Spawn Prefab: ");
                _gridPrefab = EditorGUILayout.ObjectField(_gridPrefab, typeof(GameObject)) as GameObject;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Spawn Origin & Parent:");
                _gridOriginAndParent = EditorGUILayout.ObjectField(_gridOriginAndParent, typeof(Transform)) as Transform;
                GUILayout.EndHorizontal();
                _gridSpawnAsPrefab = GUILayout.Toggle(_gridSpawnAsPrefab, "Grid spawn as prefab ");
                _gridSize = EditorGUILayout.Vector3IntField("Grid size: ", _gridSize);
                _gridSpacing = EditorGUILayout.Vector3Field("Grid spacing: ", _gridSpacing);
                if (GUILayout.Button("DeleteSpawned"))
                {
                    DeleteGrid();
                }
                break;
        }
        
        
        
        GUILayout.EndVertical();
    }

    private void ActivateScript()
    {
        switch (_mode)
        {
            case FuncMode.Renaming:
                RenameSelected();
                break;
            case FuncMode.Align:
                AlignSelected();
                break;
            case FuncMode.ReplaceClonesWithPrefabs:
                ReplaceClonesWithPrefabs();
                break;
            case FuncMode.SelectRandomFromSelected:
                SelectRandomFromSelected();
                break;
            case FuncMode.GridSpawn:
                GridSpawn();
                break;
        }
    }

    private void RenameSelected()
    {
        List<GameObject> sortedList = Selection.transforms.OrderBy(a => a.transform.GetSiblingIndex()).Select(a=>a.gameObject).ToList();
        for (int i = 0; i < sortedList.Count; i++)
        {
            sortedList[i].name = _nameToAppendIndex + (_startIndex + i);
        }
        Debug.Log("Renamed!");
    }
    
    private void AlignSelected()
    {
        if(!_alignX && !_alignY && !_alignZ) return;
        
        List<Transform> transforms = Selection.transforms.Where(t => t != _alignPointA && t != _alignPointB).ToList();
        int transformsCount = transforms.Count;
        Vector3 vector3Step = (_alignPointB.position - _alignPointA.position)/(transformsCount+1);
        
        Debug.Log(transformsCount);

        for (int i = 0; i < transformsCount; i++)
        {
            transforms[i].position = new Vector3(_alignX ? _alignPointA.position.x + vector3Step.x * (i + 1) : transforms[i].position.x,
                _alignY ? _alignPointA.position.y + vector3Step.y * (i + 1) : transforms[i].position.y,
                _alignZ ? _alignPointA.position.z + vector3Step.z * (i + 1) : transforms[i].position.z);
        }

        Debug.Log("Aligned!");
    }



    private void ReplaceClonesWithPrefabs()
    {
        List<Transform> objects =
            _findClonesFromSelected ? Selection.transforms.ToList() : FindObjectsOfType<Transform>().ToList();
        List<GameObject> clones;
        if(!_ignorePrefabName)
        {
            clones = objects
            .Where(x => x.name.StartsWith(_clonesPrefab.name+_clonePostfix)).Select(x=>x.gameObject).ToList();
        }
        else
        {
            clones = objects
                .Where(x => x.name.Contains(_clonePostfix)).Select(x=>x.gameObject).ToList();
        }
        foreach (GameObject spawnedClone in clones)
        {
            GameObject newObject = PrefabUtility.InstantiatePrefab(_clonesPrefab) as GameObject;
            
            newObject.transform.position = spawnedClone.transform.position;
            newObject.transform.parent = spawnedClone.transform.parent;

            DestroyImmediate(spawnedClone);
        }
        
        Debug.Log("Replaced!");
    }

    private void SelectRandomFromSelected()
    {
        List<GameObject> selected = Selection.transforms.Select(x=>x.gameObject).ToList();
        List<GameObject> chosen = selected.Where(_ => Random.Range(0, 101) <= _selectionChance).ToList();
        Selection.objects = chosen.ToArray();
    }

    private void GridSpawn()
    {
        if(_gridPrefab == null || _gridOriginAndParent == null) return;
        
        _gridObjects = new();
        Vector3 spawnPosition = Vector3.zero;
        for (int i = 0; i < _gridSize.x; i++)
        {
            for (int j = 0; j < _gridSize.y; j++)
            {
                for (int k = 0; k < _gridSize.z; k++)
                {
                    spawnPosition = new Vector3(i * _gridSpacing.x, j * _gridSpacing.y, k * _gridSpacing.z) + _gridOriginAndParent.position;
                    
                    if (_gridSpawnAsPrefab)
                    {
                        GameObject spawned = PrefabUtility.InstantiatePrefab(_gridPrefab) as GameObject;
                        spawned.transform.position = spawnPosition;
                        spawned.transform.parent = _gridOriginAndParent;
                        _gridObjects.Add(spawned);
                    }
                    else
                    {
                        GameObject spawned = Instantiate(_gridPrefab, spawnPosition, Quaternion.identity,
                            _gridOriginAndParent);
                        _gridObjects.Add(spawned);
                    }
                }
            }
        }
        // Selection.objects = _gridObjects.ToArray();
    }

    private void DeleteGrid()
    {
        if (_gridObjects != null)
        {
            foreach (GameObject gridObject in _gridObjects)
            {
                DestroyImmediate(gridObject);
            }
        }
    }
}
#pragma warning restore