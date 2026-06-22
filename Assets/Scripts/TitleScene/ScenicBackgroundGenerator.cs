using UnityEngine;

public class ScenicBackgroundGenerator : MonoBehaviour
{
    [SerializeField] private int seed = 7305;
    [SerializeField] private int width = 28;
    [SerializeField] private int height = 18;
    [SerializeField] private float tileSize = 1f;
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Sprite[] grassTiles;
    [SerializeField] private Sprite[] flowerTiles;
    [SerializeField] private Sprite[] roadTiles;
    [SerializeField] private Sprite[] stoneTiles;
    [SerializeField] private int roadTileIndex = 0;
    [SerializeField] private Sprite[] propSprites;

    private const string GeneratedRootName = "__GeneratedScenicBackground";
    private Transform generatedRoot;

    private void Awake()
    {
        if (transform.Find(GeneratedRootName) == null)
        {
            Generate();
        }
    }

    private void OnValidate()
    {
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
        tileSize = Mathf.Max(0.1f, tileSize);
        roadTileIndex = Mathf.Max(0, roadTileIndex);
    }

    [ContextMenu("Generate Scenic Background")]
    private void Generate()
    {
        if (grassTiles == null || grassTiles.Length == 0)
        {
            return;
        }

        Sprite baseGroundTile = FirstValid(grassTiles);
        if (baseGroundTile == null)
        {
            return;
        }

        ClearGeneratedRoot();

        SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>();
        if (parentRenderer != null)
        {
            parentRenderer.enabled = false;
        }

        generatedRoot = new GameObject(GeneratedRootName).transform;
        generatedRoot.SetParent(transform, false);
        generatedRoot.localPosition = Vector3.zero;

        int minX = -width / 2;
        int minY = -height / 2;
        System.Random random = new System.Random(seed);
        Sprite roadTile = PickAtOrFirstValid(roadTiles, roadTileIndex) ?? baseGroundTile;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int mapX = minX + x;
                int mapY = minY + y;
                Vector3 position = new Vector3(mapX * tileSize, mapY * tileSize, 0f);
                Sprite tile = Pick(grassTiles, random) ?? baseGroundTile;

                bool isHorizontalPath = mapY >= 0 && mapY <= 1 && mapX > minX + 2 && mapX < minX + width - 3;
                bool isVerticalPath = mapX >= 0 && mapX <= 1 && mapY > minY + 2 && mapY < minY + height - 3;
                bool isStoneArea = isHorizontalPath || isVerticalPath;
                bool isWaterArea = IsWaterArea(mapX, mapY);

                if (isStoneArea && roadTile != null)
                {
                    tile = roadTile;
                }

                CreateSprite(generatedRoot, "Ground", baseGroundTile, position, -21, Color.white, true);

                if (tile != baseGroundTile)
                {
                    CreateSprite(generatedRoot, "GroundDetail", tile, position, -20, Color.white, true);
                }

                if (isWaterArea && stoneTiles != null && stoneTiles.Length > 0)
                {
                    CreateSprite(generatedRoot, "Water", Pick(stoneTiles, random), position, -17, new Color(0.26f, 0.62f, 0.9f, 0.9f), true);
                }

                bool keepCenterReadable = Mathf.Abs(mapX) <= 6 && Mathf.Abs(mapY) <= 4;
                if (!isStoneArea && !isWaterArea && !keepCenterReadable && flowerTiles != null && flowerTiles.Length > 0 && random.NextDouble() < 0.12)
                {
                    CreateSprite(generatedRoot, "Flower", Pick(flowerTiles, random), position, -18, Color.white, true);
                }
            }
        }

        if (propSprites != null && propSprites.Length > 0)
        {
            AddBorderProps(generatedRoot, random, minX, minY);
            AddPropCluster(generatedRoot, random, minX + 4, minY + height - 4, 7);
            AddPropCluster(generatedRoot, random, minX + width - 5, minY + 4, 7);
            AddPropCluster(generatedRoot, random, minX + 5, minY + 4, 6);
            AddPropCluster(generatedRoot, random, minX + width - 6, minY + height - 5, 6);
        }
    }

    private void AddBorderProps(Transform root, System.Random random, int minX, int minY)
    {
        for (int i = 0; i < 36; i++)
        {
            bool verticalEdge = random.NextDouble() < 0.55;
            float x = verticalEdge
                ? (random.NextDouble() < 0.5 ? minX + 1 : minX + width - 2)
                : minX + 1 + (float)random.NextDouble() * (width - 2);
            float y = verticalEdge
                ? minY + 1 + (float)random.NextDouble() * (height - 2)
                : (random.NextDouble() < 0.5 ? minY + 1 : minY + height - 2);

            if (Mathf.Abs(x) <= 6 && Mathf.Abs(y) <= 4)
            {
                continue;
            }

            Vector3 position = new Vector3(Mathf.Round(x) * tileSize, Mathf.Round(y) * tileSize, 0f);
            CreateSprite(root, "BorderProp", Pick(propSprites, random), position, -16, Color.white, false);
        }
    }

    private bool IsWaterArea(int x, int y)
    {
        bool lowerLeftPond = x >= -12 && x <= -9 && y >= -7 && y <= -5;
        bool upperRightPond = x >= 9 && x <= 12 && y >= 5 && y <= 7;
        bool roundedLowerLeftEdges = (x == -12 || x == -9) && (y == -7 || y == -5);
        bool roundedUpperRightEdges = (x == 9 || x == 12) && (y == 5 || y == 7);

        return (lowerLeftPond && !roundedLowerLeftEdges) || (upperRightPond && !roundedUpperRightEdges);
    }

    private void AddPropCluster(Transform root, System.Random random, int originX, int originY, int count)
    {
        count = random.Next(Mathf.Max(2, count - 2), count + 2);

        for (int i = 0; i < count; i++)
        {
            float offsetX = (float)(random.NextDouble() * 4.0 - 2.0);
            float offsetY = (float)(random.NextDouble() * 3.0 - 1.5);
            Vector3 position = new Vector3(Mathf.Round(originX + offsetX) * tileSize, Mathf.Round(originY + offsetY) * tileSize, 0f);
            CreateSprite(root, "Prop", Pick(propSprites, random), position, -16, Color.white, false);
        }
    }

    private Sprite Pick(Sprite[] sprites, System.Random random)
    {
        if (sprites == null || sprites.Length == 0)
        {
            return null;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            Sprite sprite = sprites[random.Next(0, sprites.Length)];
            if (sprite != null)
            {
                return sprite;
            }
        }

        return null;
    }

    private Sprite FirstValid(Sprite[] sprites)
    {
        if (sprites == null)
        {
            return null;
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
            {
                return sprites[i];
            }
        }

        return null;
    }

    private Sprite PickAtOrFirstValid(Sprite[] sprites, int index)
    {
        if (sprites == null || sprites.Length == 0)
        {
            return null;
        }

        if (index >= 0 && index < sprites.Length && sprites[index] != null)
        {
            return sprites[index];
        }

        return FirstValid(sprites);
    }

    private void CreateSprite(Transform parent, string prefix, Sprite sprite, Vector3 localPosition, int sortingOrder, Color color, bool fitToTile)
    {
        if (sprite == null)
        {
            return;
        }

        GameObject tile = new GameObject(prefix);
        tile.transform.SetParent(parent, false);
        tile.transform.localPosition = new Vector3(
            Mathf.Round(localPosition.x / tileSize) * tileSize,
            Mathf.Round(localPosition.y / tileSize) * tileSize,
            localPosition.z
        );

        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        renderer.sortingOrder = sortingOrder;

        if (fitToTile && sprite.bounds.size.x > 0f && sprite.bounds.size.y > 0f)
        {
            tile.transform.localScale = new Vector3(
                tileSize / sprite.bounds.size.x,
                tileSize / sprite.bounds.size.y,
                1f
            );
        }

        if (tileMaterial != null)
        {
            renderer.sharedMaterial = tileMaterial;
        }
    }

    private void ClearGeneratedRoot()
    {
        Transform existing = transform.Find(GeneratedRootName);
        if (existing == null)
        {
            generatedRoot = null;
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(existing.gameObject);
        }
        else
        {
            DestroyImmediate(existing.gameObject);
        }

        generatedRoot = null;
    }
}
