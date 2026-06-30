using UnityEditor;
using UnityEngine;

public static class BackgroundTreePlacementTool
{
    private const string BackgroundPrefabPath = "Assets/Prefabs/Backgrounds/CommonGameBackground.prefab";
    private const string TreeParentName = "Generated Trees";

    private static readonly string[] TreePrefabPaths =
    {
        "Assets/DownloadAssets/Pixel Art Top Down - Basic/Prefab/Plant/PF Plant - Tree 01.prefab",
        "Assets/DownloadAssets/Pixel Art Top Down - Basic/Prefab/Plant/PF Plant - Tree 02.prefab",
        "Assets/DownloadAssets/Pixel Art Top Down - Basic/Prefab/Plant/PF Plant - Tree 03.prefab"
    };

    private static readonly TreePlacement[] Placements =
    {
        new(-23.5f, -11.5f, 0, 0.95f, false),
        new(-15.0f, -14.0f, 1, 0.9f, true),
        new(-6.5f, -10.0f, 2, 0.85f, false),
        new(6.0f, -13.0f, 0, 0.9f, true),
        new(17.5f, -9.5f, 1, 0.95f, false),
        new(24.0f, -14.0f, 2, 0.85f, true),
        new(-26.0f, -2.0f, 1, 0.9f, false),
        new(-12.5f, -3.5f, 2, 0.85f, true),
        new(12.0f, -2.5f, 0, 0.9f, false),
        new(25.0f, 2.0f, 1, 0.95f, true),
        new(-20.0f, 8.5f, 2, 0.85f, false),
        new(-3.5f, 6.5f, 0, 0.9f, true),
        new(9.5f, 9.0f, 1, 0.9f, false),
        new(21.0f, 7.0f, 2, 0.85f, true),
        new(-27.0f, 15.0f, 0, 0.9f, false),
        new(-10.0f, 14.0f, 1, 0.95f, true),
        new(4.0f, 15.0f, 2, 0.85f, false),
        new(18.0f, 14.5f, 0, 0.9f, true),
    };

    [InitializeOnLoadMethod]
    private static void PlaceTreesAfterScriptReload()
    {
        EditorApplication.delayCall += PlaceTreesIfMissing;
    }

    [MenuItem("Tools/Background/Place Trees In Common Background")]
    public static void PlaceTreesInCommonBackground()
    {
        GameObject root = PrefabUtility.LoadPrefabContents(BackgroundPrefabPath);
        if (root == null)
        {
            Debug.LogError($"Failed to load prefab: {BackgroundPrefabPath}");
            return;
        }

        try
        {
            Transform existingParent = root.transform.Find(TreeParentName);
            if (existingParent != null)
            {
                Object.DestroyImmediate(existingParent.gameObject);
            }

            GameObject treeParent = new GameObject(TreeParentName);
            treeParent.transform.SetParent(root.transform, false);

            GameObject[] treePrefabs = new GameObject[TreePrefabPaths.Length];
            for (int i = 0; i < TreePrefabPaths.Length; i++)
            {
                treePrefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(TreePrefabPaths[i]);
                if (treePrefabs[i] == null)
                {
                    Debug.LogError($"Missing tree prefab: {TreePrefabPaths[i]}");
                    return;
                }
            }

            for (int i = 0; i < Placements.Length; i++)
            {
                TreePlacement placement = Placements[i];
                GameObject tree = (GameObject)PrefabUtility.InstantiatePrefab(treePrefabs[placement.PrefabIndex], treeParent.transform);
                tree.name = $"Tree {i + 1:00}";
                tree.transform.localPosition = new Vector3(placement.X, placement.Y, 0f);
                tree.transform.localScale = new Vector3(placement.FlipX ? -placement.Scale : placement.Scale, placement.Scale, 1f);
            }

            PrefabUtility.SaveAsPrefabAsset(root, BackgroundPrefabPath);
            Debug.Log($"Placed {Placements.Length} trees in {BackgroundPrefabPath}");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }

    private static void PlaceTreesIfMissing()
    {
        GameObject root = PrefabUtility.LoadPrefabContents(BackgroundPrefabPath);
        if (root == null)
        {
            return;
        }

        try
        {
            Transform existingParent = root.transform.Find(TreeParentName);
            if (existingParent != null && existingParent.childCount == Placements.Length)
            {
                return;
            }
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }

        PlaceTreesInCommonBackground();
    }

    private readonly struct TreePlacement
    {
        public readonly float X;
        public readonly float Y;
        public readonly int PrefabIndex;
        public readonly float Scale;
        public readonly bool FlipX;

        public TreePlacement(float x, float y, int prefabIndex, float scale, bool flipX)
        {
            X = x;
            Y = y;
            PrefabIndex = prefabIndex;
            Scale = scale;
            FlipX = flipX;
        }
    }
}
