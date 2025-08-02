using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    /// <summary>
    /// Die to record
    /// </summary>
    private Die die;

    /// <summary>
    /// Recording of animation
    /// </summary>
	private List<Frame> recording = new();

    /// <summary>
    /// Stores the playback
    /// </summary>
    Coroutine playback = null;

    /// <summary>
    /// Maximum number of frames that can be in the recording.
    /// Prevents freezes if the die clips out
    /// </summary>
    private readonly int maxFrameBuffer = 500;

	public List<Frame> Recording => recording;

	private void Start()
    {
        die = GetComponent<Die>();
    }

	public void RecordSimulation()
    {
        // Delete the old recording
        recording.Clear();

        // Only simulate from code
        Physics.simulationMode = SimulationMode.Script;

        // While die is moving
        while (!die.IsStopped() && recording.Count < maxFrameBuffer)
        {
            // Record location and position
            recording.Add(new(die.transform.position, die.transform.rotation));
            Physics.Simulate(Time.fixedDeltaTime);
		}

		// Return to normal simulation mode
		Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    /// <summary>
    /// Attempts to play the recorded animation.
    /// </summary>
    /// <returns>Whether the animation successfully started.</returns>
    public bool PlaybackSimulation()
    {
        // Prevents playback if the animation is already playing back or doesn't exist
        if (playback == null && recording.Count > 0)
		{
			playback = StartCoroutine(Playback());
            return true;
        }

        return false;
    }

    private IEnumerator Playback()
    {
        //Debug.Log("Starting playback");

        //float startTime = Time.time;

        // Disable physics on the die
        SetPhysicsEnabled(false);

        // Replay each stored frame
        for (int i = 0; i < recording.Count; i++)
        {
            //Frame f = recording.Dequeue();
            Frame f = recording[i];
            die.transform.SetPositionAndRotation(f.position, f.rotation);
            yield return new WaitForFixedUpdate();
        }

        // Reenable physics
        SetPhysicsEnabled(true);

        //Debug.Log($"Playback finished after {Time.time - startTime}");

        playback = null;
    }

	private void SetPhysicsEnabled(bool enabled)
	{
		die.Rigidbody.useGravity = enabled;
		die.Rigidbody.isKinematic = !enabled;
	}

    /// <summary>
    /// Stores a single frame of animation
    /// </summary>
	public struct Frame
	{
		public Vector3 position;
		public Quaternion rotation;
        // if audio is needed, needs to be added here

		public Frame(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}
	}
}
