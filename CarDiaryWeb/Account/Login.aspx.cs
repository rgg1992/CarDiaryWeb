using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using CarDiaryWeb.Models;

namespace CarDiaryWeb.Account
{
    public partial class Login : Page
    {
        public static string userName = "";
         DatabaseUtility db = new DatabaseUtility();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (User.Identity.Name != null && !User.Identity.Name.Equals(""))
            {
                loadNextPage(User.Identity.Name);
            }

            RegisterHyperLink.NavigateUrl = "Register";
            //OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            {
                //Validate the user password
                 // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();

                // This doen't count login failures towards account lockout
                // To enable password failures to trigger lockout, change to shouldLockout: true
                var result = signinManager.PasswordSignIn(UserName.Text, Password.Text, RememberMe.Checked, shouldLockout: false);

                switch (result)
                {
                    case SignInStatus.Success:
                        IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                        userName = UserName.Text;
                        if (!userName.Equals(""))
                        {
                            Application["userName"] = userName;

                            loadNextPage(userName);
                        }
                        break;
                    default:
                        FailureText.Text = "Невалидно потребителско име или парола.";
                        ErrorMessage.Visible = true;
                        break;
                }

                //userName = "test";
                //Application["userName"] = userName;
                if (!userName.Equals(""))
                    loadNextPage(userName);
            }
            }

        protected void loadNextPage(string userName)
        {
            
            int carCount = db.getCarCountForUser(userName);
            Application["carCount"] = carCount;
            Application["carID"] = null;
            switch (carCount)
            {
                case 0:
                    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertCar.aspx", Response);
                    break;
                case 1:
                    IdentityHelper.RedirectToReturnUrl("~/CarProfile.aspx", Response);
                    break;
                case -99:
                    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertCar.aspx", Response);
                    break;
                default:
                    IdentityHelper.RedirectToReturnUrl("~/CarProfile.aspx", Response);
                    break;
            }
        }

    }
}
