using UnityEngine;

public class CharacterPicker : MonoBehaviour
{
	public BodyType Hero { get; private set; }

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void ChooseHero(CharacterPickChoice picker)
	{
		Hero = picker.bodyType;
	}

	public void ChooseHero(BodyType bodyType)
	{
		Hero = bodyType;
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}
}

public enum BodyType { Masc, Fem }
