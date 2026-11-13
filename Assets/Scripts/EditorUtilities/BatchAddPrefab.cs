using UnityEditor;
using UnityEngine;

namespace EditorUtilities
{
    public class BatchAddPrefab : EditorWindow
    {
        private GameObject _childPrefab;

        [MenuItem("Tools/Batch Add Child Prefab")]
        public static void ShowWindow()
        {
            GetWindow<BatchAddPrefab>("프리팹 일괄 추가");
        }

        private void OnGUI()
        {
            GUILayout.Label("선택한 프리팹들에 자식 프리팹 일괄 추가", EditorStyles.boldLabel);
            _childPrefab = (GameObject)EditorGUILayout.ObjectField("추가할 자식 프리팹 (A)", _childPrefab, typeof(GameObject), false);

            if (GUILayout.Button("선택된 프리팹들에 모두 추가하기"))
            {
                if (_childPrefab is null)
                {
                    Debug.LogWarning("추가할 자식 프리팹을 먼저 지정해주세요.");
                    return;
                }
                AddChildToSelectedPrefabs();
            }
        }

        private void AddChildToSelectedPrefabs()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            int count = 0;

            foreach (GameObject obj in selectedObjects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                // 프리팹 에셋인지 확인
                if (string.IsNullOrEmpty(assetPath) || PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.NotAPrefab)
                    continue;

                // 프리팹 에셋을 메모리로 로드하여 수정
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
            
                // A 프리팹을 자식으로 인스턴스화 (중첩 프리팹 구조 유지)
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(_childPrefab, prefabRoot.transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;

                // 변경사항 저장 및 메모리 해제
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);
                PrefabUtility.UnloadPrefabContents(prefabRoot);
                count++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"총 {count}개의 프리팹에 '{_childPrefab.name}' 추가 완료!");
        }
    }
}