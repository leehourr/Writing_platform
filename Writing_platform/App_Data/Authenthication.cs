using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Data.Entity;

public class Authenthication
{
    private int id;

    public int Id  
    {
        get { return id; }  
        set { id = value; } 
    }
    public string Name { get; set; }    
    public int Status_code { get; set; }    
    public string Message { get; set; }


    public string GetStringFromHash(byte[] hash)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            result.Append(hash[i].ToString("X2"));
        }
        return result.ToString();
    }

    public string GetSHA256Hash(string input)
    {
        SHA256 sha256 = SHA256Managed.Create();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] hash = sha256.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }
}
public class AuthDBContext : DbContext
{
    public DbSet<Authenthication> Auth { get; set; }
}
