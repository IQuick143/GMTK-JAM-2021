using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Assertions;
#endif

/// <summary>
/// Handles the overarching components of the game
/// </summary>
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PrefabManager))]
[RequireComponent(typeof(MenuManager))]
[RequireComponent(typeof(SpriteManager))]
public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }
	public static InputManager input { get; private set; }
	public static PrefabManager prefab { get; private set; }
	public static MenuManager menu { get; private set; }
	public static SpriteManager sprite {get; private set; }

	public int Currency;

	void Awake() {
		if (Instance == null) Instance = this;
		else if (Instance != this) Destroy(this.gameObject);

		input = this.GetComponent<InputManager>();
		prefab = this.GetComponent<PrefabManager>();
		menu = this.GetComponent<MenuManager>();
		sprite = this.GetComponent<SpriteManager>();
		
		#if UNITY_EDITOR
		Assert.IsNotNull(GameManager.prefab);
		Assert.IsNotNull(GameManager.input);
		Assert.IsNotNull(GameManager.menu);
		Assert.IsNotNull(GameManager.sprite);
		#endif
	}
}