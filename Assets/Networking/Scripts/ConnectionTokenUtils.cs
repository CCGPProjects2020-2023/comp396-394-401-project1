/*
    Author's Name:          Audrey Bernier Larose
    Last Modified By:       Audrey Bernier Larose
    Last Date Modified:     December 12, 2023
    Program Description:    Utility class that takes care of creating tokens.
    Revision History:       December 12, 2023: Initial script and documentation.                            
 */
//***The following is based on this tutorial:   https://www.youtube.com/watch?v=KqpMOdPj3co&list=PLyDa4NP_nvPfHhPuumJylSj8jXyULsT1X&index=1YouTube
using System;


public static class ConnectionTokenUtils
{
    /// <summary>
    /// Create new random Token
    /// </summary>
    public static byte[] NewToken() => Guid.NewGuid().ToByteArray();

    /// <summary>
    /// Converts a Token into a Hash format
    /// </summary>
    /// <param name="token">Token to be hashed</param>
    /// <returns>Token hash</returns>
    public static int HashToken(byte[] token) => new Guid(token).GetHashCode();

    /// <summary>
    /// Converts a Token into a String
    /// </summary>
    /// <param name="token">Token to be parsed</param>
    /// <returns>Token as a string</returns>
    public static string TokenToString(byte[] token) => new Guid(token).ToString();
}