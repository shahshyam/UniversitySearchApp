using GlobalUniversityApp.Client;
using GlobalUniversityApp.Commands;
using GlobalUniversityApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace GlobalUniversityApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUniversityClient _universityClient;
        public ObservableCollection<University> Universities { get; set; }
        public MainWindowViewModel(IUniversityClient universityClient)
        {
            _universityClient = universityClient;
            Universities = new ObservableCollection<University>();
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

        private ICommand searchUniversityCommand;
        public ICommand SearchUniversityCommand
        {
            get
            {
                return searchUniversityCommand ?? (searchUniversityCommand = new RelayCommand(async (x) =>
                {
                    SearchMessage=String.Empty;
                    HasSearchResult = true;
                    HasContent = false;
                    var results = await _universityClient.GetUniversity(SearchText, CountryName);
                    Universities.Clear();
                    if (results.Count > 0)
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
                        SearchText= SelectedUniversity.Name;
                        HasSearchResult = false;
                    }
                }));
            }
        }


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

        private bool _hasNoContent = true;
        public bool HasNoContent
        {
            get => _hasNoContent;
            set { _hasNoContent = value; OnPropertyChanged(nameof(HasNoContent)); }
        }

    }
}
