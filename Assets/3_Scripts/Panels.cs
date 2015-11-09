using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
  // TODO: Create the Editor for those variables
  private GameObject _helpPanel;
  private GameObject _inspectorHelp;
  private GameObject _languagePanel;
  private FirstPersonController _mvtCam;

  // NOTE: I could replace this by using the pointers. 
  private GameObject RightArrow;
  private GameObject LeftArrow;

  // This token contains the active language.
  private string _token;

  // Function used to write only once this lengthy code line
  // NOTE: Maybe change this function to use the pointers instead.
  private void setText(string obj, string t, string a)
  {
    // FIXME: db.GetData will have to be coded
    GameObject.Find(obj).GetComponent<Text>().text = db.GetData(t, a);
  }

  // TODO: Add singleton design pattern to protect from multi creation

  private void HandleHelpPanel()
  {
    setText("TextFleches",     "exploration",  "croixDirectionnelle");
    setText("TextJoystick",    "exploration",  "joystick");
    setText("TextInspector3",  "exploration",  "inspecter");
    setText("TextHelp3",       "exploration",  "aide");
    setText("TextLanguages3",  "exploration",  "langue");
  }

  private void HandleInspectorPanel()
  {
    setText("TextFlecheN",     "inspector",  "haut");
    setText("TextFlecheS",     "inspector",  "bas");
    setText("TextFlecheO",     "inspector",  "gauche");
    setText("TextFlecheE",     "inspector",  "droite");
    setText("TextJoystickN",   "inspector",  "zoomer");
    setText("TextJoystickS",   "inspector",  "dezoomer");
    setText("TextInspector2",  "inspector",  "inspecter");
    setText("TextHelp2",       "inspector",  "aide");
    setText("TextLanguages2",  "inspector",  "langue"); 
  }

  private void HandleLanguagePanel()
  {
    setText("TextLanguage", "language", "name");
    setText("TextHelp1", "exploration", "aide");
    setText("TextLanguages1", "exploration", "langue");
    //NOTE: Flag could be changed here tho
  }



  private void ManageHelp()
  {
    // NOTE: This line of code will probably change according to the new code structure
    if (Inspector.Instance().Activated)
    {
      _inspectorHelp.SetActive(!_inspectorHelp.activeSelf);
      _helpPanel.SetActive(false);
      _languagePanel.SetActive(false);
    }
    else
    {
      _helpPanel.SetActive(!_helpPanel.activeSelf);
      _languagePanel.SetActive(false);
      _inspectorHelp.SetActive(false);
      _characterController.enabled = !isPanelActive(); // disabling camera movement
    }
  }

  private void ManageLanguage()
  {  
    _languagePanel.SetActive(!_languagePanel.activeSelf);
    _helpPanel.SetActive(false);
    _inspectorHelp.SetActive(false);
    _characterController.enabled = !isPanelActive(); // disabling camera movement

    if (_languagePanel.activeSelf)
    {
      // NOTE: The manager will be certainly renamed 
      Maanger.Instance._OnChange += HandleLanguagePanel;
    }else{
      Manager.Instance._OnChange -= HandleLanguagePanel;
    }
  }
  
  private void ToggleArrows()
  {
      if (ActualIndex == langman.GetAvailableLanguages().Count) { RightArrow.SetActive(false); }
      else { RightArrow.SetActive(true); }

      if (ActualIndex == 1) { LeftArrow.SetActive(false); }
      else { LeftArrow.SetActive(true); }
  }

  private void ChangeLanguage(string arrow)
  {
    // Active means there is at least one language after in db
    if (!LeftArrow.activeSelf && arrow = "left"){ return; }
    if (!RightArrow.activeSelf && arrow = "right"){ return; }

    if (arrow == "left"){
      // Set actual language to the previous one.
      // FIXME: db.setLanguage will have to be coded
      db.setLanguage("PREV");
    }else{
      // Set actual language to the next one. 
      db.setLanguage("NEXT");
    }

    // Checking if arrows need to be (de)activated
    ToggleArrows();

    // TODO: Change flag.
  }


  void Awake()
  {
    _mvtCam = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
  }

  public void Update()
  {
    // If token doesn't need to change, no refresh.
    if (_token != db.GetData("language", "code"))
    {
      _token = db.GetData("language", "code");
      HandleInspectorPanel();
      HandleHelpPanel();
    }

    if (Input.GetButtonUp("Aide") )
    {
      ManageHelp();
    }
    else if (Input.GetButtonUp("Langue"))
    {
      ManageLanguage();
    }
    else if(Input.GetAxis("Horizontal") == -1 && _languagePanel.activeSelf)
    {
      ChangeLanguage("right");
    }
    else if(Input.GetAxis("Horizontal") == 1 && _languagePanel.activeSelf)
    {
      ChangeLanguage("left");
    }
  }
}