using System;
using System.Collections.Generic;
using System.Text;

class User
{
    private string username;
    private string password;
    private string name;
    private string surname;
    public void Set_Username(string n)
    {
        username = n;
    }
    public string Get_Username()
    {
        return username;
    }
    public void Set_Password(string n)
    {
        password = n;
    }
    public string Get_Password()
    {
        return password;
    }
    public void Set_Name(string n)
    {
        name = n;
    }
    public string Get_Name()
    {
        return name;
    }
    public void Set_Surname(string n)
    {
        surname = n;
    }
    public string Get_Surname()
    {
        return surname;
    }
}