using System;
using System.Collections.Generic;
using System.Text;

class GiftController
{
    Database database;
    public List<Gift> Gifts;

    public GiftController(Database database) {
        this.database = database;

        Gifts = database.Read_Gifts_From_File();
    }

    public void Add_Gifts_To_List(Gift gift, bool exists, List<User> users)
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

    public void FindUser(List<User> users, string name, Gift gift)
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

    public void Find_Gift(string gift, ref bool exists)
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
}