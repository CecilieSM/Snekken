using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models;
public class Person
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


        public Person(string name, string email, string phone = "ikke oplyst")
    {
        this.Name = name;
        this.Email = email;
        this.Phone = phone;
    }
}
