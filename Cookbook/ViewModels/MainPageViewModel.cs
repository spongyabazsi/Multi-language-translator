using Cookbook.Models;
using Cookbook.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Cookbook.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        #region attributes

        LanguageService service = new LanguageService();
        //reading Yandex's languages into this
        public ObservableCollection<Language> langs { get; set; } =
                             new ObservableCollection<Language>();
        //reading Thesaurus's languages into this
        public ObservableCollection<string> langsthesa { get; set; } =
                             new ObservableCollection<string>();
        //splitting Yandex's languages pairs (left side to char '-')
        public ObservableCollection<string> fromlangsorigi { get; set; } =
                     new ObservableCollection<string>();
        //splitting Yandex's languages pairs (right side to char '-')
        public ObservableCollection<string> tolangs { get; set; } =
                     new ObservableCollection<string>();
        //after each successful translation we append history with the newest query
        public ObservableCollection<string> history { get; set; } =
                     new ObservableCollection<string>();
        public ObservableCollection<string> fromlangs { get;
            set; } =
                             new ObservableCollection<string>();
        //the programs functions (translation and synonyms
        public ObservableCollection<string> funcs { get; set; } =
                             new ObservableCollection<string>();
        //Selected item from Input language combobox
        private string _selecteditem;
        public string SelectedItem
        {
            get { return _selecteditem; }
            set {
                Set(ref _selecteditem, value);
                SelectedChanged();
            }
        }
        //Selected item from output language combobox
        private string _selecteditemto;
        public string SelectedItemto
        {
            get { return _selecteditemto; }
            set { Set(ref _selecteditemto, value); }
        }

        //selected item from function combobox
        private string _selecteditemfuncs;
        public string SelectedItemfuncs
        {
            get { return _selecteditemfuncs; }
            set { Set(ref _selecteditemfuncs, value); ReplaceFromCombo();
         
               }
        }
        //user's expression to translate / get synonym
        private string _expression;
        public string Expression
        {
            get { return _expression; }
            set { Set(ref _expression, value); }
        }
        //result of query
        private string _result;
        public string Result
        {
            get { return _result; }
            set { Set(ref _result, value); }
        }
        #endregion

        /// <summary>
        /// Loading in values statically and dynamically through LanguageService.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(
            object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //langGroups is a string, because couldn't convert the json formatted string into the correct type, so it had to be done manually.
            var langGroups = await service.GetLanguagesAsync(); 
            foreach (var item in langGroups)
            {
                langs.Add(item);
                string from = item.langs.Split('-')[0];
                if (fromlangsorigi.Contains(from) == false)
                {
                    fromlangsorigi.Add(from);
                }

            }
            //modes of the program
            funcs.Add("translate");
            funcs.Add("synonym");
            //Thesaurus does not have an API call for requesting available languages, so had to be done statically.
            string thesalangs = "cs_CZ, da_DK, de_CH, de_DE, en_US, el_GR, es_ES, fr_FR, hu_HU, it_IT, no_NO, pl_PL, pt_PT, ro_RO, ru_RU, sk_SK";
            string[] langos = thesalangs.Split(',');
            for (int i = 0; i < langos.Length; i++)
            {
                langos[i] = langos[i].Replace(" ", "");
                langsthesa.Add(langos[i]);
            }
            await base.OnNavigatedToAsync(parameter, mode, state);
        }

        
        /// <summary> Changes the value of the output language list depending on the choice of input language </summary>
        public void SelectedChanged()
        {
            tolangs.Clear();
            if (SelectedItemfuncs == "translate")
            {
                for (int i = 0; i < langs.Count; i++)
                {
                    string[] langpair = langs[i].langs.Split('-');
                    if (langpair[0].Equals(SelectedItem))
                    {
                        tolangs.Add(langpair[1]);
                    }
                }
            }
            else
            {
                SelectedItemto = SelectedItem;
            }
        }
        /// <summary>
        /// Translate command for translation and getting synonyms.
        /// </summary>
        public DelegateCommand TranslateCommand { get; }
        /// <summary>
        /// Speech command to read out loud the result of query.
        /// </summary>
        public DelegateCommand SpeechCommand { get; }
        public DelegateCommand ClearCommand { get; }
        /// <summary>
        /// Calling LanguageService depending on what mode we are using (translation or synonym).
        /// </summary>
        private async void TranslateAsync()
        {
            LanguageService languageService = new LanguageService();

            if (SelectedItemfuncs == "translate")
            {
                Translate resu = await service.GetTranslation(Expression, SelectedItem + '-' + SelectedItemto);
                if (resu == null) Result = "No query result";
                else Result = resu.GetTranslations();

            }
            else if (SelectedItemfuncs == "synonym")
            {

                WordDetails wd = await service.GetSynonym(Expression, SelectedItem);
                Result = wd.ToString();

            }
            else Result = "Choose a Magic Mode!";
            if (Result != "No query result" && Result != "Choose a Magic Mode!")
            {
                string[] add = Result.Split(',');
                
                history.Insert(0,SelectedItemfuncs + ": " + Expression + " to " + add[0]);

            }
        }
        /// <summary>
        /// Calling LanguageService's readoutloud method for audio.
        /// </summary>
        private async void SpeakAsync()
        {
            SpeechService service = new SpeechService();
            await service.readOutLoud(Result);
        }
        /// <summary>
        /// Fill the value of comboboxes depending on your mode (translation or synonym).
        /// </summary>
        
        
        private void Clear()
        {
            history.Clear();
        }
        private void ReplaceFromCombo()
        {
            fromlangs.Clear();
            if (SelectedItemfuncs == "translate")
            {
                for (int i = 0; i < fromlangsorigi.Count; i++)
                {
                    fromlangs.Add(fromlangsorigi[i]);
                }
            }
            else {
                for (int i = 0; i < langsthesa.Count; i++)
                {
                    fromlangs.Add(langsthesa[i]);
                }
            }
            SelectedItemto = "";
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPageViewModel()
        {
            TranslateCommand = new DelegateCommand(TranslateAsync);
            SpeechCommand = new DelegateCommand(SpeakAsync);
            ClearCommand = new DelegateCommand(Clear);
        }
    }
}
