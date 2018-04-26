using Akavache;
using AkavacheExemplo.Models;
using AkavacheExemplo.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AkavacheExemplo
{
    public partial class MainPage : ContentPage
    {
        public bool IsLoading;

        private readonly ObservableRangeCollection<Game> _games = new ObservableRangeCollection<Game>();
        public ObservableCollection<Game> Games => _games;
        public NintendoService _NintendoService;

        private ICommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                    (refreshCommand = new Command(() => LoadGames(), () => !IsLoading));
            }
        }

        public MainPage()
        {
            InitializeComponent();

            _NintendoService = new NintendoService();

            BindingContext = this;

            RefreshCommand.Execute(null);
        }

        private void LoadGames()
        {
            IsLoading = true;

            GetGames(true);

            IsLoading = false;
        }

        public void GetGames(bool force = false)
        {
            var cache = BlobCache.LocalMachine;
            cache.GetAndFetchLatest("games", GetRemoteGamesAsync,
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    var invalidateCache = (force || elapsed > new TimeSpan(24, 0, 0));
                    return invalidateCache;
                })
                .Subscribe((games) =>
                {
                    _games.ReplaceRange(games);
                });
        }

        private async Task<IEnumerable<Game>> GetRemoteGamesAsync()
        {
            return await _NintendoService.GetGamesAsync(50);
        }
    }
}
