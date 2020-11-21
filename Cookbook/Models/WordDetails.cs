using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Models
{

    ///<summary>Model class for getting synonyms from Thesaurus.</summary>
    public class WordDetails
    {
        public List<Response> response { get; set; }
        /// <summary>
        /// Overriding ToString to obtain synonym values.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string res = "";
            if (response == null) return "No query result";
            else
            {
                for (int i = 0; i < response.Count; i++)
                {
                    res += response[i].ToString() + "\n";
                }
                return res;
            }
        }
    }

    public class Response
    {
        public List list { get; set; }

        public override string ToString() {
            string res = "";
            res += list.synonyms + " (" + list.category + ")";
            return res.Replace("|", ", ");
            
        }
    }

    public class List
    {
        public string category { get; set; }
        public string synonyms { get; set; }
    }

}
