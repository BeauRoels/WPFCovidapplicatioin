using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCovidApp
{
    class Person
    {
        // Properties
        public long IdNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public string[] Address { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberFamilyMembers { get; set; }
        public string TypeContact { get; set; }
        public bool AlreadyContacted { get; set; }
        // Constructor
        public Person(long idNo, string fN, string lN, DateTime bd, string gender, string[] address, long phone, string mail, int noFamily, 
            string typeContact, bool contacted)
        {
            IdNo = idNo;
            FirstName = fN;
            LastName = lN;
            Birthday = bd;
            Gender = gender;
            Address = address;
            PhoneNumber = phone;
            Email = mail;
            NumberFamilyMembers = noFamily;
            TypeContact = typeContact;
            AlreadyContacted = contacted;
        }
        // Geef de persoon weer
        public string DisplayPerson()
        {
            return $"{IdNo} {FirstName} {LastName} {Birthday:dd/MM/yyyy} {Gender} {Address[0]} {Address[1]} {Address[2]} {Address[3]} {PhoneNumber} {Email} {NumberFamilyMembers} " +
                $"{TypeContact} " + (AlreadyContacted ? "ja" : "nee");
        }
    }
}
