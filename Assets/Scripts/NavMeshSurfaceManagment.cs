using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshSurfaceManagement : MonoBehaviour
{
    public static NavMeshSurfaceManagement Instance {  get; private set; }

    private NavMeshSurface navMeshSurface;

    private void Update()
    {
        Instance = this;
        navMeshSurface = GetComponent<NavMeshSurface>();
        navMeshSurface.hideEditorLogs = true;
    }

    public void RebakeNavMeshSurface()
    {
        navMeshSurface.BuildNavMesh();
    }
}
