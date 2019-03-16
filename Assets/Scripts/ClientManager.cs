using Assets.Scripts;
using Microsoft.Extensions.DependencyInjection;
using Networking.PackageParser;
using Networking.PackageParser.Implementations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{

    public InputField UsernameField;
    public InputField PasswordField;

    private ConfigurationService _configurationService;
    private readonly IPackageParser _packageParser;
    private readonly ClientConnection _clientConnection;
    private MenuManager _menuManager;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ClientManager()
    {
        _packageParser = GameContext.ServiceProvider.GetRequiredService<IPackageParser>();
        _clientConnection = GameContext.ServiceProvider.GetRequiredService<ClientConnection>();
    }

    public void Execute()
    {
        Debug.Log("Build test connection.");
        _clientConnection.Connect("127.0.0.1", 6543);
        Debug.Log("Creating streams.");

        Debug.Log("Write login packages");

        string username = UsernameField.text;
        string password = PasswordField.text;

        _packageParser.ParsePackageToStream(new LoginRequestPackage
        {
            Username = username,
            Password = password,
        }, _clientConnection.Writer);

        Debug.Log("Receive Login Response Packages");
        var packageData = _packageParser.ParsePackageFromStream(_clientConnection.Reader);
        Debug.Log($"Received Login Response Package TYPE: {packageData.GetType()} result: {(packageData as LoginResponsePackage).IsValid}");

        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        menuManager.RaceSelectionMenu();
    }

    public void RaceOne()
    {
        Debug.Log("Create Race One");
        int raceId = 1;

        _packageParser.ParsePackageToStream(new RaceCreateRequestPackage
        {
            RaceId = raceId
        }, _clientConnection.Writer);

        Debug.Log("Receive Race One");
        var receivedData = _packageParser.ParsePackageFromStream(_clientConnection.Reader);

        MenuManager menuManager = GetComponentInChildren<MenuManager>(true);
        menuManager.CharacterCreationMenu();
    }
}
