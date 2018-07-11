using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Grid
{
    class AlphaNumeric : NotifyPropertyChangedImpl, IComparable<AlphaNumeric>
    {
        private readonly string defaultValue = "0";

        private string content;
        public AlphaNumeric()
        {
            this.content = "0";
        }
        
        public AlphaNumeric(AlphaNumeric a)
        {
            if (a == null) this.content = this.defaultValue;
            else if (a.content == null) this.content = this.defaultValue;
            else this.Content = a.content;
        }

        public AlphaNumeric(string s)
        {
            if (s == null) this.content = this.defaultValue;
            else this.Content = s;
        }

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;

                // Remove all non-alphanumeric characters
                Regex notAlphaNumeric = new Regex("[^a-zA-Z0-9 -]");
                content = notAlphaNumeric.Replace(content, "");

                OnPropertyChanged("Content");
            }
        }

        // *** String properties ***
        public override bool Equals(Object other)
        {
            return (other is AlphaNumeric) && ((AlphaNumeric)other).content.Equals(this.content);
        }

        public override int GetHashCode()
        {
            return this.content.GetHashCode();
        }

        public override string ToString()
        {
            return this.content;
        }

        public static AlphaNumeric Parse(string s)
        {
            return new AlphaNumeric(s);
        }

        public int CompareTo(AlphaNumeric other)
        {
            return this.content.CompareTo(other.content);
        }
    }
}
