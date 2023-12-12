using CommunityToolkit.Mvvm.Input;
using DataLayer;
using SADA.Helpers;
using SADA.Infastructure.Core;
using SADA.Infastructure.Core.Enums;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace SADA.ViewModel.MainMenu.SalaryAndStaff.Staff
{
    public class StaffListViewModel : TabObservableObjectList<DataLayer.Staff>
    {
        #region Fields

        #region Services fields

        #endregion Services fields

        #region IEnumerables fields

        private CollectionWithSelection<StaffRole> _staffRoles;
        private CollectionWithSelection<StaffPost> _staffPosts;
        private CollectionWithSelection<User> _users;
        private CollectionWithSelection<CarDealership> _carDealerships;

        #endregion IEnumerables fields

        #region Selected fields

        #endregion Selected fields

        #region Other fields

        private SADAEntities _ctx;

        #endregion Other fields

        #region Filter fields

        private FilterMaker _filter;

        #endregion Filter fields

        #endregion Fields

        #region Constructor

        public StaffListViewModel(IDialogService dialogService, ITabService tabService):
            base(dialogService, tabService, TypeWrapper<TabObservableObjectForm<DataLayer.Staff>>.Make<StaffViewModel>())
        {

            EditTabName = (e) => $"Изменение сотрудника №{_selectedEntity.ID}";
            AddTabName = (e) => "Добавление сотрудника";

            _filter = new FilterMaker();
        }

        protected StaffListViewModel(): base(null, null, null)
        { }

        #endregion Constructor

        #region Properties

        public CollectionWithSelection<StaffRole> StaffRoles
        {
            get => _staffRoles;
            set => SetProperty(ref _staffRoles, value);
        }

        public CollectionWithSelection<StaffPost> StaffPosts
        {
            get => _staffPosts;
            set => SetProperty(ref _staffPosts, value);
        }
        public CollectionWithSelection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
        public CollectionWithSelection<CarDealership> CarDealerships
        {
            get => _carDealerships;
            set => SetProperty(ref _carDealerships, value);
        }


        #region Filter properties

        public FilterMaker Filter
        {
            get => _filter;
        }

        #endregion Filter properties

        #endregion Properties

        #region Commands

        #endregion Commands

        #region Command implementation

        protected override void _OnClose()
        {
            var result = _dialogService.ShowMessageBox("Вопрос", $"Закрыть вкладку {Name}?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _RaiseCloseEvent();
                _ctx?.Dispose();
            }
        }

        protected override void _SearchCommand()
        {
            // Базовый поиск по типу оплаты, контрагенту

            _currentQuery = _defaultQuery;

            if (_staffRoles.Selected != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.RoleID == _staffRoles.Selected.ID);
            }

            if (_staffPosts.Selected != null)
            {
                _currentQuery = _currentQuery
                    .Where(c => c.PostID == _staffPosts.Selected.ID);
            }

            try
            {
                Entities = new ObservableCollection<DataLayer.Staff>(
                _currentQuery
                .Where(_baseFilter)
                .Take(_dataCountPerPage)
                .ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessageBox("Ошибка", ex.Message, MessageBoxButton.OK);
            }
        }

        protected override void _SaveAsFileCommand()
        {
        }

        protected override void _ApplyFilterCommand()
        {
            try
            {
                _currentQuery = _defaultQuery.Where(_filter.MakeFilter());
                Entities = new ObservableCollection<DataLayer.Staff>(
                    _currentQuery.Take(_dataCountPerPage).ToList());
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        protected override void _ClearFilterCommand()
        {
            _filter.FilterFieldsClear();
        }

        protected override void LoadedInner()
        {
            try
            {
                _ctx = new SADAEntities();

                _baseFilter = c => c.IsDeleted == false;

                _defaultQuery = JoinBaseQuery(_ctx.Staff);

                Entities = new ObservableCollection<DataLayer.Staff>(
                    _defaultQuery
                    .Where(_baseFilter)
                    .Take(_dataCountPerPage).ToList());

                Users = new CollectionWithSelection<User>(_ctx.User
                    .AsNoTracking()
                    .ToList());

                CarDealerships = new CollectionWithSelection<CarDealership>(_ctx.CarDealership
                    .AsNoTracking()
                    .ToList());

                StaffRoles = new CollectionWithSelection<StaffRole>(_ctx.StaffRole
                    .AsNoTracking()
                    .ToList());

                StaffPosts = new CollectionWithSelection<StaffPost>(_ctx.StaffPost
                    .AsNoTracking()
                    .ToList());


                _filter.StaffPosts = StaffPosts.Clone();
                _filter.StaffRoles = StaffRoles.Clone();
                _filter.CarDealerships = CarDealerships.Clone();
                _filter.Users = Users.Clone();
            }
            catch (DbEntityValidationException ex)
            {
                DbEntityValidationExceptionHelper.ShowException(ex);
            }
        }

        #endregion Command implementation

        #region Other

        protected override IQueryable<DataLayer.Staff> JoinBaseQuery(IQueryable<DataLayer.Staff> query)
        {
            return query
                .Include(s => s.User)
                .Include(s => s.File)
                .Include(s => s.StaffRole)
                .Include(s => s.StaffPost)
                .Include(s => s.CarDealership)
                .Include(s => s.Passport)
                .AsNoTracking();
        }

        public sealed class FilterMaker : EntityFilterBase<DataLayer.Staff>
        {
            #region Filter private fields

            private CollectionWithSelection<StaffRole> _staffRoles;
            private CollectionWithSelection<StaffPost> _staffPosts;
            private CollectionWithSelection<User> _users;
            private CollectionWithSelection<CarDealership> _carDealerships;

            #endregion Filter private fields

            #region Filter properties

            public CollectionWithSelection<StaffRole> StaffRoles
            {
                get => _staffRoles;
                set => SetProperty(ref _staffRoles, value);
            }

            public CollectionWithSelection<StaffPost> StaffPosts
            {
                get => _staffPosts;
                set => SetProperty(ref _staffPosts, value);
            }
            public CollectionWithSelection<User> Users
            {
                get => _users;
                set => SetProperty(ref _users, value);
            }
            public CollectionWithSelection<CarDealership> CarDealerships
            {
                get => _carDealerships;
                set => SetProperty(ref _carDealerships, value);
            }

            public string FullName { get; set; }

            public bool ShowIsDeleted { get; set; } = false;

            #endregion Filter properties

            public override Expression<Func<DataLayer.Staff, bool>> MakeFilter()
            {
                var expression = defaultExpression;

                if(!string.IsNullOrEmpty(FullName))
                {
                    expression = expression
                        .And(s => (s.Passport.Surname + s.Passport.Name + s.Passport.Patronymic).Contains(FullName));
                }

                if (StaffPosts.Selected != null)
                {
                    expression = expression
                        .And(s => s.PostID == _staffPosts.Selected.ID);
                }

                if (StaffRoles.Selected != null)
                {
                    expression = expression
                        .And(s => s.RoleID == _staffRoles.Selected.ID);
                }

                if (Users.Selected != null)
                {
                    expression = expression
                        .And(s => s.UserID == _users.Selected.ID);
                }

                if (CarDealerships.Selected != null)
                {
                    expression = expression
                        .And(s => s.CarDealershipID == _carDealerships.Selected.ID);
                }

                if (ShowIsDeleted == true)
                {
                    expression = expression.And(c => c.IsDeleted == true || c.IsDeleted == false);
                }

                return expression;
            }
        }

        #endregion Other
    }
}