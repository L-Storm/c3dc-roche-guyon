using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Panels : MonoBehaviour
{
	// Attribute used to make this class a singleton.
	private static Panels _instance = null;

	// TODO: Create the Editor for those variables
	public GameObject _helpPanel;
	public GameObject _inspectorHelp;
	public GameObject _languagePanel;

	private FirstPersonController _mvtCam;
	private Database _db;
	private GameObject _rightArrow;
	private GameObject _leftArrow;

	// This token contains the active language.
	private string _token;

	// Singleton
	public static Panels Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType(typeof(Panels)) as Panels;
			}
			return _instance;
		}
	}

	// Function used to write only once this lengthy code line
	private void setText(GameObject obj, string txt, string t, string a)
	{
		obj.transform.Find("Panel/" + txt).GetComponent<Text>().text = _db.GetData(t, a);
	}

	private void HandleHelpPanel()
	{
		setText(_helpPanel, "TextFleches", "explorator", "croixDirectionnelle");
		setText(_helpPanel, "TextJoystick", "explorator", "joystick");
		setText(_helpPanel, "TextInspector", "explorator", "inspecter");
		setText(_helpPanel, "TextHelp", "explorator", "aide");
		setText(_helpPanel, "TextLanguages", "explorator", "langue");
	}

	private void HandleInspectorPanel()
	{
		setText(_inspectorHelp, "TextFlecheN", "inspector", "haut");
		setText(_inspectorHelp, "TextFlecheS", "inspector", "bas");
		setText(_inspectorHelp, "TextFlecheO", "inspector", "gauche");
		setText(_inspectorHelp, "TextFlecheE", "inspector", "droite");
		setText(_inspectorHelp, "TextJoystickN", "inspector", "zoomer");
		setText(_inspectorHelp, "TextJoystickS", "inspector", "dezoomer");
		setText(_inspectorHelp, "TextInspector", "inspector", "inspecter");
		setText(_inspectorHelp, "TextHelp", "inspector", "aide");
		setText(_inspectorHelp, "TextLanguages", "inspector", "langue");
	}

	private void HandleLanguagePanel()
	{
		setText(_languagePanel, "TextLanguage", "language", "name");
		setText(_languagePanel, "TextHelp", "explorator", "aide");
		setText(_languagePanel, "TextLanguages", "explorator", "langue");

		Transform f = _languagePanel.transform.Find("Panel/Flag");
    	Rect rec = f.GetComponent<RectTransform>().rect;
		Texture2D tex = _db.GetFlag();
		Vector2 s = new Vector2(tex.width, tex.height);
		f.GetComponent<Image>().sprite =
	    Sprite.Create(tex, new Rect(0,0,s.x , s.y), new Vector2(0.5f,0.5f), (s.x / rec.width));
	}

	public bool isPanelActive()
	{
		return _languagePanel.activeSelf || _helpPanel.activeSelf || _inspectorHelp.activeSelf;
	}

	private void ManageHelp()
	{
		// NOTE: Inspector will certainly change with industrialization
 		if (StateManager.Instance.GetState() == "Inspector")
 		{
 			_inspectorHelp.SetActive(!_inspectorHelp.activeSelf);
 			_helpPanel.SetActive(false);
 			_languagePanel.SetActive(false);
 		} else {
			_helpPanel.SetActive(!_helpPanel.activeSelf);
			_languagePanel.SetActive(false);
			_inspectorHelp.SetActive(false);
			_mvtCam.enabled = !isPanelActive(); // disabling camera movement
 		}
	}

	private void ManageLanguage()
	{
		_languagePanel.SetActive(!_languagePanel.activeSelf);
		_helpPanel.SetActive(false);
		_inspectorHelp.SetActive(false);
		_mvtCam.enabled = !isPanelActive(); // disabling camera movement

		if (_languagePanel.activeSelf)
		{
			Database.Instance._OnChange += HandleLanguagePanel;
		} else {
			Database.Instance._OnChange -= HandleLanguagePanel;
		}
	}

	private void ToggleArrows()
	{
		if (_db.GetLanguageIndex() == _db.getAvailableLanguages().Count - 1) {
			_rightArrow.SetActive(false);
		} else {
			_rightArrow.SetActive(true);
		}

		if (_db.GetLanguageIndex() == 0) { 
			_leftArrow.SetActive(false);
		} else { 
			_leftArrow.SetActive(true);
		}
	}

	private void ChangeLanguage(string dir)
	{
		// Active means there is at least one language after in _db
		if (!_leftArrow.activeSelf && dir == "left") { return; }
		if (!_rightArrow.activeSelf && dir == "right") { return; }

		if (dir == "left") {
			// Set actual language to the previous one.
			_db.SetLanguage("PREV");
		} else {
			// Set actual language to the next one. 
			_db.SetLanguage("NEXT");
		}

		// Checking if arrows need to be (de)activated
		ToggleArrows();
	}


	void Awake()
	{
		_mvtCam = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
		_db = Database.Instance;
		_rightArrow = _languagePanel.transform.Find("Panel/FlecheD").gameObject;
		_leftArrow  = _languagePanel.transform.Find("Panel/FlecheG").gameObject;
		ToggleArrows();
	}

	public void Update()
	{
		// If token doesn't need to change, no refresh.
		if (_token != _db.GetData("language", "code"))
		{
			_token = _db.GetData("language", "code");
			HandleInspectorPanel();
			HandleHelpPanel();
			HandleLanguagePanel();
		}

		if (Input.GetButtonUp("Aide"))
		{
			ManageHelp();
		}
		else if (Input.GetButtonUp("Langue"))
		{
			ManageLanguage();
		}
		else if (Input.GetButtonUp("Droite") && _languagePanel.activeSelf)
		{
			ChangeLanguage("right");
		}
		else if (Input.GetButtonUp("Gauche") && _languagePanel.activeSelf)
		{
			ChangeLanguage("left");
		}
	}
}
