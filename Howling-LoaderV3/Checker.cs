using System;
using System.IO;
using System.Security.Cryptography;

public class Checker
{
    public bool CheckFiles()
    {
        string Authed_ShouldBe = "da5404d098b1fc3f40d8e473e9676656";
        string JWT_ShouldBe = "77ff6f4da066fd54ec20777fbd9154fa";
        bool isLegit = false;
        if (CalculateMD5Checksum($"{Environment.CurrentDirectory}\\Authed.dll") == Authed_ShouldBe)
        {
            isLegit = true;
        }
        else
        {
            isLegit = false;
        }

        if (CalculateMD5Checksum($"{Environment.CurrentDirectory}\\jose-jwt.dll") == JWT_ShouldBe)
        {
            isLegit = true;
        }
        else
        {
            isLegit = false;
        }
        return isLegit;
    }

    private string CalculateMD5Checksum(string _file)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(_file))
            {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}