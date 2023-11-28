using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

class Kid_Info
{
    private string name;
    private string last_Name;
    private string Present;
    private bool fully_assigned;
    public void Set_name(string name2)
    {
        name = name2;
    }
    public void Set_LastName(string last_name)
    {
        last_Name = last_name;
    }
    public void Set_Present(string present)
    {
        Present = present;
    }
    public string Get_Name()
    {
        return name;
    }
    public string Get_Last_Name()
    {
        return last_Name;
    }
    public string Get_Present()
    {
        return Present;
    }
    // Status to indetify if kid has present or not.
    public void Set_Status(bool a)
    {
        fully_assigned = a;
    }
    public bool Get_Status()
    {
        return fully_assigned;
    }
}
class Gift
{
    private int amount = 0;
    private string gift;
    private string status = "Laisva"; // laisva, nelaisva (kam priskirta), gaminama, pagaminta
    private User assined = new User();
    public void Set_status(string stat)
    {
        status = stat;
    }
    public string Get_Status()
    {
        return status;
    }
    public User Get_assignee()
    {
        return assined;
    }
    public void Set_assinee(User user)
    {
        assined = user;
    }
    public void Set_Gift(string g)
    {
        gift = g;
    }
    public int Get_Amount()
    {
        return amount;
    }
    public void Set_Amount(int num)
    {
        amount = num;
    }
    public string Get_Gift()
    {
        return gift;
    }
}

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

namespace SantaClause
{
    class Program
    {   
        static private bool authorized = false;
        static private List<Kid_Info> kids = new List<Kid_Info>();
        static private List<Gift> Gifts = new List<Gift>();
        static private List<Kid_Info> Fully_Registered_Kids = new List<Kid_Info>();

        static private string path_data;
        static private string path_data2;
        static private string path_gift;
        static private string path_users;

        static void Main(string[] args)
        {
            string currentDir = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.ToString();
            path_data = currentDir + "\\Data.txt";
            path_data2 = currentDir +"\\Data2.txt";
            path_gift = currentDir + "\\gifts.txt";
            path_users = currentDir + "\\user_info.txt";

            List<User> users = new List<User>();
            Gift gift = new Gift();
            Kid_Info kid = new Kid_Info();

            kids = Read_File(path_data);
            Fully_Registered_Kids = Read_File(path_data2);
            users = Read_Users(path_users);

            Read_Gifts(ref Gifts);
            while (!authorized)
            {
                Login();
            }
            while (true)
            {

                Menu();

                bool exists = false;
                int input_key;

                while (!int.TryParse(Console.ReadLine().ToString(), out input_key))
                {
                    Console.WriteLine(" Unknown command. Enter Command from Menu.....");
                    Console.WriteLine(input_key + "\n");
                }
                if (input_key == (int)User_Input.Exit)
                    Environment.Exit(0);
                if (input_key == (int)User_Input.Add_New_Child)
                {
                    Add_New_Child_To_List(kid);

                    kid = null;
                }
                else if (input_key == (int)User_Input.Add_New_Gift)
                {
                    Add_Gifts_To_List(gift, exists, users);
                    gift = null;
                }
                else if (input_key == (int)User_Input.Add_Spec_Gift_To_Kid)
                {
                    Add_Gift_To_Specific_Child(gift, exists);
                    gift = null;
                }
                else if (input_key == (int)User_Input.Add_Spec_Gift_To_Rand_Kid)
                {
                    Add_Specific_Gift_To_Rand_Child(gift, exists);
                    gift = null;
                }
                else if (input_key == (int)User_Input.Rand_Gift_To_Rand_Kid)
                {
                    Add_Rand_Gift_To_Rand_Child(exists);
                }
                else if (input_key == (int)User_Input.Lazy_Mode)
                {
                    Lazy_Mode();
                }
                else if (input_key == (int)User_Input.Display_All_Kids)
                {
                    Display_All_Kids();
                }
                else if (input_key == (int)User_Input.Display_Gift_List)
                {
                    Gift_List(ref Gifts);
                }
                else if (input_key == (int)User_Input.Display_Unassignes)
                {
                    int amount = 0;
                    Console.WriteLine(" Number of assigned children: " + Fully_Registered_Kids.Count);
                    Console.WriteLine(" Number of unassigned children: " + kids.Count);

                    foreach (Gift tmp_gift in Gifts)
                        if (tmp_gift.Get_Amount() == 0)
                            amount++;
                    Console.WriteLine(" Number of unassigned gifts: " + amount);
                }
                else if (input_key == (int)User_Input.Change_Status)
                {
                    Console.WriteLine("kokiai dovanai reikia pakeisti statusa");

                    String input = Console.ReadLine().ToString();
                    Console.WriteLine("pakeisti i statusa: ");

                    foreach (Gift i in Gifts)
                    {
                        if (i.Get_Gift() == input)
                        {
                            input = Console.ReadLine().ToString();

                            if (input == "laisva" || input == "nelaisva" || input == "daroma" || input == "padaryta")
                            {
                                i.Set_status(input);
                            }
                            //  break;
                        }

                    }
                }
                else if (input_key == (int)User_Input.Display_User_Info)
                {
                    Console.WriteLine("Input the user's username: ");
                    string username = Console.ReadLine();
                    bool found = false;
                    foreach (User user in users)
                    {
                        if (user.Get_Username() == username)
                        {
                            Display_User_Info(user);
                            found = true;
                            break;
                        }
                    }

                    if (!found) { Console.WriteLine("No such user found"); }
                }

                using (StreamWriter sw = File.CreateText(path_gift))
                    {
                        foreach (Gift gifts_tmp in Gifts)
                        {
                            User u = gifts_tmp.Get_assignee();
                            sw.WriteLine(gifts_tmp.Get_Gift());
                            sw.WriteLine(gifts_tmp.Get_Amount());
                            sw.WriteLine(gifts_tmp.Get_Status());
                        }
                    }
                    Console.WriteLine("press any key to continue...\n");
                    Console.ReadKey();
                    Console.Clear();
                }

            }
            static void Login()
            {
                Console.Write("****************************************************************************\n");
                Console.Write("**                             Welcome!                                   **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**           Press         |           Operation                          **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             1           |               Login                          **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             2           |            Registration                      **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**                TO EXIT PROGRAM TYPE 0 AND PRESS ENTER                  **\n");
                Console.Write("****************************************************************************\n");

                int input_key;

                while (!int.TryParse(Console.ReadLine().ToString(), out input_key))
                {
                    Console.WriteLine(" Unknown command. Enter Command from Menu.....");
                    Console.WriteLine(input_key + "\n");
                }
                if (input_key == (int)User_Input.Exit)
                    Environment.Exit(0);
                if (input_key == (int)User_Input.Login)
                {
                    LoginPage();
                }
                else if (input_key == (int)User_Input.Register)
                {
                    RegisterPage();
                }


            }

            static void LoginPage()
            {
                Console.WriteLine("Enter your username: ");
                string username = Console.ReadLine();

                Console.WriteLine("Enter your password: ");
                string password = Console.ReadLine();

                if (IsValidLogin(username, password))
                {
                    Console.WriteLine("Login successful!");
                    authorized = true;
                }
                else
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                }
                Console.WriteLine("press any key to continue: ");

                Console.ReadKey();
                Console.Clear();
            }

            static bool IsValidLogin(string username, string password)
            {
                try
                {
                    string[] lines = File.ReadAllLines(path_users);

                    foreach (string line in lines)
                    {
                        string[] userInfo = line.Split(';');
                        if (userInfo.Length == 4)
                        {
                            string storedUsername = userInfo[2].Trim();
                            string storedPassword = userInfo[3].Trim();

                            if (storedUsername == username && storedPassword == password)
                            {
                                return true; // Valid login
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error reading user_info.txt: {ex.Message}");
                }

                return false;
            }

            static void RegisterPage()
            {
                User newUser = new User();

                Console.WriteLine("Enter a username: ");
                newUser.Set_Username(Console.ReadLine());

                Console.WriteLine("Enter a password: ");
                newUser.Set_Password(Console.ReadLine());

                Console.WriteLine("Enter your first name: ");
                newUser.Set_Name(Console.ReadLine());

                Console.WriteLine("Enter your surname: ");
                newUser.Set_Surname(Console.ReadLine());

                if (!IsUsernameTaken(newUser.Get_Username()))
                {
                    WriteUserToFile(newUser);
                    Read_Users(path_users);
                    Console.WriteLine("Registration successful! You can now log in.");
                }
                else
                {
                    Console.WriteLine("Username already exists. Please choose another username.");
                }

                Console.WriteLine("press any key to continue: ");

                Console.ReadKey();
                Console.Clear();
                return;
            }

            static void WriteUserToFile(User user)
            {
                // Append the new user information to the user_info.txt file
                string userInfo = $"{user.Get_Name()};{user.Get_Surname()};{user.Get_Username()};{user.Get_Password()}";
                File.AppendAllLines(path_users, new[] { userInfo });
            }

            static bool IsUsernameTaken(string username)
            {
                try
                {
                    string[] lines = File.ReadAllLines(path_users);

                    for (int i = 0; i < lines.Length; i += 4)
                    {
                        string storedUsername = lines[i].Trim();

                        if (storedUsername == username)
                        {
                            return true;
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error reading user_info.txt: {ex.Message}");
                }

                return false;
            }
            static void Menu()
            {
                Console.Write("****************************************************************************\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**           Santa Clause gift registry system                            **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**                       Commands                                         **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**           Press         |           Operation                          **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             1           | Add new child to list                        **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             2           | Add new gift to list                         **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             3           | Set specific gift to wanted child            ** \n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             4           | Set specific gift to random child            **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             5           | Set random gift to random child              **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             6           | Activate  Lazy mode                          **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             7           | Display all kids(registred/unregistred)     **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             8           | Display gift list                            **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             9           | Display Number of unassigned children/gifts  **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             10          | Change status                                **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**             11          | Display user info                            **\n");
                Console.Write("****************************************************************************\n");
                Console.Write("**                TO EXIT PROGRAM TYPE 0 AND PRESS ENTER                  **\n");
                Console.Write("****************************************************************************\n");
            }

        static List<User> Read_Users(string Path)
        {
            List<User> result = new List<User>();
            string text = File.ReadAllText(Path);
            string[] lines = text.Split(Environment.NewLine);
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

        static void Display_User_Info(User user)
        {
            Console.WriteLine(user.Get_Name() + " " + user.Get_Surname() + ":");
            Console.WriteLine("Username: " + user.Get_Username());
            Console.WriteLine("Password: *********");
        }

        static List<Kid_Info> Read_File(string Path)
            {

                List<Kid_Info> Kids = new List<Kid_Info>();
                int flag = 0;
                string text = File.ReadAllText(Path);
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
            static bool New_Name(string name, string last_name, List<Kid_Info> Kids)
            {
                foreach (Kid_Info kid in Kids)
                {
                    if (kid.Get_Name() == name && kid.Get_Last_Name() == last_name)
                        return false;
                }
                return true;
            }
            static void Write_To_File(ref List<Kid_Info> Kids, string Path)
            {
                using (StreamWriter sw = File.CreateText(Path))
                {
                    foreach (Kid_Info kido in Kids)
                    {
                        sw.WriteLine(kido.Get_Name());
                        sw.WriteLine(kido.Get_Last_Name());

                        if (kido.Get_Status())
                        {
                            sw.WriteLine(kido.Get_Present());
                        }
                        else sw.WriteLine("0");
                    }
                }
            }
            static void Gift_List(ref List<Gift> gifts)
            {
                if (Gifts.Count == 0)
                {
                    Console.WriteLine("There's no gifts assigned...");
                }
                else
                {
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("                     Gift list ");
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("           Gift --> amount  request of this gift                ");
                    Console.WriteLine("*****************************************************************************");
                    foreach (Gift gift in gifts)
                    {
                        Console.WriteLine(gift.Get_Gift() + " --> " + gift.Get_Amount() + " -> " + gift.Get_Status() + "\n");
                    }
                }
            }
            static void Fully_Registered(ref List<Kid_Info> Kids, ref List<Kid_Info> Fully_R)
            {
                for (int i = 0; i < Kids.Count; i++)
                {
                    if (Kids[i].Get_Status())
                    {
                        Fully_R.Add(Kids[i]);
                        Kids.Remove(Kids[i]);
                        i--;
                    }
                }
                Write_To_File(ref Fully_R, path_data2);

            }
            static void Read_Gifts(ref List<Gift> Gifts)
            {
                string text = File.ReadAllText(path_gift);
                string[] lines = text.Split(Environment.NewLine);
                int flag = 0;
                Gift g = new Gift();

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
                        Gifts.Add(g);
                        g = null;
                    }
                }
            }
            static void Find_Gift(ref List<Gift> Gifts, string gift, ref bool exists)
            {
                foreach (Gift gifts_tmp in Gifts)
                {
                    if (gifts_tmp.Get_Gift() == gift)
                    {
                        gifts_tmp.Set_Amount(gifts_tmp.Get_Amount() + 1);
                        exists = true;
                        return;
                    }
                }
            }
            static void Find_And_Substract_Gift(Kid_Info k, ref List<Gift> Gifts)
            {
                if (k.Get_Status())
                {
                    foreach (Gift temp_gift in Gifts)
                    {
                        if (temp_gift.Get_Gift() == k.Get_Present())
                            temp_gift.Set_Amount(temp_gift.Get_Amount() - 1);
                    }
                }
            }
            static bool Is_Valid(string n)
            {
                Regex check = new Regex(@"^([A-Z][a-z-A-z]+)$");
                return check.IsMatch(n);
            }
            static void Add_New_Child_To_List(Kid_Info kid)
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

                    if (New_Name(kid.Get_Name(), kid.Get_Last_Name(), kids) && New_Name(kid.Get_Name(), kid.Get_Last_Name(), Fully_Registered_Kids))
                    {
                        kids.Add(kid);

                        Write_To_File(ref kids, path_data);

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
            static void Add_Gifts_To_List(Gift gift, bool exists, List<User> users)
            {
                Console.WriteLine("adding gifts... \n");
                if (gift == null)
                    gift = new Gift();
                gift.Set_Gift(Console.ReadLine());

                foreach (Gift gifts_tmp in Gifts)
                {
                    if (gifts_tmp.Get_Gift() == gift.Get_Gift())
                    {
                        exists = true;
                        break;
                    }
                };
                Console.WriteLine("is there any elf that coul take this gift? (y/n)");
                string input = Console.ReadLine().ToString();
                if (input == "y")
                {
                    Console.WriteLine("input elf username to assing him the pressent");
                    input = Console.ReadLine().ToString();
                    FindUser(users, input, gift);
                    gift.Set_status("nelaisva");
                }
                else
                {
                    Console.WriteLine("ok");
                }
                if (!exists)
                {
                    gift.Set_Amount(0);
                    Gifts.Add(gift);
                }

            }
            static void FindUser(List<User> users, string name, Gift gift)
            {
                foreach (User i in users)
                {
                    if (name == i.Get_Name())
                    {
                        Console.WriteLine("user found");
                        gift.Set_assinee(i);
                    }
                }
            }
            static void Change_Status(Gift gift, String input)
            {
                if (input == "laisva" || input == "nelaisva" || input == "daroma" || input == "padaryta")
                {
                    gift.Set_status(input);
                }
            }
            static void Add_Gift_To_Specific_Child(Gift gift, bool exists)
            {
                Kid_Info tmp_kid = new Kid_Info();
                //set specific gift to wanted child
                string name, last_name;
                Console.WriteLine("enter name of the kid: ");
                name = Console.ReadLine().ToString();
                Console.WriteLine("enter Last Name of the kid: ");
                last_name = Console.ReadLine().ToString();
                Console.WriteLine("enter present to give: ");
                string present = Console.ReadLine().ToString();
                bool found = false;

                if (gift == null)
                    gift = new Gift();

                foreach (Kid_Info kido in kids)
                {
                    if (kido.Get_Name().Equals(name) && kido.Get_Last_Name().Equals(last_name))
                    {
                        Find_And_Substract_Gift(kido, ref Gifts);

                        kido.Set_Present(present);
                        kido.Set_Status(true);
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    Find_Gift(ref Gifts, present, ref exists);

                    if (!exists)
                    {
                        gift.Set_Gift(present);
                        gift.Set_Amount(1);
                        Gifts.Add(gift);
                    }
                }



                Fully_Registered(ref kids, ref Fully_Registered_Kids);
                Write_To_File(ref kids, path_data);
            }
            static void Add_Specific_Gift_To_Rand_Child(Gift gift, bool exists)
            {
                Random rd = new Random();
                int random_index = rd.Next(0, kids.Count);
                Console.WriteLine("What gift you want to give for random child? \n");
                if (gift == null)
                    gift = new Gift();
                gift.Set_Gift(Console.ReadLine());

                Find_Gift(ref Gifts, gift.Get_Gift(), ref exists);

                Find_And_Substract_Gift(kids[random_index], ref Gifts);

                if (!exists)
                {
                    gift.Set_Amount(1);
                    Gifts.Add(gift);
                }

                kids[random_index].Set_Present(gift.Get_Gift());
                kids[random_index].Set_Status(true);

                Console.WriteLine("Present has been asigned ...\n");

                Fully_Registered(ref kids, ref Fully_Registered_Kids);
                Write_To_File(ref kids, path_data);
            }
            static void Add_Rand_Gift_To_Rand_Child(bool exists)
            {
                if (kids.Count > 0 && Gifts.Count > 0)
                {
                    Random rd = new Random();
                    int random_child = rd.Next(0, kids.Count);
                    int random_gift = rd.Next(0, Gifts.Count);

                    if (kids[random_child].Get_Present() != Gifts[random_gift].Get_Gift())
                    {

                        foreach (Gift gg in Gifts)
                        {
                            if (gg.Get_Gift() == kids[random_child].Get_Present())
                            {
                                gg.Set_Amount(gg.Get_Amount() - 1);
                                break;
                            }
                        }
                        kids[random_child].Set_Present(Gifts[random_gift].Get_Gift());
                        kids[random_child].Set_Status(true);

                        Find_Gift(ref Gifts, Gifts[random_gift].Get_Gift(), ref exists);
                    }
                    else
                    {
                        kids[random_child].Set_Present(Gifts[random_gift].Get_Gift());
                        kids[random_child].Set_Status(true);
                    }

                    Fully_Registered(ref kids, ref Fully_Registered_Kids);
                    Write_To_File(ref kids, path_data);
                }
                else
                {
                    Console.WriteLine("there's no  kids  or gifts assigned!!!");
                    Console.WriteLine("press any key to continue: ");

                    Console.ReadKey();
                    Console.Clear();

                    return;
                }

            }
            static void Lazy_Mode()
            {
                Console.WriteLine("Warning!!! Lazy mode will effect all fully unregistered children. After lazy button activated all childres will have assigned to gifts from gift list!!! To continue this mode please comfirm operation (YES/NO) \n");
                string input = Console.ReadLine();
                if (input == "YES")
                {
                    int index = 0;
                    foreach (Kid_Info k in kids)
                    {
                        if (index == Gifts.Count)
                            index = 0;

                        Find_And_Substract_Gift(k, ref Gifts);

                        k.Set_Present(Gifts[index].Get_Gift());
                        Gifts[index].Set_Amount(Gifts[index].Get_Amount() + 1);
                        k.Set_Status(true);

                        index++;
                    }
                    Fully_Registered(ref kids, ref Fully_Registered_Kids);
                    Write_To_File(ref kids, path_data);
                }
                else
                {
                    return;
                }
            }
            static void Display_All_Kids()
            {
                if (kids.Count == 0)
                {
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("There is no unregistered kids...");
                    Console.WriteLine("*****************************************************************************");
                }
                else
                {
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("Not fully registered kids: ");
                    Console.WriteLine("*****************************************************************************");
                    foreach (var kido in kids)
                    {
                        Console.WriteLine(kido.Get_Name() + " " + kido.Get_Last_Name() + "\n");
                    }

                }
                if (Fully_Registered_Kids.Count == 0)
                {
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("There's no fully registred kids");
                    Console.WriteLine("*****************************************************************************");
                }
                else
                {
                    Console.WriteLine("*****************************************************************************");
                    Console.WriteLine("Fully registered kids: ");
                    Console.WriteLine("*****************************************************************************");
                    foreach (var kido in Fully_Registered_Kids)
                    {
                        Console.WriteLine(kido.Get_Name() + " " + kido.Get_Last_Name() + " -->  " + kido.Get_Present() + "\n");
                    }
                }
            }
        }
        enum User_Input
        {   
            Register = 2,
            Login = 1,


            Exit = 0,
            Add_New_Child = 1,
            Add_New_Gift = 2,
            Add_Spec_Gift_To_Kid = 3,
            Add_Spec_Gift_To_Rand_Kid = 4,
            Rand_Gift_To_Rand_Kid = 5,
            Lazy_Mode = 6,
            Display_All_Kids = 7,
            Display_Gift_List = 8,
            Display_Unassignes = 9,
            Change_Status = 10,
            Display_User_Info = 11
        }
    }

