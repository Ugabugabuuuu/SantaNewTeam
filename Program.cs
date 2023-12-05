using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace SantaClause
{
    class Program
    {
        static Database database;

        static public Kid_Controller kid_controller;
        static public GiftController gift_controller;
        static List<User> users = new List<User>();
        static  public List<History> history = new List<History>();

        static private bool authorized = false;

        static void Main(string[] args)
        {
            database = new Database();
            kid_controller = new Kid_Controller(database);
            gift_controller = new GiftController(database);

            Gift gift = new Gift();
            Kid_Info kid = new Kid_Info();

            users = database.Read_Users();

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
                {
                    History h  = new History(User_Input.Exit.ToString(), DateTime.Now);
                    database.Write_History(h);
                    Environment.Exit(0);
                }
                if (input_key == (int)User_Input.Add_New_Child)
                {
                    kid_controller.Add_New_Kid_To_List(kid);
                    History h = new History(User_Input.Add_New_Child.ToString(), DateTime.Now);
                    database.Write_History(h);
                    kid = null;
                }
                else if (input_key == (int)User_Input.Add_New_Gift)
                {
                    gift_controller.Add_Gifts_To_List(gift, exists, users);
                    History h = new History(User_Input.Add_New_Gift.ToString(), DateTime.Now);
                    database.Write_History(h);
                    gift = null;
                }
                else if (input_key == (int)User_Input.Add_Spec_Gift_To_Kid)
                {
                    Add_Gift_To_Specific_Child(gift, exists);
                    gift = null;

                    History h = new History(User_Input.Add_Spec_Gift_To_Kid.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Add_Spec_Gift_To_Rand_Kid)
                {
                    Add_Specific_Gift_To_Rand_Child(gift, exists);
                    gift = null;

                    History h = new History(User_Input.Add_Spec_Gift_To_Rand_Kid.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Rand_Gift_To_Rand_Kid)
                {

                    History h = new History(User_Input.Rand_Gift_To_Rand_Kid.ToString(), DateTime.Now);
                    database.Write_History(h);

                    Add_Rand_Gift_To_Rand_Child(exists);
                }
                else if (input_key == (int)User_Input.Lazy_Mode)
                {
                    History h = new History(User_Input.Lazy_Mode.ToString(), DateTime.Now);
                    database.Write_History(h);

                    Lazy_Mode();
                }
                else if (input_key == (int)User_Input.Display_All_Kids)
                {
                    Display_All_Kids(kid_controller.Get_Kids(KidsOptions.NotRegistered));

                    History h = new History(User_Input.Display_All_Kids.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Display_Gift_List)
                {
                    Display_All_Gifts(gift_controller.Gifts);

                    History h = new History(User_Input.Display_Gift_List.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Display_Unassignes)
                {
                    int amount = 0;
                    Console.WriteLine(" Number of assigned children: " + kid_controller.Get_Kids(KidsOptions.Registered).Count);
                    Console.WriteLine(" Number of unassigned children: " + kid_controller.Get_Kids(KidsOptions.NotRegistered).Count);

                    foreach (Gift tmp_gift in gift_controller.Gifts)
                        if (tmp_gift.Get_Amount() == 0)
                            amount++;
                    Console.WriteLine(" Number of unassigned gifts: " + amount);

                    History h = new History(User_Input.Display_Unassignes.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Change_Status)
                {
                    Console.WriteLine("kokiai dovanai reikia pakeisti statusa");

                    String input = Console.ReadLine().ToString();
                    Console.WriteLine("pakeisti i statusa: ");

                    foreach (Gift i in gift_controller.Gifts)
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
                    History h = new History(User_Input.Change_Status.ToString(), DateTime.Now);
                    database.Write_History(h);
                }
                else if (input_key == (int)User_Input.Display_User_Info)
                {
                    History h = new History(User_Input.Display_User_Info.ToString(), DateTime.Now);
                    database.Write_History(h);

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
                else if (input_key == (int)User_Input.Update_User_Info)
                {
                    History h = new History(User_Input.Update_User_Info.ToString(), DateTime.Now);
                    database.Write_History(h);

                    Console.WriteLine("Input the user's username: ");
                    string username = Console.ReadLine();
                    bool found = false;
                    foreach (User user in users)
                    {
                        if (user.Get_Username() == username)
                        {
                            Console.WriteLine("Input new username: ");
                            string newUsername = Console.ReadLine();
                            Console.WriteLine("Input new password: ");
                            string newPassword = Console.ReadLine();
                            Console.WriteLine("Input new name: ");
                            string newName = Console.ReadLine();
                            Console.WriteLine("Input new surname: ");
                            string newSurname = Console.ReadLine();
                            user.Set_Username(newUsername);
                            user.Set_Password(newPassword);
                            user.Set_Name(newName);
                            user.Set_Surname(newSurname);

                            database.Write_All_Users_To_File(users);
                            found = true;
                            break;
                        }
                    }

                    if (!found) { Console.WriteLine("No such user found"); }
                }
                else if(input_key == (int)User_Input.Show_History)
                {
                    history = database.Read_History();
                    foreach (History h2 in history)
                    {
                        Console.WriteLine(h2.get_Executed() + "->" + h2.get_Command());
                    }
                }

                using (StreamWriter sw = File.CreateText(database.path_gift))
                {
                    foreach (Gift gifts_tmp in gift_controller.Gifts)
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
            {
                History h = new History(User_Input.Exit.ToString(), DateTime.Now);
                database.Write_History(h);
                Environment.Exit(0);
            }
              
            if (input_key == (int)User_Input.Login)
            {
                History h = new History("Login", DateTime.Now);
                database.Write_History(h);
                LoginPage();
            }
            else if (input_key == (int)User_Input.Register)
            {
                History h = new History("Register", DateTime.Now);
                database.Write_History(h);
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
                string[] lines = File.ReadAllLines(database.path_users);

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
                database.Read_Users();
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

        static void Write_All_Users_To_File(List<User> pUsers, string Path)
        {
            using (StreamWriter sw = File.CreateText(Path))
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
        static void WriteUserToFile(User user)
        {
            // Append the new user information to the user_info.txt file
            string userInfo = $"{user.Get_Name()};{user.Get_Surname()};{user.Get_Username()};{user.Get_Password()}";
            File.AppendAllLines(database.path_users, new[] { userInfo });
        }

        static public bool IsUsernameTaken(string username)
        {
            try
            {
                string[] lines = File.ReadAllLines(database.path_users);

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
            Console.Write("**             3           | Set specific gift to wanted child           ** \n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             4           | Set specific gift to random child            **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             5           | Set random gift to random child              **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             6           | Activate  Lazy mode                          **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             7           | Display all kids(registred/unregistred)      **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             8           | Display gift list                            **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             9           | Display Number of unassigned children/gifts  **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             10          | Change status                                **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             11          | Display user info                            **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             12          | Update user info                             **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**             15          | Show history                                 **\n");
            Console.Write("****************************************************************************\n");
            Console.Write("**                TO EXIT PROGRAM TYPE 0 AND PRESS ENTER                  **\n");
            Console.Write("****************************************************************************\n");
        }


        static void Display_User_Info(User user)
        {
            Console.WriteLine(user.Get_Name() + " " + user.Get_Surname() + ":");
            Console.WriteLine("Username: " + user.Get_Username());
            Console.WriteLine("Password: *********");
        }




        static void Display_All_Gifts(List<Gift> gifts)
        {
            if (gifts.Count == 0)
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
        static void Display_All_Kids(List<Kid_Info> kids)
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
            if (kid_controller.Get_Kids(KidsOptions.Registered).Count == 0)
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
                foreach (var kido in kid_controller.Get_Kids(KidsOptions.Registered))
                {
                    Console.WriteLine(kido.Get_Name() + " " + kido.Get_Last_Name() + " -->  " + kido.Get_Present() + "\n");
                }
            }
        }

        static void Find_And_Substract_Gift(Kid_Info k, ref List<Gift> Gifts)
        {
            if (k.Is_Assigned())
            {
                foreach (Gift temp_gift in Gifts)
                {
                    if (temp_gift.Get_Gift() == k.Get_Present())
                        temp_gift.Set_Amount(temp_gift.Get_Amount() - 1);
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

            foreach (Kid_Info kido in kid_controller.Get_Kids(KidsOptions.NotRegistered))
            {
                if (kido.Get_Name().Equals(name) && kido.Get_Last_Name().Equals(last_name))
                {
                    Find_And_Substract_Gift(kido, ref gift_controller.Gifts);

                    kido.Set_Present(present);
                    kido.Set_Status(true);
                    found = true;
                    break;
                }
            }
            if (found)
            {
                gift_controller.Find_Gift(present, ref exists);

                if (!exists)
                {
                    gift.Set_Gift(present);
                    gift.Set_Amount(1);
                    gift_controller.Gifts.Add(gift);
                }
            }

            kid_controller.Register_Assigned_Kids();

            database.Write_Kids_To_File(kid_controller.Get_Kids(KidsOptions.NotRegistered), KidsOptions.NotRegistered);
        }
        static void Add_Specific_Gift_To_Rand_Child(Gift gift, bool exists)
        {
            Random rd = new Random();
            int random_index = rd.Next(0, kid_controller.Get_Kids(KidsOptions.NotRegistered).Count);
            Console.WriteLine("What gift you want to give for random child? \n");
            if (gift == null)
                gift = new Gift();
            gift.Set_Gift(Console.ReadLine());

            gift_controller.Find_Gift(gift.Get_Gift(), ref exists);

            Find_And_Substract_Gift(kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_index], ref gift_controller.Gifts);

            if (!exists)
            {
                gift.Set_Amount(1);
                gift_controller.Gifts.Add(gift);
            }

            kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_index].Set_Present(gift.Get_Gift());
            kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_index].Set_Status(true);

            Console.WriteLine("Present has been asigned ...\n");

            kid_controller.Register_Assigned_Kids();
            database.Write_Kids_To_File(kid_controller.Get_Kids(KidsOptions.NotRegistered), KidsOptions.NotRegistered);
        }
        static void Add_Rand_Gift_To_Rand_Child(bool exists)
        {
            if (kid_controller.Get_Kids(KidsOptions.NotRegistered).Count > 0 && gift_controller.Gifts.Count > 0)
            {
                Random rd = new Random();
                int random_child = rd.Next(0, kid_controller.Get_Kids(KidsOptions.NotRegistered).Count);
                int random_gift = rd.Next(0, gift_controller.Gifts.Count);

                if (kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Get_Present() != gift_controller.Gifts[random_gift].Get_Gift())
                {

                    foreach (Gift gg in gift_controller.Gifts)
                    {
                        if (gg.Get_Gift() == kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Get_Present())
                        {
                            gg.Set_Amount(gg.Get_Amount() - 1);
                            break;
                        }
                    }
                    kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Set_Present(gift_controller.Gifts[random_gift].Get_Gift());
                    kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Set_Status(true);

                    gift_controller.Find_Gift(gift_controller.Gifts[random_gift].Get_Gift(), ref exists);
                }
                else
                {
                    kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Set_Present(gift_controller.Gifts[random_gift].Get_Gift());
                    kid_controller.Get_Kids(KidsOptions.NotRegistered)[random_child].Set_Status(true);
                }

                kid_controller.Register_Assigned_Kids();
                database.Write_Kids_To_File(kid_controller.Get_Kids(KidsOptions.NotRegistered), KidsOptions.NotRegistered);
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
                foreach (Kid_Info k in kid_controller.Get_Kids(KidsOptions.NotRegistered))
                {
                    if (index == gift_controller.Gifts.Count)
                        index = 0;

                    Find_And_Substract_Gift(k, ref gift_controller.Gifts);

                    k.Set_Present(gift_controller.Gifts[index].Get_Gift());
                    gift_controller.Gifts[index].Set_Amount(gift_controller.Gifts[index].Get_Amount() + 1);
                    k.Set_Status(true);

                    index++;
                }
                kid_controller.Register_Assigned_Kids();
                database.Write_Kids_To_File(kid_controller.Get_Kids(KidsOptions.NotRegistered), KidsOptions.NotRegistered);
            }
            else
            {
                return;
            }
        }
        void Add_To_History (String str)
        {
        }

    }

}
