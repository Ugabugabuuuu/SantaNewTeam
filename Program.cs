using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
namespace SantaClause
{
    class Program
    {
        static private List<Kid_Info> kids = new List<Kid_Info>();
        static private List<Gift> Gifts = new List<Gift>();
        static private List<Kid_Info> Fully_Registered_Kids = new List<Kid_Info>();
        static private string path_data = "C:\\Users\\danie\\source\\repos\\SantaClauseTask_Danielius_Pliuskys\\data.txt";
        static private string path_data2 = "C:\\Users\\danie\\source\\repos\\SantaClauseTask_Danielius_Pliuskys\\Data2.txt";
        static private string path_gift = "C:\\Users\\danie\\source\\repos\\SantaClauseTask_Danielius_Pliuskys\\gifts.txt";
        static void Main(string[] args)
        {
            Gift gift = new Gift();
            Kid_Info kid = new Kid_Info();

            kids = Read_File(path_data);
            Fully_Registered_Kids = Read_File(path_data2);

            Read_Gifts(ref Gifts);

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
                    Add_Gifts_To_List(gift, exists);
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

                using (StreamWriter sw = File.CreateText(path_gift))
                {
                    foreach (Gift gifts_tmp in Gifts)
                    {
                        sw.WriteLine(gifts_tmp.Get_Gift());
                        sw.WriteLine(gifts_tmp.Get_Amount());
                    }
                }
                Console.WriteLine("press any key to continue...\n");
                Console.ReadKey();
                Console.Clear();
            }


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
            Console.Write("**                TO EXIT PROGRAM TYPE 0 AND PRESS ENTER                  **\n");
            Console.Write("****************************************************************************\n");
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
            if(Gifts.Count == 0)
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
                    Console.WriteLine(gift.Get_Gift() + " --> " + gift.Get_Amount() + "\n");
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
                    g.Set_Amount(Int32.Parse(line));
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
        static void Add_Gifts_To_List(Gift gift, bool exists)
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

            if (!exists)
            {
                gift.Set_Amount(0);
                Gifts.Add(gift);
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
            if(found)
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
        enum User_Input
        {
            Exit = 0,
            Add_New_Child = 1,
            Add_New_Gift = 2,
            Add_Spec_Gift_To_Kid = 3,
            Add_Spec_Gift_To_Rand_Kid = 4,
            Rand_Gift_To_Rand_Kid = 5,
            Lazy_Mode = 6,
            Display_All_Kids = 7,
            Display_Gift_List = 8,
            Display_Unassignes = 9
        }
    }

}
