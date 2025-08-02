using System.Collections.Generic;
using UnityEngine;
using Modifiers;
using System.Linq;

public class Die : MonoBehaviour
{
	[SerializeField] private List<int> defaultFaces;
	[SerializeField] private Transform meshTransform;

	[SerializeField] private List<DieFace> faces;
	[SerializeField] private MaterialModifiers material = MaterialModifiers.None;
	[SerializeField] private ColorModifiers color = ColorModifiers.None;

	private Rigidbody rb;
	private bool isRolling;
	public AnimationRecorder recorder;

	public int Sides => faces.Count;
	public List<DieFace> Faces => faces;

	public MaterialModifiers Material => material;
	public ColorModifiers Color => color;

	public void ApplyMaterial(MaterialModifiers mat) => material = mat;
	public void ApplyColor(ColorModifiers col) => color = col;

	public Rigidbody Rigidbody => rb;
	public AnimationRecorder Recorder => recorder;

	// For debugging
	//float startTime;
	//[SerializeField] bool doRolls = true;

	private void Start()
	{
		if (defaultFaces.Count > 0)
		{
			SetFaces(defaultFaces.ToArray());
		}
		else
		{
			SetFaces(1, 2, 3, 4, 5, 6);
		}

		rb = GetComponent<Rigidbody>();
		recorder = GetComponent<AnimationRecorder>();

		//ApplyPhysics();
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
		isRolling = true;
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
