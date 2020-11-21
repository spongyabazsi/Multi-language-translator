using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Models
{

    /// <summary>
    /// Model class for getting available languages to translate into using Yandex Dictionary API.
    /// </summary>
    public class Language
    {
        public string langs { get; set; }

        public override string ToString()
        {
            return langs;
        }
    }

}
