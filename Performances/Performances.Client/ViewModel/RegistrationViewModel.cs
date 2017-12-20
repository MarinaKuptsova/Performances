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
using Performances.Api.Models;
using Performances.Client.Commands;
using Performances.Client.Model;
using Performances.Client.View;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class RegistrationViewModel : BaseModel
    {
        public MainViewModel Parent { get; set; }

        public User RegisterUser { get; set; }
        public CreativeTeam RegisterCreativeTeam { get; set; }
        public Performances.Model.File AttachedPhoto { get; set; }
        
        public UserRegistrationView uRegistrationView { get; set; } 
        public CreativeTeamRegistrationView ctRegistrationView { get; set; }

        public string Warning { get; set; }
        public BitmapImage ImageThumbnail { get; set; }
        

        public RegistrationViewModel(MainViewModel mvm)
        {
            Parent = mvm;
        }

        #region GetAvatar
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

        #region Registration

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


        #region Registration

        private RelayCommand _createCreativeTeamCommand;

        public RelayCommand CreateCreativeTeamCommand
        {
            get
            {
                return _createUserCommand ?? (_createCreativeTeamCommand =
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

    }
}
