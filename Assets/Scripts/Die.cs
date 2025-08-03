using System.Collections.Generic;
using UnityEngine;
using Modifiers;
using System.Linq;

public class Die : MonoBehaviour
{
	public enum DieType
	{
		D6, D8
	}

	[SerializeField] private DieInfo dieInfo;
	[SerializeField] private List<DieFace> faces;
	[SerializeField] private Transform meshTransform;

	private Rigidbody rb;
	private AnimationRecorder recorder;


	public DieInfo DieInfo => dieInfo;
	public List<DieFace> Faces => faces;
	public int Sides => faces.Count;
	public Rigidbody Rigidbody => rb;
	public AnimationRecorder Recorder => recorder;

	// For debugging
	//float startTime;
	//[SerializeField] bool doRolls = true;

	private void Start()
	{
		SetData(new DieInfo(DieType.D6, new int[]{ 1, 2, 3, 4, 5, 6 }));
	}

	public void SetData(DieInfo d)
	{
		dieInfo = d;
		for (int i = 0; i < faces.Count; i++)
		{
			faces[i].SetValue(dieInfo.FaceValues[i]);
		}
	}

	public void SetFaces(params int[] faceValues)
	{
		if (faceValues.Length != faces.Count)
		{
			Debug.LogError("Mismatched faces and faceValues");
			return;
		}

		for (int i = 0; i < faces.Count; i++)
		{
			faces[i].SetValue(faceValues[i]);
		}
	}

	/// <summary>
	/// Rotates the mesh of a die. Doesn't rotate the actual object
	/// </summary>
	/// <param name="fromIndex">The index of the face currently facing up</param>
	/// <param name="toIndex">The index of the face that should face up</param>
	/// <exception cref="System.ArgumentException"></exception>
	public void TransformToTarget(int fromIndex, int toIndex)
	{
		//Debug.Log($"Applying rotation {dieRots[fromIndex, toIndex]}");
		//Vector3 localAngles = meshTransform.localEulerAngles;
		meshTransform.Rotate((Sides switch
		{
			//4 => Transformations.d4, // 4 sided dice are currently broken.
			6 => Transformations.d6,
			8 => Transformations.d8,
			_ => throw new System.ArgumentException("Improper number of sides on die")
		})[fromIndex, toIndex], Space.Self);
		//Debug.Log($"Rot was {localAngles}, now {meshTransform.localEulerAngles}");
	}

	public int Roll()
	{
		int faceIndex = Random.Range(0, faces.Count);
		return MaterialModifier.Process(this, faces[faceIndex].Value);
	}

	[ContextMenu("Roll")]
	public void ApplyPhysics()
	{
		//startTime = Time.time;
		Vector3 force = new(Random.Range(-5, 5), Random.Range(2, 7), Random.Range(-5, 5));
		Vector3 torque = new(Random.Range(-25, 25), Random.Range(-25, 25), Random.Range(-25, 25));

		rb.linearVelocity = force;
		rb.AddTorque(torque, ForceMode.VelocityChange);
	}

	public DieFace GetTopFace() => faces.OrderByDescending(f => f.YPos).FirstOrDefault();

	public bool IsStopped() => rb.angularVelocity == Vector3.zero && rb.linearVelocity == Vector3.zero;

	public int[] GetFaceValues() => faces.Select(f => f.Value).ToArray();
}

public readonly struct Transformations
{
	//public static readonly Vector3[,] d4 =
	//{
	//	{ new(000.00f, 000.00f, 000.00f), new(331.87f, 019.11f, 112.21f), new(070.53f, 240.00f, 180.00f), new(331.87f, 220.89f, 247.79f), },
	//	{ new(028.13f, 340.89f, 247.79f), new(000.00f, 000.00f, 000.00f), new(098.66f, 220.89f, 067.79f), new(000.00f, 201.79f, 135.59f), },
	//	{ new(289.47f, 120.00f, 180.00f), new(261.34f, 139.11f, 292.21f), new(000.00f, 000.00f, 000.00f), new(261.34f, 100.90f, 067.79f), },
	//	{ new(028.13f, 019.10f, 112.21f), new(000.00f, 038.21f, 224.42f), new(098.66f, 259.10f, 292.21f), new(000.00f, 000.00f, 000.00f), },
	//};
	public static readonly Vector3[,] d6 =
	{
		{ new(000, 000, 000), new(000, 000, 270), new(090, 000, 000), new(270, 000, 000), new(000, 000, 090), new(000, 000, 180), },
		{ new(000, 000, 090), new(000, 000, 000), new(000, 090, 090), new(000, 270, 090), new(000, 000, 180), new(000, 000, 270), },
		{ new(270, 000, 000), new(270, 000, 270), new(000, 000, 000), new(180, 000, 000), new(270, 000, 090), new(270, 000, 180), },
		{ new(090, 360, 000), new(090, 360, 270), new(180, 360, 000), new(000, 000, 000), new(090, 360, 090), new(090, 360, 180), },
		{ new(000, 000, 270), new(000, 000, 180), new(000, 270, 270), new(000, 090, 270), new(000, 000, 000), new(000, 000, 090), },
		{ new(000, 000, 180), new(000, 000, 090), new(270, 000, 180), new(090, 000, 180), new(000, 000, 270), new(000, 000, 000), },
	};
	public static readonly Vector3[,] d8 =
	{
		{ new(000, 000, 000), new(000, 180, 180), new(000, 180, 000), new(000, 000, 180), new(000, 090, 000), new(000, 270, 180), new(000, 270, 000), new(000, 090, 180), },
		{ new(000, 180, 180), new(000, 000, 000), new(000, 000, 180), new(000, 180, 000), new(000, 090, 180), new(000, 270, 000), new(000, 270, 180), new(000, 090, 000), },
		{ new(000, 180, 000), new(000, 000, 180), new(000, 000, 000), new(000, 180, 180), new(000, 270, 000), new(000, 090, 180), new(000, 090, 000), new(000, 270, 180), },
		{ new(000, 000, 180), new(000, 180, 000), new(000, 180, 180), new(000, 000, 000), new(000, 270, 180), new(000, 090, 000), new(000, 090, 180), new(000, 270, 000), },
		{ new(000, 270, 000), new(000, 090, 180), new(000, 090, 000), new(000, 270, 180), new(000, 000, 000), new(000, 180, 180), new(000, 180, 000), new(000, 000, 180), },
		{ new(000, 270, 180), new(000, 090, 000), new(000, 090, 180), new(000, 270, 000), new(000, 180, 180), new(000, 000, 000), new(000, 000, 180), new(000, 180, 000), },
		{ new(000, 090, 000), new(000, 270, 180), new(000, 270, 000), new(000, 090, 180), new(000, 180, 000), new(000, 000, 180), new(000, 000, 000), new(000, 180, 180), },
		{ new(000, 090, 180), new(000, 270, 000), new(000, 270, 180), new(000, 090, 000), new(000, 000, 180), new(000, 180, 000), new(000, 180, 180), new(000, 000, 000), },
	};
}

public struct DieInfo
{
	Die.DieType type;
	[SerializeField] private int[] faceValues;
	[SerializeField] private MaterialModifiers material;
	[SerializeField] private ColorModifiers color;

	public readonly Die.DieType Type => type;
	public readonly MaterialModifiers Material => material;
	public readonly ColorModifiers Color => color;
	public readonly int[] FaceValues => faceValues;

	public void ApplyMaterial(MaterialModifiers mat) => material = mat;
	public void ApplyColor(ColorModifiers col) => color = col;
	public void SetFaceValues(params int[] values) => faceValues = values;

	public DieInfo(Die.DieType dieType, int[] values, MaterialModifiers mat = MaterialModifiers.None, ColorModifiers col = ColorModifiers.None)
	{
		type = dieType;
		bool sideTypeMatch = dieType switch
		{
			Die.DieType.D6 => values.Length == 6,
			Die.DieType.D8 => values.Length == 8,
			_ => throw new System.ArgumentException("Faces and die type don't match"),
		};

		if (!sideTypeMatch)
		{
			throw new System.ArgumentException("Faces and die type don't match");
		}

		faceValues = values;
		material = mat;
		color = col;
	}

	public DieInfo(DieInfo d)
	{
		type = d.type;
		faceValues = d.faceValues;
		material = d.material;
		color = d.color;
	}
}