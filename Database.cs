using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Database
{
    public string notRegisteredKidsPath;
    public string RegisteredKidsPath;
    public string path_gift;
    public string path_users;
    public string path_history;
    public string path_feedback;

    public Database()
    {
        string currentDir = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        notRegisteredKidsPath = currentDir + "\\Data.txt";
        RegisteredKidsPath = currentDir + "\\Data2.txt";
        path_gift = currentDir + "\\gifts.txt";
        path_users = currentDir + "\\user_info.txt";
        path_history = currentDir + "\\history.txt";
        path_feedback = currentDir + "\\feedbacks.txt";
    }

    public List<Kid_Info> Read_Kids_From_File(KidsOptions kidsOptions)
    {
        String path = "";

        if (kidsOptions == KidsOptions.Registered) path = RegisteredKidsPath;
        if (kidsOptions == KidsOptions.NotRegistered) path = notRegisteredKidsPath;
        List<Kid_Info> Kids = new List<Kid_Info>();
        int flag = 0;
        string text = File.ReadAllText(path);
        string[] lines = text.Split(Environment.NewLine);
        Kid_Info kid = null;
        foreach (var line in lines)
        {
            if (flag == 0)
            {
                if (kid == null)
                    kid = new Kid_Info();
                kid.Set_name(line);
                flag++;
            }
            else if (flag == 1)
            {
                kid.Set_LastName(line);
                flag++;
            }
            else if (flag == 2)
            {

                if (line != "0")
                {
                    kid.Set_Present(line);
                    kid.Set_Status(true);
                }
                else kid.Set_Status(false);
                Kids.Add(kid);
                flag = 0;
                kid = null;
            }

        }
        return Kids;

    }
    public void Write_Kids_To_File(List<Kid_Info> Kids, KidsOptions kidsOptions)
    {
        String path = "";
        if (kidsOptions == KidsOptions.Registered) path = RegisteredKidsPath;
        if (kidsOptions == KidsOptions.NotRegistered) path = notRegisteredKidsPath;

        using (StreamWriter sw = File.CreateText(path))
        {
            foreach (Kid_Info kido in Kids)
            {
                sw.WriteLine(kido.Get_Name());
                sw.WriteLine(kido.Get_Last_Name());

                if (kido.Is_Assigned())
                {
                    sw.WriteLine(kido.Get_Present());
                }
                else sw.WriteLine("0");
            }
        }
    }

    public List<Gift> Read_Gifts_From_File()
    {
        string text = File.ReadAllText(path_gift);
        string[] lines = text.Split(Environment.NewLine);
        int flag = 0;
        Gift g = new Gift();
        List<Gift> result = new List<Gift>();

        foreach (var line in lines)
        {
            if (flag == 0)
            {
                g = new Gift();
                g.Set_Gift(line);
                flag++;
            }
            else if (flag == 1)
            {
                int a = Int32.Parse(line);
                g.Set_Amount(a);
                flag++;
            }
            else if (flag == 2)
            {
                g.Set_status(line);
                flag = 0;
                result.Add(g);
                g = null;
            }
        }

        return result;
    }

    public List<User> Read_Users()
    {
        List<User> result = new List<User>();
        string text = File.ReadAllText(path_users);
        string[] lines = text.Split("\n");
        foreach (var line in lines)
        {
            if (line == "") continue;

            string[] details = line.Split(";");
            User temp = new User();
            temp.Set_Name(details[0]);
            temp.Set_Surname(details[1]);
            temp.Set_Username(details[2]);
            temp.Set_Password(details[3]);

            result.Add(temp);
        }

        return result;
    }

    public void Write_All_Users_To_File(List<User> pUsers)
    {
        using (StreamWriter sw = File.CreateText(path_users))
        {
            foreach (User user in pUsers)
            {
                sw.Write(user.Get_Name() + ";");
                sw.Write(user.Get_Surname() + ";");
                sw.Write(user.Get_Username() + ";");
                sw.Write(user.Get_Password() + "\n");
            }
        }
    }
    public List<History> Read_History()
    {

        List<History> result = new List<History>();
        string text = File.ReadAllText(path_history);
        string[] lines = text.Split("\n");
        foreach (var line in lines)
        {
            if (line == "") continue;

            string[] details = line.Split(";");
            History temp = new History(details[0], DateTime.Parse(details[1]));

            result.Add(temp);
        }

        return result;
    }
    public void Write_History(History history)
    {
        using (StreamWriter sw = new StreamWriter(path_history, true))
        {
            sw.Write(history.get_Command());
            sw.Write(";");
            sw.Write(history.get_Executed());
            sw.Write("\n");
        }
    }

    public void Write_Feedback_To_File(List<KeyValuePair<bool, string>> feedbacks)
    {
        using (StreamWriter sw = File.CreateText(path_feedback))
        {
            foreach (KeyValuePair<bool, string> feedback in feedbacks)
            {
                if (feedback.Key) sw.Write("Positive\n");
                if (!feedback.Key) sw.Write("Negative\n");
                sw.Write(feedback.Value + "\n\n");
            }
        }
    }
    public List<KeyValuePair<bool, string>> Read_Feedback_From_File()
    {
        List<KeyValuePair<bool, string>> result = new List<KeyValuePair<bool, string>>();
        string text = File.ReadAllText(path_feedback);
        string[] lines = text.Split("\n");

        bool positive = false;
        string feedback;

        foreach (var line in lines)
        {
            if (line == "") continue;
            if (line == "Positive") positive = true;
            else if (line == "Negative") positive = false;
            else
            {
                feedback = line;
                result.Add(new KeyValuePair<bool, string>(positive, line));
            }
        }

        return result;
    }
}

