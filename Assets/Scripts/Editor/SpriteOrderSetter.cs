#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class SpriteOrderSetter
{
    const float Z_TOLERANCE = 0.01f;  // Used to order sprites by Z position within a group

    [MenuItem("GameObject/Set Sprite Order By Z Position", false, 49)]
    private static void SetSpriteOrderByZPosition()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        // Retrieve SpriteRenderers and filter out null ones
        SpriteRenderer[] spriteRenderers = selectedObjects.Select(obj => obj.GetComponent<SpriteRenderer>()).Where(sr => sr != null).ToArray();

        // Group sprites based on overlap
        List<List<SpriteRenderer>> groups = new List<List<SpriteRenderer>>();
        foreach (var spriteRenderer in spriteRenderers)
        {
            bool placedInGroup = false;
            Bounds spriteBounds = spriteRenderer.bounds;

            foreach (var group in groups)
            {
                if (group.Any(other => other.bounds.Intersects(spriteBounds)))
                {
                    group.Add(spriteRenderer);
                    placedInGroup = true;
                    break;
                }
            }

            if (!placedInGroup)
            {
                groups.Add(new List<SpriteRenderer> { spriteRenderer });
            }
        }

        // Sort the sprites within each group by Z position
        foreach (var group in groups)
        {
            group.Sort((sr1, sr2) => sr1.transform.position.z.CompareTo(sr2.transform.position.z));

            // Set Order in Layer based on sorted order
            for (int i = 0; i < group.Count; i++)
            {
                group[i].sortingOrder = i;
            }
        }
    }

    [MenuItem("GameObject/Set Sprite Order By Z Position", true)]
    private static bool ValidateSetSpriteOrderByZPosition()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        // Check if any selected object doesn't have a SpriteRenderer component
        return selectedObjects.All(obj => obj.GetComponent<SpriteRenderer>() != null);
    }


    [MenuItem("GameObject/Reset Sprite Order", false, 50)]
    private static void ResetSpriteOrder()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        // Retrieve SpriteRenderers and filter out null ones
        SpriteRenderer[] spriteRenderers = selectedObjects.Select(obj => obj.GetComponent<SpriteRenderer>()).Where(sr => sr != null).ToArray();

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sortingOrder = 0;
        }
    }

    [MenuItem("GameObject/Select Same Order In Layer", false, 51)]
    private static void SelectSameOrderInLayer()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject == null)
            return;

        SpriteRenderer spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            return;

        int orderInLayer = spriteRenderer.sortingOrder;

        SpriteRenderer[] allSpriteRenderers = GameObject.FindObjectsOfType<SpriteRenderer>();
        GameObject[] objectsWithSameOrder = allSpriteRenderers.Where(sr => sr.sortingOrder == orderInLayer).Select(sr => sr.gameObject).ToArray();

        Selection.objects = objectsWithSameOrder;
    }
}
#endif
