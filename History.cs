using System;
using System.Collections.Generic;
using System.Text;

class History
{
    String command;
    DateTime executed;
    public History(String cmd, DateTime dateTime)
    {
        this.command = cmd;
        this.executed = dateTime;
    }
    public String get_Command()
    {
        return command;
    }
    public DateTime get_Executed()
    {
        return executed;
    }
    public void set_Executed(DateTime dateTime) 
    {  
        executed = dateTime;
    }
    public void set_Command(String cmd)
    { 
        cmd = command;
    }
}
