using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DataLoader : MonoBehaviour
{
    public string[] Users;


    IEnumerator Start()
    {
        // Load data from your PHP file
        WWW UsersData = new WWW("http://officedash.wuaze.com/UsersData.php");
        yield return UsersData;

        if (string.IsNullOrEmpty(UsersData.error))
        {
            string UserDataString = UsersData.text;
            Debug.Log("Raw Data:\n" + UserDataString);

            // Split items by semicolon (;) — make sure your PHP uses ';' to separate
            Users = UserDataString.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);

            // Print each item
            foreach (string user in Users)
            {
                Debug.Log("User : " + user);
            }
        }
        else
        {
            Debug.LogError("Error loading UsersData.php: " + UsersData.error);
        }

        // get specific value 
        print(GetDataValue(Users[0], " Username"));
        print(GetDataValue(Users[0], "password"));
    }

    // Extract value by index keyword
    string GetDataValue(string data, string key)
    {
        // Each field separated by '|', trim spaces
        string[] fields = data.Split('|');
        foreach (var field in fields)
        {
            var trimmed = field.Trim();
            // Check if field starts with key + colon
            if (trimmed.StartsWith(key + ":"))
            {
                // Extract value after colon
                return trimmed.Substring((key + ":").Length).Trim();
            }
        }
        return ""; // Not found
    }
}
