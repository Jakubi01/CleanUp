using UnityEditor;
using UnityEngine;

namespace EditorUtilities
{
    public abstract class BatchApplyPrefabs
    {
        [MenuItem("Tools/Prefabs/Apply Selected Prefab Overrides")]
        public static void ApplySelectedPrefabs()
        {
            GameObject[] selected = Selection.gameObjects;
            int count = 0;

            foreach (GameObject go in selected)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(go))
                {
                    GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(go);
                    if (PrefabUtility.HasPrefabInstanceAnyOverrides(root, false))
                    {
                        PrefabUtility.ApplyPrefabInstance(root, InteractionMode.UserAction);
                        count++;
                    }
                }
            }
            Debug.Log($"선택된 프리팹 중 {count}개의 오버라이드 적용 완료!");
        }

        [MenuItem("Tools/Prefabs/Apply ALL Prefab Overrides in Scene")]
        public static void ApplyAllPrefabsInScene()
        {
            GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int count = 0;

            foreach (GameObject go in allObjects)
            {
                // 프리팹의 최상위 루트 오브젝트인지 확인
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(go))
                {
                    if (PrefabUtility.HasPrefabInstanceAnyOverrides(go, false))
                    {
                        PrefabUtility.ApplyPrefabInstance(go, InteractionMode.UserAction);
                        count++;
                    }
                }
            }
            Debug.Log($"씬 전체에서 수정된 프리팹 {count}개 일괄 적용 완료!");
        }
    }
}