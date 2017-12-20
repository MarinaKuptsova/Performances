using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performances.Client.Commands;
using Performances.Client.Model;
using Performances.Client.View;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class UserLoginViewModel : BaseModel, IScreen
    {
        protected UserLoginView _userLoginView;
        public MainViewModel Parent { get; set; }
        public string WarningText { get; set; }
        public User CurrentUser { get; set; }

        public UserLoginViewModel(MainViewModel mvm)
        {
            Parent = mvm;
        }

        #region Autentification

        private RelayCommand _loginUserCommand;

        public RelayCommand LoginUserCommand
        {
            get
            {
                return _loginUserCommand ?? (_loginUserCommand =
                           new RelayCommand(param => ExecuteLoginUserCommand(param),
                               param => CanExecuteLoginUserCommand(param)));
            }
        }

        async Task<User> ExecuteLoginUserCommand(object param)
        {
            if (CurrentUser.Email == null || CurrentUser.Password == null)
            {
                WarningText = "*Заполните все поля";
                return null;
            }
            else
            {
                var user = await DataAccess.DataAccess.LoginUser(CurrentUser.Email, CurrentUser.Password);
                Parent.MainUser = user;
                if (Parent.MainUser == null)
                {
                    WarningText = "*Неверно введены email или пароль";
                    return null;
                }
                else
                {
                    //ConverterByteToImage(user.PhotoBytes);
                    Parent.CurrentScreenType = ScreenTypes.Events;
                    Parent.SetScreen();
                    return user;
                }
            }
        }

        public bool CanExecuteLoginUserCommand(object param)
        {
            return true;
        }
        #endregion

        #region BackToRegistration

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand =
                           new RelayCommand(param => ExecuteBackCommand(param),
                               param => CanExecuteBackCommand(param)));
            }
        }

        public void ExecuteBackCommand(object param)
        {
            Parent.CurrentScreenType = ScreenTypes.Register;
            Parent.SetScreen();
        }

        public bool CanExecuteBackCommand(object param)
        {
            return true;
        }

        #endregion

        public void Initialize()
        {
            CurrentUser = new User();
            WarningText = null;
        }

        public object View()
        {
            return _userLoginView ?? (_userLoginView = new UserLoginView() {DataContext = this});
        }
    }
}
