using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    public AnimationRecorder(Die d)
    {
        die = d;
    }

    /// <summary>
    /// Die to record
    /// </summary>
    private Die die;

    /// <summary>
    /// Recording of animation
    /// </summary>
	private Queue<Frame> recording;

    /// <summary>
    /// Stores the playback
    /// </summary>
    Coroutine playback;

	public Queue<Frame> Recording => recording;

    public void Record()
    {
        // Delete the old recording
        recording.Clear();

        // Only simulate from code
        Physics.simulationMode = SimulationMode.Script;

        // While die is moving
        while (!die.IsStopped())
        {
            // Record location and position
            recording.Enqueue(new(die.transform.position, die.transform.rotation));
            Physics.Simulate(Time.fixedDeltaTime);
        }

        // Return to normal simulation mode
        Physics.simulationMode = SimulationMode.FixedUpdate;
    }

    public void PlayRecording()
    {
        // Prevents playback if the animation is already playing back or doesn't exist
        if (playback != null && recording.Count > 0)
        {
            playback = StartCoroutine(Playback());
        }
    }

    private IEnumerator Playback()
    {
        // Disable physics on the die
        SetPhysicsEnabled(false);

        // Replay each stored frame
        while (recording.Count > 0)
        {
            Frame f = recording.Dequeue();
            die.transform.SetPositionAndRotation(f.position, f.rotation);
            yield return new WaitForFixedUpdate();
        }

        // Reenable physics
        SetPhysicsEnabled(true);
        playback = null;
    }

	public void SetPhysicsEnabled(bool enabled)
	{
		die.Rigidbody.useGravity = enabled;
		die.Rigidbody.isKinematic = !enabled;
	}

	public struct Frame
	{
		public Vector3 position;
		public Quaternion rotation;

		public Frame(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}
	}
}
