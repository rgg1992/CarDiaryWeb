using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace CarDiaryWeb
{
    public partial class InsertCar : System.Web.UI.Page
    {
        string brand, model, userName, fuel, engine, image;
        int year, horsePowers;
        int car_id = 0;
        DatabaseUtility db = new DatabaseUtility();
        bool update;

        protected void Page_Load(object sender, EventArgs e)
        {
            RangeValidator1.MinimumValue = "1900";
            RangeValidator1.MaximumValue = DateTime.Now.Year.ToString();
            RangeValidator1.ErrorMessage = "Моля въведете конкретна стойност за Година (1900 - " + DateTime.Now.Year.ToString() + ")!";

            //if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/Account/Login.aspx", Response);
            //}

            //Labelcount.Text = Application["carCount"].ToString();
            if (Application["userName"] != null)
                userName = Application["userName"].ToString();
            else
                userName = User.Identity.Name;

            if (Application["carID"] != null && !Application["carID"].Equals(""))
                car_id = Int32.Parse(Application["carID"].ToString());

            if (ddlFuel.Items.Count == 1)
            {
                ddlFuel.Items.Add(new ListItem("Бензин"));
                ddlFuel.Items.Add(new ListItem("Дизел"));
                ddlFuel.Items.Add(new ListItem("Метан"));
                ddlFuel.Items.Add(new ListItem("Електричество"));
                ddlFuel.Items.Add(new ListItem("Газ"));
            }

            if (!IsPostBack)
            {
                brand = ""; model = ""; fuel = ""; engine = ""; image = "";
                year = 0; horsePowers = 0;
                update = false;
                Application["update"] = false;
                ddlBrand.AppendDataBoundItems = true;
                try
                {
                    using (var context = new Entities())
                    {
                        var brands = (from cb in context.car_brands
                                      select cb.car_brand).Distinct().ToList();

                        ddlBrand.DataSource = brands;
                        ddlBrand.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ddlBrand.Items.Add(new ListItem("Добави Марка"));
                }

                if (car_id != 0)
                {
                    Page.Title = "Промяна на автомобил";
                    update = true;
                    Application["update"] = true;
                    fillInfoAboutCar(car_id, userName);
                }
            }
            else
            {
                //profileImage.ImageUrl = "~/no_photo.png";
            }
        }

        private void fillInfoAboutCar(int carID, string user)
        {
            car car = db.readCar(carID, user);
            tbYear.Text = car.year.ToString();
            tbEngine.Text = car.engine;
            tbHorsePowers.Text = car.h_powers.ToString();
            profileImage.ImageUrl = car.image;
            btnAddCar.Text = "Редактирай";
            brand = car.brand;
            ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindByText(brand));
            fillDDLModel();
            ddlModel.Enabled = true;
            ddlModel.SelectedIndex = ddlModel.Items.IndexOf(ddlModel.Items.FindByText(car.model));
            ddlFuel.SelectedIndex = ddlFuel.Items.IndexOf(ddlFuel.Items.FindByText(car.fuel));
            update = true;
            Application["update"] = true;
        }

        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            brand = ddlBrand.SelectedItem.Text;

            if (brand.Equals("Добави Марка"))
            {
                //if choosen value is "Add Brand" show div for inserting new brand and new model in db

                rowLabels.Visible = true;
                lbNewBrand.Visible = true;
                rowTextBox.Visible = true;
                tbNewBrand.Visible = true;
                tbNewModel.Visible = true;
                ddlModel.Enabled = false;
                tbNewModel.Text = "";
                tbNewBrand.Text = "";

            }
            else
            {
                rowLabels.Visible = false;
                lbNewBrand.Visible = false;
                tbNewBrand.Visible = false;
                rowTextBox.Visible = false;
                tbNewBrand.Text = "";

                fillDDLModel();
            }

        }

        protected void fillDDLModel()
        {
            ddlModel.Items.Clear();
            ddlModel.Items.Add(new ListItem("--Изберете модел--"));

            ddlModel.AppendDataBoundItems = true;
            //String strConnString = ConfigurationManager.ConnectionStrings["CarDiaryDB"].ConnectionString;
            //String strQuery = "select car_model from car_brands where car_brand=@carBrand";
            //SqlConnection con = new SqlConnection(strConnString);
            //SqlCommand cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@carBrand", brand);
            //cmd.CommandType = CommandType.Text;
            //cmd.CommandText = strQuery;
            //cmd.Connection = con;
            //try
            //{
            //    con.Open();
            //    ddlModel.DataSource = cmd.ExecuteReader();
            //    ddlModel.DataTextField = "car_model";
            //    ddlModel.DataBind();
            try
            {
                using (var context = new Entities())
                {
                    var models = (from cb in context.car_brands
                                  where (cb.car_brand == brand)
                                  select cb.car_model).Distinct().ToList();

                    ddlModel.DataSource = models;
                    ddlModel.DataBind();
                    if (ddlModel.Items.Count > 1)
                    {
                        ddlModel.Enabled = true;
                    }
                    else
                    {
                        ddlModel.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ddlModel.Items.Add(new ListItem("Добави Модел"));
            }
        }

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            model = ddlModel.SelectedItem.Text;

            if (model.Equals("Добави Модел"))
            {

                rowLabels.Visible = true;
                rowTextBox.Visible = true;
                lbNewBrand.Visible = false;
                tbNewBrand.Visible = false;
                tbNewModel.Text = "";

            }
            else
            {
                rowLabels.Visible = false;
                rowTextBox.Visible = false;
                tbNewModel.Text = "";
            }
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            if (!profileImageUpload.Value.Equals(""))
            {
                //if (profileImageUpload.PostedFile)
                //{
                if (profileImageUpload.PostedFile.ContentType == "image/jpeg" || profileImageUpload.PostedFile.ContentType == "image/png" || profileImageUpload.PostedFile.ContentType == "image/bmp" || profileImageUpload.PostedFile.ContentType == "image/gif")
                {
                    if (Convert.ToInt64(profileImageUpload.PostedFile.ContentLength) < 10000000)
                    {
                        //string photoFolder = Path.Combine(@"C:\Users\Rado\Desktop\Projects\60%_PI_proekt\Organizer\Photos\", userName);
                        string path = HttpContext.Current.Request.PhysicalApplicationPath;
                        //string photoFolder = Path.Combine(@"C:\Users\radoslav.gavrailov\Desktop\PI_proekt\PI_proekt\Organizer\Photos\", userName);
                        string photoFolder = Path.Combine(path, "Photos", userName);
                        if (!Directory.Exists(photoFolder))
                        {
                            Directory.CreateDirectory(photoFolder);
                        }

                        string extension = Path.GetExtension(profileImageUpload.PostedFile.FileName);
                        string uniqueFileName = Path.ChangeExtension(profileImageUpload.PostedFile.FileName, DateTime.Now.Ticks.ToString());

                        string image = Path.Combine(photoFolder, uniqueFileName + extension);
                        profileImageUpload.PostedFile.SaveAs(image);
                        lblStatus.Text = "<font color='green'>Файлът бе успешно записан " + profileImageUpload.PostedFile.FileName + "</font>";
                        profileImage.ImageUrl = "~/Photos/" + userName + "/" + uniqueFileName + extension;
                    }
                    else
                        lblStatus.Text = "Файлът трябва да бъде по-малък от 10 мб";
                }
                else
                    lblStatus.Text = "Файлът трябва да бъде от тип jpeg, jpg, png, bmp или gif";
            }
            else
                lblStatus.Text = "Не сте посочили файл";

        }

        protected void btnAddCar_Click(object sender, EventArgs e)
        {

            fuel = ddlFuel.SelectedItem.Text;
            engine = tbEngine.Text;

            //brand = ddlBrand.SelectedItem.Text;
            if (!tbNewBrand.Text.Equals(""))
            {
                brand = tbNewBrand.Text;
            }
            else
                brand = ddlBrand.SelectedItem.Text;
            if (!tbNewModel.Text.Equals(""))
            {
                model = tbNewModel.Text;
                db.addCarBrandModel(brand, model);
            }
            else
                model = ddlModel.SelectedItem.Text;


            if (!brand.Equals("--Изберете марка--") && !brand.Equals(""))
            {
                if (!model.Equals("--Изберете модел--") && !model.Equals(""))
                {
                    if (!tbYear.Text.Equals("") && tbYear.Text != null)
                    {
                        year = Int32.Parse(tbYear.Text);

                        if (year > 1900 && year < Int32.Parse(DateTime.Now.Year.ToString()))
                        {
                            if (!fuel.Equals("--Изберете гориво--") && !fuel.Equals(""))
                            {
                                if (!engine.Equals(""))
                                {
                                    if (!tbHorsePowers.Text.Equals("") && tbHorsePowers.Text != null)
                                    {
                                        //try catch
                                        horsePowers = Int32.Parse(tbHorsePowers.Text);
                                        if (horsePowers > 0 && horsePowers < 2000)
                                        {
                                            image = profileImage.ImageUrl;
                                            car car = new car(brand, model, year, engine, fuel, horsePowers, image, userName);
                                            if (!Convert.ToBoolean(Application["update"].ToString()))
                                            {
                                                int car_id = db.insertCar(car, userName);
                                                Application["carID"] = car_id;
                                                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);
                                            }
                                            else
                                            {
                                                if (db.updateCar(car_id, car))
                                                {
                                                    Application["carID"] = car_id;
                                                    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);
                                                }
                                                else
                                                {
                                                    Application["carID"] = car_id;
                                                    Response.Write("<script>alert('Грешка при редактиране на автомобила !')</script>");
                                                }
                                            }
                                        }
                                        else
                                            createDialog(1);
                                    }
                                    else
                                        createDialog(2);
                                }
                                else
                                    createDialog(3);
                            }
                            else
                                createDialog(4);
                        }
                        else
                            createDialog(5);
                    }
                    else
                        createDialog(6);
                }
                else
                    createDialog(7);
            }
            else
                createDialog(8);


        }

        private void createDialog(int domain)
        {
            switch (domain)
            {
                case 1: Response.Write("<script>alert('Моля попълнете коректна стойност за \"Конски сили\" (стойност между 1 и 2000!')</script>"); break;
                case 2: Response.Write("<script>alert('Моля попълнете стойност за \"Конски сили\" !')</script>"); break;
                case 3: Response.Write("<script>alert('Моля попълнете стойност за \"Двигател\" !')</script>"); break;
                case 4: Response.Write("<script>alert('Моля изберете стойност за \"Гориво\" !')</script>"); break;
                case 5: Response.Write("<script>alert('Моля попълнете коректна стойност за \"Година\" (стойност между 1900 и " + DateTime.Now.Year + ") !')</script>"); break;
                case 6: Response.Write("<script>alert('Моля попълнете стойност за \"Година\" !')</script>"); break;
                case 7: Response.Write("<script>alert('Моля изберете стойност за \"Модел\" !')</script>"); break;
                case 8: Response.Write("<script>alert('Моля изберете стойност за \"Марка\" !')</script>"); break;
            }

        }

    }
}