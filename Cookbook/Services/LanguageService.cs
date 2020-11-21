using Cookbook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace Cookbook.Services
{
    /// <summary>
    /// Service class for network calls.
    /// </summary>
    class LanguageService
    {
        private readonly string apikey = "dict.1.1.20200505T110543Z.b9fabeb7eb7fc41c.d5a0850505ea646a3fa3a0bd582ef36071a7f7fa";
        private readonly string url = "https://dictionary.yandex.net/api/v1/dicservice.json/getLangs?key=";

        /// <summary>
        /// Function to get string from URL in json format.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }

        }
        /// <summary>
        /// Function to get a list of Languages calling getAsync method.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Language>> GetLanguagesAsync()
        {
            //JsonConvert couldn't convert it into languages, so had to be done manually
            string st = await GetAsync(url + apikey);
            String[] lingo = st.Split(',');
            List<Language> lang = new List<Language>();
            for (int i = 0; i < lingo.Length; i++)
            {
                lingo[i] = lingo[i].Trim('[');
                lingo[i] = lingo[i].Trim(']');
                lingo[i] = lingo[i].Trim('"');
                Language l = new Language();
                l.langs = lingo[i];
                lang.Add(l);
            }
            return lang;
        }
        /// <summary>
        /// Getting translation for given expression in given language. Using Yandex Dictionary API.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="lango"></param>
        /// <returns>Translated word or null if translation not exist</returns>
        public async Task<Translate> GetTranslation(string expression, string lango)
        {
            string url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key=" + apikey + "&lang=" + lango + "&" + "text=" + expression;
            string json = await GetAsync(url);
            Translate word = JsonConvert.DeserializeObject<Translate>(json);
            if (word.def == null || word.def.Count == 0) return null;
            else return word;
        }


        /// <summary>
        /// Getting synonyms of given expression in the chosen language. Using Thesaurus API.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="lango"></param>
        /// <returns></returns>
        public async Task<WordDetails> GetSynonym(string expression, string lango)
        {
            string api = "88J1LrO1Af77ov1huMfQ";
            string url = "http://thesaurus.altervista.org/thesaurus/v1?word="+expression+"&language="+lango+"&key="+api+"&output=json";
            string json = await GetAsync(url);
            WordDetails word = JsonConvert.DeserializeObject<WordDetails>(json);
            return word;
        }
    }
}
