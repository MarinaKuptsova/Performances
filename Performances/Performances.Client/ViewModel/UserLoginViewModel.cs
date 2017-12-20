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
                //var user = await DataAccess.DataAccess.LoginUser(OldUser.FirstName, OldUser.LastName, OldUser.Password);
                //Parent.MainUser = user;
                //if (Parent.MainUser == null)
                //{
                //    WarningText = "*Неверно введены имя, фамилия или пароль";
                //    return null;
                //}
                //else
                //{
                //    ConverterByteToImage(user.Ava);
                //    Parent.CurrentScreenType = ScreenTypes.Dialogs;
                //    Parent.SetScreen();
                //    return user;
                //}
                return null;
            }
        }

        public bool CanExecuteLoginUserCommand(object param)
        {
            return true;
        }
        #endregion

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public object View()
        {
            throw new NotImplementedException();
        }
    }
}
