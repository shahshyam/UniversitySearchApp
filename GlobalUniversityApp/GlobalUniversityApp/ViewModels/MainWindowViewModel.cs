using GlobalUniversityApp.Client;
using GlobalUniversityApp.Commands;
using GlobalUniversityApp.Helpers;
using GlobalUniversityApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace GlobalUniversityApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUniversityClient _universityClient;
        public ObservableCollection<University> Universities { get; set; }
        private readonly TypeAssistant _typeAssistant;
        private static object _lock = new object();
        public MainWindowViewModel(IUniversityClient universityClient, TypeAssistant typeAssistant)
        {
            _universityClient = universityClient;
            _typeAssistant = typeAssistant;
            _typeAssistant.Idled += OntypeAssistantIdled;
            Universities = new ObservableCollection<University>();
            BindingOperations.EnableCollectionSynchronization(Universities, _lock);
        }

        private async void OntypeAssistantIdled(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SearchText) || !_allowToSearch)
            {
                _allowToSearch = true;
                return;
            }                
            SearchMessage = String.Empty;
            HasSearchResult = true;
            HasContent = false;            
            var results = await _universityClient.GetUniversity(SearchText, CountryName);           
            Universities.Clear();
            if (results != null && results.Count > 0)
            {
                HasContent = true;
                foreach (var result in results)
                {
                    Universities.Add(new University()
                    {
                        Name = result.Name
                    });
                }
                HasNoContent = false;
            }
            else
            {
                SearchMessage = "University name is not found";
                HasNoContent = true;
            }            
        }

        /// <summary>
        /// Load list of Country in combobox
        /// </summary>
        public List<string> Countries
        {
            get
            {
                RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
                List<string> countryNames = new List<string>();
                foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures)) {

                    country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                    countryNames.Add(country.DisplayName.ToString());
                }
                countryNames = countryNames.OrderBy(names => names).Distinct().ToList();
                return countryNames;
            }
        }

        /// <summary>
        /// Text entered in texbox for university search
        /// </summary>
        /// 
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set {
                _searchText = value;
                HasSearchResult = false;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        /// <summary>
        /// Selected Country from Dropdown
        /// </summary>
        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        /// <summary>
        /// HasSearchResult is used for displaying suggestion
        /// </summary>
        private bool _hasSearchResult;
        public bool HasSearchResult
        {
            get => _hasSearchResult;
            set
            {
                _hasSearchResult = value;
                OnPropertyChanged(nameof(HasSearchResult));
            }
        }

        private University _selectedUniversity;
        public University SelectedUniversity
        {
            get { return _selectedUniversity; }
            set
            {
                _selectedUniversity = value;
                OnPropertyChanged(nameof(SelectedUniversity));
            }
        }
        private bool _allowToSearch = true;
        private ICommand searchUniversityCommand;
        public ICommand SearchUniversityCommand
        {
            get
            {
                return searchUniversityCommand ?? (searchUniversityCommand = new RelayCommand( (x) =>
                {
                    _typeAssistant.TextChanged();                  
                }
                , (y) =>
                {
                    return !string.IsNullOrEmpty(SearchText);
                }
                ));
            }
        }

        private ICommand selectedUniversityCommand;
        public ICommand SelectedUniversityCommand
        {
            get
            {
                return selectedUniversityCommand ?? (selectedUniversityCommand = new RelayCommand((x) =>
                {
                    if (SelectedUniversity != null)
                    {
                        _allowToSearch = false;
                        SearchText = SelectedUniversity.Name;
                        HasSearchResult = false;
                    }
                }));
            }
        }

        /// <summary>
        /// Dispay search result message
        /// </summary>
        private string _searchMessage = String.Empty;
        public string SearchMessage
        {
            get => _searchMessage;
            set
            {
                _searchMessage = value;
                OnPropertyChanged(nameof(SearchMessage));
            }
        }

        /// <summary>
        /// Display list of search result
        /// </summary>
        private bool _hasContent = false;
        public bool HasContent
        {
            get => _hasContent;
            set
            {
                _hasContent = value;
                OnPropertyChanged(nameof(HasContent));
            }
        }

        /// <summary>
        /// Flag is used for displaying not found result
        /// </summary>
        private bool _hasNoContent = true;
        public bool HasNoContent
        {
            get => _hasNoContent;
            set { _hasNoContent = value; OnPropertyChanged(nameof(HasNoContent)); }
        }

    }
}
