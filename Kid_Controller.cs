using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Kid_Controller
{
    Database database;
    private List<Kid_Info> Not_Registered_Kids;
    private List<Kid_Info> Registered_Kids;


    public Kid_Controller(Database database) {
        this.database = database;

        Not_Registered_Kids = database.Read_Kids_From_File(KidsOptions.NotRegistered);
        Registered_Kids = database.Read_Kids_From_File(KidsOptions.Registered);
    }

    public void Add_New_Kid_To_List(Kid_Info kid)
    {
        if (kid == null)
            kid = new Kid_Info();

        Console.WriteLine("Input name: ");

        kid.Set_name(Console.ReadLine());

        Console.WriteLine("Input Surname: ");

        kid.Set_LastName(Console.ReadLine());
        kid.Set_Status(false);

        if (Is_Valid(kid.Get_Name()) && Is_Valid(kid.Get_Last_Name()))
        {

            if (Is_New_Name(kid.Get_Name(), kid.Get_Last_Name(), Not_Registered_Kids) && Is_New_Name(kid.Get_Name(), kid.Get_Last_Name(), Registered_Kids))
            {
                Not_Registered_Kids.Add(kid);

                database.Write_Kids_To_File(Not_Registered_Kids, KidsOptions.NotRegistered);

                kid = null;
                return;
            }
            else
            {
                Console.Clear();

                Console.WriteLine("Name already exists!");
                return;
            }
        }
        else
            Console.WriteLine("Not alowed symbols!!!");

        return;
    }

    bool Is_New_Name(string name, string last_name, List<Kid_Info> Kids)
    {
        foreach (Kid_Info kid in Kids)
        {
            if (kid.Get_Name() == name && kid.Get_Last_Name() == last_name)
                return false;
        }
        return true;
    }

    public void Register_Assigned_Kids()
    {
        for (int i = 0; i < Not_Registered_Kids.Count; i++)
        {
            if (Not_Registered_Kids[i].Is_Assigned())
            {
                Registered_Kids.Add(Not_Registered_Kids[i]);
                Not_Registered_Kids.Remove(Not_Registered_Kids[i]);
                i--;
            }
        }
        database.Write_Kids_To_File(Registered_Kids, KidsOptions.Registered);
    }

    public bool Is_Valid(string n)
    {
        Regex check = new Regex(@"^([A-Z][a-z-A-z]+)$");
        return check.IsMatch(n);
    }

    public List<Kid_Info> Get_Kids(KidsOptions kidsOptions)
    {
        if (kidsOptions == KidsOptions.Registered) return Registered_Kids;
        else return Not_Registered_Kids;
    }
}
