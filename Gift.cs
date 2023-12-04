using System;
using System.Collections.Generic;
using System.Text;

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