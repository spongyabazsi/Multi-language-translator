using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    /// <summary>
    /// Model class for getting translations using Yandex Dictionary API.
    /// </summary>
    public class Translate
    {
        public Head head { get; set; }
        public List<Def> def { get; set; }

        /// <summary>
        /// Getting a string with all possible translation of queried word.
        /// </summary>
        /// <returns></returns>
        public string GetTranslations()
        {
            List<Def> l = this.def;
            string result = "";
            if (this == null) return "No query result";
            else
            {
                for (int i = 0; i < this.def.Count; i++)
                {
                    for (int j = 0; j < this.def[i].tr.Count; j++)
                    {
                        result += this.def[i].tr[j].text + ", ";
                    }
                }
                if (result != "")
                    result = result.Remove(result.Length - 2);
                return result;
            }
        }
    }

    public class Head
    {
    }

    public class Def
    {
        public string text { get; set; }
        public string pos { get; set; }
        public List<Tr> tr { get; set; }
    }

    public class Tr
    {
        public string text { get; set; }
        public string pos { get; set; }
        public List<Syn> syn { get; set; }
        public List<Mean> mean { get; set; }
        public List<Ex> ex { get; set; }
    }

    public class Syn
    {
        public string text { get; set; }
    }

    public class Mean
    {
        public string text { get; set; }
    }

    public class Ex
    {
        public string text { get; set; }
        public List<Tr1> tr1{ get; set; }
    }

    public class Tr1
    {
        public string text { get; set; }
    }

}
