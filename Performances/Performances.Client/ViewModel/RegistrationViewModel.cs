using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;
using Performances.Api.Models;
using Performances.Client.Commands;
using Performances.Client.Model;
using Performances.Client.View;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class RegistrationViewModel : BaseModel, IScreen
    {
        public MainViewModel Parent { get; set; }

        public User RegisterUser { get; set; }
        public CreativeTeam RegisterCreativeTeam { get; set; }
        public Performances.Model.File AttachedPhoto { get; set; }
        
        public UserRegistrationView uRegistrationView { get; set; } 
        public CreativeTeamRegistrationView ctRegistrationView { get; set; }
        public WelcomeView wView { get; set; }

        public string Warning { get; set; }
        public BitmapImage ImageThumbnail { get; set; }
        
        public RegisterStates RegisterState { get; set; }

        public RegistrationViewModel(MainViewModel mvm)
        {
            Parent = mvm;
            RegisterState = RegisterStates.None;
        }

        #region OpenFileDialogCommand

        private RelayCommand _openFileDialogCommand;

        public RelayCommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ?? (_openFileDialogCommand =
                           new RelayCommand(param => ExecuteOpenFileDialogCommand(param),
                               param => CanExecuteOpenFileDialogCommand(param)));
            }
        }

        public void ExecuteOpenFileDialogCommand(object param)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var path = dialog.FileName;
                AttachedPhoto.Filename = dialog.SafeFileName;
                Image image1 = Image.FromFile(path);

                ImageConverter converter = new ImageConverter();
                AttachedPhoto.Bytes = (byte[])converter.ConvertTo(image1, typeof(byte[]));

                if (AttachedPhoto.Bytes != null)
                {
                    MemoryStream stream = new MemoryStream(AttachedPhoto.Bytes);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.EndInit();
                    ImageThumbnail = image;
                }
            }
        }

        public bool CanExecuteOpenFileDialogCommand(object param)
        {
            return true;
        }

        #endregion

        #region CreateUserCommand

        private RelayCommand _createUserCommand;

        public RelayCommand CreateUserCommand
        {
            get
            {
                return _createUserCommand ?? (_createUserCommand =
                           new RelayCommand(param => ExecuteCreateUserCommand(param),
                               param => CanExecuteCreateUserCommand(param)));
            }
        }

        async Task<User> ExecuteCreateUserCommand(object param)
        {
            if (RegisterUser.Name == null || RegisterUser.Surname == null || RegisterUser.Password == null)
            {
                Warning = "Заполните все обязательные поля";
                return null;
            }
            else
            {
                CreateUserParameters createUser = new CreateUserParameters();
                createUser.user = RegisterUser;
                createUser.photo = AttachedPhoto;
                var user = await DataAccess.DataAccess.CreateUser(createUser);
                Warning = null;
                Parent.CurrentScreenType = ScreenTypes.Login;
                Parent.SetScreen();
                return user;
            }
        }

        public bool CanExecuteCreateUserCommand(object param)
        {
            return true;
        }

        #endregion
        
        #region CreateCreativeTeamCommand

        private RelayCommand _createCreativeTeamCommand;

        public RelayCommand CreateCreativeTeamCommand
        {
            get
            {
                return _createCreativeTeamCommand ?? (_createCreativeTeamCommand =
                           new RelayCommand(param => ExecuteCreateCreativeTeamCommand(param),
                               param => CanExecuteCreateCreativeTeamCommand(param)));
            }
        }

        async Task<CreativeTeam> ExecuteCreateCreativeTeamCommand(object param)
        {
            if (RegisterUser.Name == null || RegisterUser.Surname == null || RegisterUser.Password == null)
            {
                Warning = "Заполните все обязательные поля";
                return null;
            }
            else
            {
                CreateCreativeTeamParameters createCreativeTeam = new CreateCreativeTeamParameters();
                createCreativeTeam.creativeTeam = RegisterCreativeTeam;
                createCreativeTeam.photo = AttachedPhoto.Bytes;
                createCreativeTeam.filename = AttachedPhoto.Filename;
                var creativeTeam = await DataAccess.DataAccess.CreateCreativeTeam(createCreativeTeam);
                Warning = null;
                Parent.CurrentScreenType = ScreenTypes.Login;
                Parent.SetScreen();
                return creativeTeam;
            }
        }

        public bool CanExecuteCreateCreativeTeamCommand(object param)
        {
            return true;
        }

        #endregion

        #region ShowUserRegistrationViewCommand

        private RelayCommand _showUserRegistrationViewCommand;

        public RelayCommand ShowUserRegistrationViewCommand
        {
            get
            {
                return _showUserRegistrationViewCommand ?? (_showUserRegistrationViewCommand =
                           new RelayCommand(param => ExecuteShowUserRegistrationViewCommand(param),
                               param => CanExecuteShowUserRegistrationViewCommand(param)));
            }
        }

        public void ExecuteShowUserRegistrationViewCommand(object param)
        {
            RegisterState = RegisterStates.AsUser;
            Parent.SetScreen();
        }

        public bool CanExecuteShowUserRegistrationViewCommand(object param)
        {
            return true;
        }

        #endregion

        #region ShowCreativeTeamRegistrationViewCommand

        private RelayCommand _showCreativeTeamRegistrationViewCommand;

        public RelayCommand ShowCreativeTeamRegistrationViewCommand
        {
            get
            {
                return _showCreativeTeamRegistrationViewCommand ?? (_showCreativeTeamRegistrationViewCommand =
                           new RelayCommand(param => ExecuteShowCreativeTeamRegistrationViewCommand(param),
                               param => CanExecuteShowCreativeTeamRegistrationViewCommand(param)));
            }
        }

        public void ExecuteShowCreativeTeamRegistrationViewCommand(object param)
        {
            RegisterState = RegisterStates.AsCreativeTeam;
            Parent.SetScreen();
        }

        public bool CanExecuteShowCreativeTeamRegistrationViewCommand(object param)
        {
            return true;
        }

        #endregion

        #region ToLoginCommand

        private RelayCommand _toLoginCommand;

        public RelayCommand ToLoginCommand
        {
            get
            {
                return _createUserCommand ?? (_toLoginCommand =
                           new RelayCommand(param => ExecuteToLoginCommand(param),
                               param => CanExecuteToLoginCommand(param)));
            }
        }

        public void ExecuteToLoginCommand(object param)
        {
            Parent.CurrentScreenType = ScreenTypes.Login;
            Parent.SetScreen();
        }

        public bool CanExecuteToLoginCommand(object param)
        {
            return true;
        }

        #endregion

        public object View()
        {
            switch (RegisterState)
            {
                case RegisterStates.None:
                    return wView ?? (wView = new WelcomeView() { DataContext = this });
                case RegisterStates.AsUser:
                    return uRegistrationView ?? (uRegistrationView = new UserRegistrationView() {DataContext = this});
                case RegisterStates.AsCreativeTeam:
                    return ctRegistrationView ??
                           (ctRegistrationView = new CreativeTeamRegistrationView() {DataContext = this});
                default:
                    return null;
            }
        }

        public void Initialize()
        {
        }
    }
}
