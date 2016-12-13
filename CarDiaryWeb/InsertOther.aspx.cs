using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarDiaryWeb
{
    public partial class InsertOther : System.Web.UI.Page
    {
        other_costs otherCost;
        int car_id;
        DatabaseUtility db = new DatabaseUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Application["carID"] != null)
            {
                car_id = Int32.Parse(Application["carID"].ToString());
            }
            else
                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);
            otherCost = new other_costs();

            if (!IsPostBack)
            {
                fillDDLCategory();
            }
        }

        protected void btnAddCost_Click(object sender, EventArgs e)
        {
            string dt = Request.Form[txtDate.UniqueID];
            if (!ddlCategory.SelectedItem.Text.Equals("Избери"))
            {
                string main_cat = getMainCategory(ddlCategory.SelectedIndex);
                otherCost.category = (main_cat + " / " + ddlCategory.SelectedItem.Text);
                if (!dt.Equals(""))
                {
                    if (checkDate(dt))
                    {
                        otherCost.cost_date = dt;
                        if (!txtPrice.Text.Equals(""))
                        {
                            if (txtPrice.Text.Contains(","))
                                txtPrice.Text = txtPrice.Text.Replace(",", ".");
                            otherCost.total_cost = Double.Parse(txtPrice.Text);
                            if (!txtMileage.Text.Equals(""))
                                otherCost.mileage = (Int32.Parse(txtMileage.Text));
                            else
                                otherCost.mileage = 0;
                            if (!txtNotes.Text.Equals(""))
                                otherCost.notes = txtNotes.Text;
                            else otherCost.notes = "";
                            otherCost.car_id = car_id;

                            Boolean insert = db.addOtherCost(otherCost);

                            if (insert)
                            {
                                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);
                            }
                            else
                            {
                                Response.Write("<script>alert('Грешка при добавяне на друг разход !')</script>");
                            }
                        }
                        else
                        {
                            createDialog(1);
                        }
                    }
                    else
                    {
                        createDialog(2);
                    }
                }
                else
                {
                    createDialog(3);
                }
            }
            else
            {
                createDialog(4);
            }
        }

        private void createDialog(int domain)
        {
            switch (domain)
            {
                case 1: Response.Write("<script>alert('Моля попълнете стойност за \"Цена\" !')</script>"); break;
                case 2: Response.Write("<script>alert('Моля попълнете коректна стойност за \"Дата\"! (попълнената дата е по-голяма от системната дата)')</script>"); break;
                case 3: Response.Write("<script>alert('Моля попълнете стойност за \"Дата\" !')</script>"); break;
                case 4: Response.Write("<script>alert('Моля изберете стойност за \"Категория\" !')</script>"); break;
            }

        }

        private void fillDDLCategory()
        {
            String[] rowItems = {"Избери","main_Поддръжка","Други","Автомивка","Пълно обслужване","Масла","Филтри","Антифриз","Ангренажен ремък/верига","Спирачни накладки","Реглаж","Добавки за гориво","Смяна на гуми",
            "main_Ремонти","Други","Двигател","Скорости","Окачване","Интериор","Спирачна уредба","Шаси","Кормилна уредба","Екстериор","Светлини","Електроника",
            "main_Покупки","Други","Части","Гуми","Аксесоари","Консумативи","Първоначална покупка на МПС",
            "main_Тунинг","Други","Чип тунинг","Оптичен тунинг","Силов тунинг",
            "main_Други такси","Застраховки","Данъци","Технически преглед","Пътни такси/данъци","Паркинг","Други"};

            ddlCategory.DataSource = rowItems;
            ddlCategory.DataBind();

            foreach (ListItem item in ddlCategory.Items)
            {
                if (item.Value.StartsWith("main_"))
                {
                    item.Text = item.Text.ToString().Replace("main_", "");
                    item.Attributes.Add("style", "font: italic bold 16px/30px Georgia, serif;");
                    item.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    item.Attributes.Add("style", "font-size:14px");
                }
            }

        }

        private Boolean checkDate(string dateReceived)
        {

            // check inserted date -> compare it with current date

            DateTime inputDate;
            DateTime curDate = DateTime.Now;
            //String dbDate;
            try
            {
                inputDate = DateTime.ParseExact(dateReceived, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                // Log the exception.
                ExceptionUtility.LogException(ex, "InsertFuel.aspx.cs");
                return false;
            }

            if (inputDate.Date < curDate.Date)
                return true;
            else
                return false;

        }

        private string getMainCategory(int index)
        {
            string main = "";
            //values are hard coded due to the current state of the input array
            // !!!!!!!!!! if array changes then this position values should be changed too
            if (index > 1 && index < 13)
            {
                main = "Поддръжка";
            }
            else if (index > 13 && index < 25)
            {
                main = "Ремонти";
            }
            else if (index > 25 && index < 32)
            {
                main = "Покупки";
            }
            else if (index > 32 && index < 37)
            {
                main = "Тунинг";
            }
            else if (index > 37 && index < 43)
            {
                main = "Други такси";
            }
            return main;
        }
    }
}