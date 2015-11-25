using Tuples;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class Database : MonoBehaviour
{
    // Attribute used to make this class a singleton.
    private static Database _instance = null;

    // Database's variables
    public string _activeRoom;
    private string _activeRoomDesc;
    private int _activeLanguage;
    private List<Tuple<string, string>> _availableLanguages;
    private Dictionary<string, Tuple<string, string>> _roomObjects;
    private Dictionary<string, string> _roomAccessPoints;
    private Dictionary<string, string> _exploratorHelp;
    private Dictionary<string, string> _inspectorHelp;

    // Event Handler
    public delegate void OnChangeEvent();
    public event OnChangeEvent _OnChange;


    // Constructor
    private Database() { }

    // Singleton
    public static Database Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(Database)) as Database;
            }
            return _instance;
        }
    }

    public int GetLanguageIndex()
    {
        return _activeLanguage;
    }

    public Tuple<string, string> GetLanguage()
    {
        return _availableLanguages[_activeLanguage];
    }

    // Getter & Setter for the active language
    public void SetLanguage(string value)
    {
        switch (value)
        {
            case "PREV":
                if (_activeLanguage > 0)
                {
                    _activeLanguage -= 1; break;
                }
                else
                {
                    throw new System.Exception("ERROR : SetLanguage : Out of bound action");
                }

            case "NEXT":
                if (_activeLanguage < _availableLanguages.Count - 1)
                {
                    _activeLanguage += 1; break;
                }
                else
                {
                    throw new System.Exception("ERROR : SetLanguage : Out of bound action");
                }

            default:
                throw new System.Exception(
                "ERROR : Database.SetLanguage only accepts \"NEXT\" or \"PREV\"");
        }
        LoadHelp(LoadXml(GetLanguage().Item1 + "/aides.xml"));
        LoadRoom(LoadXml(GetLanguage().Item1 + "/salles/" + _activeRoom));
        _OnChange();
    }

    public string GetData(string type, string info)
    {
        switch (type)
        {
            case "language": return _availableLanguages[_activeLanguage].Item2;
            case "explorator": return _exploratorHelp[info];
            case "inspector": return _inspectorHelp[info];
            // Return room info (room, title/desc)
            case "room": return (info == "title" ? _activeRoom : _activeRoomDesc);

            // Return acces info (access, id)
            case "access": return _roomAccessPoints[info];

            // Return object info (ID:title/desc)
            case "object":
                string[] tmp = info.Split(':');
                return (tmp[1] == "title" ? _roomObjects[tmp[0]].Item1 : _roomObjects[tmp[0]].Item2);

            default:
                throw new System.Exception(
           "ERROR : Database.GetData only accepts \"language\", \"explorator\" or \"inspector\".");
        }
    }

    public Texture2D GetFlag()
    {
        string p = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.MyDocuments).Replace('\\', '/');
        p += "/ApplicationRealiteVirtuelleLaRoche/langue/" + GetLanguage().Item1 + "/drapeau.png";

        WWW www = new WWW("file:///" + p);
        while (!www.isDone) { }

        return www.texture;
    }

    // Function to get the available languages
    public List<Tuple<string, string>> getAvailableLanguages()
    {
        return _availableLanguages;
    }

    // Function returning a XmlDocument containing data loaded from FS
    public XmlDocument LoadXml(string p)
    {
        string path = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.MyDocuments).Replace('\\', '/');
        path += "/ApplicationRealiteVirtuelleLaRoche/langue/" + p;

        // NOTE: Maybe change te while loop for a yield
        WWW www = new WWW("file:///" + path);
        while (!www.isDone) { }

        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(www.text);
        return xDoc;
    }

    private void LoadAvailableLanguages(XmlDocument data)
    {
        _availableLanguages = new List<Tuple<string, string>>();
        XmlNodeList langs = data.GetElementsByTagName("langue");
        int i = 0;

        // Fill availableLanguages with data from XML.
        foreach (XmlNode lang in langs)
        {
            XmlAttributeCollection attrs = lang.Attributes;
            _availableLanguages.Add(new Tuple<string, string>(attrs["code"].Value, attrs["nom"].Value));

            // NOTE: There is probably a better way to get index
            if (data.GetElementsByTagName("langueParDefaut")[0].Attributes["code"].Value == attrs["code"].Value)
            { _activeLanguage = i; }
            i += 1;
        }
    }

    private void LoadHelp(XmlDocument data)
    {
        _exploratorHelp = new Dictionary<string, string>();
        _inspectorHelp = new Dictionary<string, string>();
        XmlNodeList nodes = data.GetElementsByTagName("boutons")[0].ChildNodes;
        foreach (XmlNode node in nodes)
        {
            if (node.ToString() != "System.Xml.XmlComment")
            {
                _exploratorHelp.Add(node.Name, node.InnerText);
                _inspectorHelp.Add(node.Name, node.InnerText);
            }
        }

        // Adding exploration commands in corresponding dictionnary
        nodes = data.GetElementsByTagName("exploration")[0].ChildNodes;
        foreach (XmlNode node in nodes)
        {
            if (node.ToString() != "System.Xml.XmlComment")
            {
                _exploratorHelp.Add(node.Name, node.InnerText);
            }
        }

        // Adding inspector commands in the dictionnary
        nodes = data.GetElementsByTagName("inspecteur")[0].ChildNodes;
        foreach (XmlNode node in nodes)
        {
            if (node.ToString() != "System.Xml.XmlComment")
            {
                _inspectorHelp.Add(node.Name, node.InnerText);
            }
        }
    }

    private void LoadRoom(XmlDocument data)
    {
        _roomObjects = new Dictionary<string, Tuple<string, string>>();
        _roomAccessPoints = new Dictionary<string, string>();

        XmlNodeList items = data.GetElementsByTagName("objet");
        foreach (XmlNode item in items)
        {
            _roomObjects.Add(
                item.Attributes["ref"].Value,
                new Tuple<string, string>(
                    item.FirstChild.InnerText,
                    item.LastChild.InnerText)
                );
        }

        items = data.GetElementsByTagName("acces");
        foreach (XmlNode item in items)
        {
            _roomAccessPoints.Add(
                item.Attributes["idAcces"].Value,
                item.Attributes["nom"].Value);
        }

        items = data.GetElementsByTagName("acces");
        _activeRoomDesc = items[0].InnerText;
    }

    public void Awake()
    {
        LoadAvailableLanguages(LoadXml("langues.xml"));
        LoadHelp(LoadXml(GetLanguage().Item1 + "/aides.xml"));
        LoadRoom(LoadXml(GetLanguage().Item1 + "/salles/" + _activeRoom));
    }
}
