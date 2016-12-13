using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;

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
                // Validate the user password
                //var manager = new UserManager();
                //ApplicationUser user = manager.Find(UserName.Text, Password.Text);
                //if (user != null)
                //{
                //    IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                //    // call db to get number of saved cars for logged in user
                //    userName = UserName.Text;
                //    if (!userName.Equals(""))
                //    {
                //        Application["userName"] = userName;

                //        loadNextPage(userName);
                //    }
                //}
                //else
                //{
                //    FailureText.Text = "Invalid username or password.";
                //    ErrorMessage.Visible = true;
                //}
                userName = "test";
                Application["userName"] = userName;

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
