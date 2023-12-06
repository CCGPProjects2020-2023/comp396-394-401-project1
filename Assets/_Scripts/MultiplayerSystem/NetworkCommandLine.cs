/** Author's Name:          Marcus Ngooi
 *  Last Modified By:       Marcus Ngooi
 *  Date Last Modified:     November 28, 2023
 *  Program Description:    Script for Network Command Line
 *  Revision History:       November 28, 2023 (Marcus Ngooi): Initial NetworkCommandLine script.
 */

using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// The script for a command line helper to run a server, client, and host.
/// </summary>
public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":

                    netManager.StartClient();
                    break;
            }
        }
    }

    /// <summary>
    /// A function that parses command-line arguments passed to the application.
    /// </summary>
    /// <returns>A dictionary where each key-value pair represents a command-line argument and its associated value.</returns>
    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}
