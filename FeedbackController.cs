using System;
using System.Collections.Generic;
using System.Text;


class FeedbackController
{
    Database database;
    static List<KeyValuePair<bool, string>> feedbacks;

    public FeedbackController(Database database)
    {
        this.database = database;

        feedbacks = database.Read_Feedback_From_File();
    }
    int Get_Logins_From_Month(DateTime dateTime)
    {
        List<History> history = database.Read_History();
        int result = 0;
        foreach (History historyItem in history)
        {
            if (historyItem.get_Executed().Month == dateTime.Month && historyItem.get_Command() == "Login")
            {
                result++;
            }
        }

        return result;
    }

    int Get_Registers_From_Month(DateTime dateTime)
    {
        List<History> history = database.Read_History();
        int result = 0;
        foreach (History historyItem in history)
        {
            if (historyItem.get_Executed().Month == dateTime.Month && historyItem.get_Command() == "Register")
            {
                result++;
            }
        }

        return result;
    }

    int Get_Actions_This_Month(User_Input action)
    {
        List<History> history = database.Read_History();
        int result = 0;
        foreach (History historyItem in history)
        {
            if (historyItem.get_Executed().Month == DateTime.Today.Month && historyItem.get_Command() == action.ToString())
            {
                result++;
            }
        }

        return result;
    }

    public void Add_Feedback(KeyValuePair<bool, string> feedback)
    {
        feedbacks.Add(feedback);
        database.Write_Feedback_To_File(feedbacks);
    }

    public string Generate_Report_From_Month(DateTime dateTime)
    {
        Console.WriteLine();
        string report =
        "*********************************************************\n" +
        "Month " + dateTime.Month + " report for system's usage\n" +
        "*********************************************************\n" +
        "Positive feedbacks: " + Get_Positive_Percentage() + " %\n" +
        "Negative feedbacks: " + (100 - Get_Positive_Percentage()) + " %\n" +
        "*********************************************************\n" +
        "New accounts: " + Get_Registers_From_Month(dateTime) + "\n" +
        "Logins: " + Get_Logins_From_Month(dateTime) + "\n" +
        "*********************************************************\n";

        Array arr = Enum.GetValues(typeof(User_Input));
        foreach (int i in Enum.GetValues(typeof(User_Input)))
        {
            report += Enum.GetName(typeof(User_Input), i) + ": " + Get_Actions_This_Month((User_Input)arr.GetValue(i)) + "\n";
        }

        return report;
    }

    static int Get_Positive_Percentage()
    {
        float positives = 0;
        foreach (KeyValuePair<bool, string> feedback in feedbacks)
        {
            if (feedback.Key) positives++;
        }

        return (int)((positives / (float)feedbacks.Count) * 100f);
    }
}

