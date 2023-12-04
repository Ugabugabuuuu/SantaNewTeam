using System;
using System.Collections.Generic;
using System.Text;

    class Kid_Info
    {
        private string name;
        private string last_Name;
        private string Present;
        private bool fullyAssigned;
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
        // Status to indetify if kid has present or not. haha
        public void Set_Status(bool a)
        {
            fullyAssigned = a;
        }
        public bool Is_Assigned()
        {
            return fullyAssigned;
        }
    }