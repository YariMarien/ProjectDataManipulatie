using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataManipulatie_DAL
{
    public abstract class BasisKlasse : IDataErrorInfo
    {

        public abstract string this[string columnName] { get; }

        public bool IsGeldig()
        {
            return string.IsNullOrWhiteSpace(Error);

        }
        public string Error
        {
            get
            {
                string foutmeldingen = "";

                foreach (var property in this.GetType().GetProperties()) 
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        string fout = this[property.Name];
                        if (!string.IsNullOrWhiteSpace(fout))
                        {
                            foutmeldingen += fout + Environment.NewLine;
                        }
                    }
                }
                return foutmeldingen;
            }
        }

    }
}
