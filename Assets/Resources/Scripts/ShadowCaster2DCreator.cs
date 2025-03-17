using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCaster2DCreator : MonoBehaviour
{
	[SerializeField]
	private bool enableSelfShadows = true;

	private CompositeCollider2D tilemapCollider;

	// static readonly FieldInfo meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);
	static readonly FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
	static readonly FieldInfo shapePathHashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
	// static readonly MethodInfo generateShadowMeshMethod = typeof(ShadowCaster2D)
    //     .Assembly
    //     .GetType("UnityEngine.Rendering.Universal.ShadowUtility")
    //     .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);

	public void Create()
	{
		DestroyOldShadowCasters();
		tilemapCollider = GetComponent<CompositeCollider2D>();

        // Debug.Log($"Path Count: {tilemapCollider.pathCount}");

		for (int i = 0; i < tilemapCollider.pathCount; i++)
		{
			var pathVertices = new Vector2[tilemapCollider.GetPathPointCount(i)];
			tilemapCollider.GetPath(i, pathVertices);
			var shadowCaster = new GameObject("ShadowCaster_" + i);
			shadowCaster.transform.parent = gameObject.transform;
			var shadowCasterComponent = shadowCaster.AddComponent<ShadowCaster2D>();
			shadowCasterComponent.selfShadows = enableSelfShadows;

			var testPath = new Vector3[pathVertices.Length];

			for (int j = 0; j < pathVertices.Length; j++)
			{
				testPath[j] = pathVertices[j];
			}

			shapePathField.SetValue(shadowCasterComponent, testPath);
			shapePathHashField.SetValue(shadowCasterComponent, Random.Range(int.MinValue, int.MaxValue));
			// meshField.SetValue(shadowCasterComponent, new Mesh());
			// generateShadowMeshMethod.Invoke(shadowCasterComponent, new object[] { meshField.GetValue(shadowCasterComponent), shapePathField.GetValue(shadowCasterComponent) });
		}
	}

	public void DestroyOldShadowCasters()
	{
		var tempList = transform.Cast<GameObject>().ToList();

		foreach (var child in tempList)
		{
			DestroyImmediate(child);
		}
	}
}
