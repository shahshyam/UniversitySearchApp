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
        private string _searchText;

        public List<string> Countries
        {
            get
            {
                RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
                List<string> countryNames = new List<string>();                
                foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))                {

                    country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                    countryNames.Add(country.DisplayName.ToString());
                }
                countryNames= countryNames.OrderBy(names => names).Distinct().ToList();
                return countryNames;
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        private ICommand searchUniversityCommand;
        public ICommand SearchUniversityCommand
        {
            get
            {
                return searchUniversityCommand ?? (searchUniversityCommand = new RelayCommand(async(x) =>
                {
                    var results = await _universityClient.GetUniversity(SearchText, CountryName);
                    Universities.Clear();
                    foreach(var result in results)
                    {
                        Universities.Add(new University()
                        {
                            Name = result.Name
                        });
                    }
                }
                , (y) =>
                {
                    return !string.IsNullOrEmpty(SearchText);
                }
                ));
            }
        }

    }
}
