#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class SpriteZPositioningEditor
{
    const float MIN_Z_DIFFERENCE = 0.001f;

    [MenuItem("GameObject/Adjust Z Positions", false, 49)]  // Added to GameObject menu
    private static void AdjustZPositions()
    {
        // Store the selected objects
        GameObject[] selectedObjects = Selection.gameObjects;

        // Sort by original z position
        selectedObjects = selectedObjects.OrderBy(go => go.transform.position.z).ToArray();

        for (int i = 1; i < selectedObjects.Length; i++)
        {
            Transform previousTransform = selectedObjects[i - 1].transform;
            Transform currentTransform = selectedObjects[i].transform;

            if (Mathf.Abs(currentTransform.position.z - previousTransform.position.z) < MIN_Z_DIFFERENCE)
            {
                Vector3 newPosition = currentTransform.position;
                newPosition.z = previousTransform.position.z + MIN_Z_DIFFERENCE;
                currentTransform.position = newPosition;
            }
        }
    }
}
#endif