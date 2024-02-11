using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace BITF21M025_Assignment1
{
    class Voter
    {
        private string voterName;
        private string cnic;
        private string selectedPartyName;
        public string Cnic
        {
            get { return cnic; }
            set { cnic = value; }
        }
        public string VoterName
        {
            get { return voterName; }
            set { voterName = value; }
        }

        public Voter(string VoterName, string cnic,string selectedPartyName="")
        {
            this.voterName = VoterName;
            this.cnic = cnic;
            this.selectedPartyName = selectedPartyName;
        }
        public string SelectedPartyName
        {
            get { return selectedPartyName; }
            set { selectedPartyName = value; }
        }

        public bool HasVoted()
        {
            if (!string.IsNullOrEmpty(selectedPartyName))
            {
                return true;
            }
            return false;
        }
    }
}
