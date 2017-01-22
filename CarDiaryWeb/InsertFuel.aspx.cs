using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarDiaryWeb
{
    public partial class InsertFuel : System.Web.UI.Page
    {
        String userName;
        //selected type of price from ddl
        String priceTypeIns = "";
        String fuelType = "";
        int carMileage = 0;
        int car_id = 0;
        String dbDate = "";
        DatabaseUtility db = new DatabaseUtility();
        public string dateText;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (Application["userName"] != null)
                userName = Application["userName"].ToString();
            else
                userName = User.Identity.Name;

            if (Application["carID"] != null && !Application["carID"].ToString().Equals(""))
            {
                car_id = Int32.Parse(Application["carID"].ToString());
                carMileage = db.getPreviousMileage(car_id);
                fuelType = db.getFuelType(car_id);
            }
            else
                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);

            if (!IsPostBack)
            {
                fillFuelDropDown();
                fillPriceDropDown();
            }
        }

        protected void radioMethodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            switch (radioMethodGroup.SelectedIndex)
            {
                case 0: byHand.Visible = true; fromPhoto.Visible = false; emptyFields(); break;
                case 1: byHand.Visible = false; fromPhoto.Visible = true; break;
            }
        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            if (!invoiceUpload.Value.Equals(""))
            {
                if (invoiceUpload.PostedFile.ContentType == "image/jpeg" || invoiceUpload.PostedFile.ContentType == "image/png" || invoiceUpload.PostedFile.ContentType == "image/bmp" || invoiceUpload.PostedFile.ContentType == "image/gif")
                {
                    if (Convert.ToInt64(invoiceUpload.PostedFile.ContentLength) < 10000000)
                    {
                        string photoFolder = HttpContext.Current.Request.PhysicalApplicationPath;
                        //string photoFolder = Path.Combine(@"C:\Users\radoslav.gavrailov\Desktop\PI_proekt\PI_proekt\Organizer\Photos\", userName);
                        photoFolder = Path.Combine(photoFolder, "Photos", userName, "Invoices");
                        //photoFolder = Path.Combine(photoFolder,userName);
                        //photoFolder = Path.Combine(photoFolder, "Invoices");
                        if (!Directory.Exists(photoFolder))
                        {
                            Directory.CreateDirectory(photoFolder);
                        }

                        string extension = Path.GetExtension(invoiceUpload.PostedFile.FileName);
                        string uniqueFileName = Path.ChangeExtension(invoiceUpload.PostedFile.FileName, DateTime.Now.Ticks.ToString());

                        string image = Path.Combine(photoFolder, uniqueFileName + extension);
                        invoiceUpload.PostedFile.SaveAs(image);
                        lblStatus.Text = "<font color='green'>Файлът бе успешно обработен " + invoiceUpload.PostedFile.FileName + "</font>";

                        //call ABBYY OCR
                        string result = callOCRService(uniqueFileName + extension);
                        //proceed output string

                        string[] info = getInfoFromInvoice(result);

                        invoiceImage.Visible = false;

                        //fill in the possible controls and then vizualize it
                        fillFuelInfo(info[0], info[1], info[2]);

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

        private void fillFuelInfo(string quantity, string sum, string date)
        {
            if (quantity.Equals("") && sum.Equals("") && date.Equals(""))
            {
                byHand.Visible = true;
                lblStatus.Text = "Неуспешно разпознаване !!";
            }
            else
            {
                byHand.Visible = true;
                txtDate.Text = DateTime.ParseExact(date, "dd/MM/yyyy", null).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                //DateTime postingDate = Convert.ToDateTime(date);
                //txtDate.Text = string.Format("{0:MM/dd/yyyy}", postingDate);
                txtQuantity.Text = quantity;
                txtSum.Text = sum;
                //emptyFields();
                checkText(txtSum.Text);
            }

        }

        private void emptyFields()
        {
            txtDistance.Text = "";
            txtMileage.Text = "";
            txtDate.Text = "";
            txtPriceInfo.Text = "";
            txtQuantity.Text = "";
            txtSum.Text = "";
        }

        private string callOCRService(string photo)
        {
            string ApplicationId;
            string Password;
            string FilePath;

            // Name of application you created
            ApplicationId = "APP_CarDiaryWeb";
            // The password from e-mail sent by server after application was created
            Password = "hWs91QPonnyZSX8t0YkEnaRB";

            FilePath = "~/Photos/" + userName + "/Invoices/" + photo;

            //FilePath = "~/FileToRecognize.png";

            return GetResult(ApplicationId, Password, FilePath, "Bulgarian", "txt");

        }



        public string GetResult(string applicationId, string password, string filePath, string language, string exportFormat)
        {
            string myStr = "";
            // Specifying new post request filling it with file content
            var url = string.Format("http://cloud.ocrsdk.com/processImage?language={0}&exportFormat={1}", language, exportFormat);
            var localPath = Page.Server.MapPath(filePath);
            ICredentials credentials = new NetworkCredential(applicationId, password);
            IWebProxy proxy = System.Net.WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            var request = AbbyyReader.CreateRequest(url, "POST", credentials, proxy);
            AbbyyReader.FillRequestWithContent(request, localPath);

            // Getting task id from response
            var response = AbbyyReader.GetResponse(request);
            var taskId = AbbyyReader.GetTaskId(response);

            // Checking if task is completed and downloading result by provided url
            url = string.Format("http://cloud.ocrsdk.com/getTaskStatus?taskId={0}", taskId);
            var resultUrl = string.Empty;
            var status = string.Empty;
            while (status != "Completed")
            {
                System.Threading.Thread.Sleep(1000);
                request = AbbyyReader.CreateRequest(url, "GET", credentials, proxy);
                response = AbbyyReader.GetResponse(request);
                status = AbbyyReader.GetStatus(response);
                resultUrl = AbbyyReader.GetResultUrl(response);
            }

            request = (HttpWebRequest)HttpWebRequest.Create(resultUrl);
            using (HttpWebResponse result = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = result.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    myStr = sr.ReadToEnd();
                }
            }
            return myStr;
        }

        private string[] getInfoFromInvoice(string text) // quantity,sum,date
        {
            String[] textSplit = text.Split(' ');
            //List<string> list = textSplit.OfType<string>().ToList();
            List<string> list = new List<string>(textSplit);

            //премахване от листа на празните елементи 
            list = list.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            string quantity = "", sum = "";
            string date = "";

            String regex = "[0-9]*\\.[0-9]*";
            String regex1 = "[0-9]*\\,[0-9]*";
            String regexDate = "\\d{2}-\\d{2}-\\d{2}";
            String regexDate1 = "\\d{2}/\\d{2}/\\d{2}";
            String regexDate2 = "\\d{2}\\.\\d{2}\\.\\d{4}";
            String regexDate3 = "\\d{2}-\\d{2}-\\d{4}";//date lukoil
            String regexDate4 = "\\d{2}/\\d{2}/\\d{4}";// date shell

            string res, item;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ToString().Contains("\n"))
                {
                    item = list[i].ToString().Replace("\n", "");
                }
                else
                {
                    item = list[i].ToString();
                }

                res = "";
                if (!quantity.Equals("") && !sum.Equals("") && !date.Equals(""))
                    break;
                else
                //quantity OMV
                if ((item.ToLower().Contains("количество") || item.ToLower().Contains("личество")) && quantity.Equals(""))
                {
                    if (Regex.IsMatch(list[i + 1].ToString(), regex) || Regex.IsMatch(list[i + 1].ToString(), regex1))
                    {
                        if (list[i + 1].Contains(","))
                        {
                            res = list[i + 1].Replace(",", ".");
                            quantity = res.ToString();
                        }
                        else
                        {
                            quantity = list[i + 1].ToString();
                        }
                    }
                    else if (Regex.IsMatch(list[i - 1].ToString(), regex) || Regex.IsMatch(list[i - 1].ToString(), regex1))
                    {
                        if (list[i - 1].Contains(","))
                        {
                            res = list[i - 1].Replace(",", ".");
                            quantity = res.ToString();
                        }
                        else
                        {
                            quantity = list[i + 1].ToString();
                        }
                    }
                }
                //total cost OMV, Petrol,Lukoil
                else if ((item.ToLower().Contains("обща") || item.ToLower().Contains("що")) && list[i + 1].ToString().ToLower().Contains("сума") && sum.Equals(""))
                {
                    if (Regex.IsMatch(list[i + 2].ToString(), regex) || Regex.IsMatch(list[i + 2].ToString(), regex1))
                    {
                        if (list[i + 2].Contains(","))
                        {
                            res = list[i + 2].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 2].ToString();
                        }
                    }
                    else if (Regex.IsMatch(list[i + 3].ToString(), regex) || Regex.IsMatch(list[i + 3].ToString(), regex1))
                    {
                        if (list[i + 3].Contains(","))
                        {
                            res = list[i + 3].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 3].ToString();
                        }
                    }
                }
                // total cost OMV
                else if (item.ToLower().Contains("всичко") && sum.Equals(""))
                {
                    if (Regex.IsMatch(list[i + 1].ToString(), regex) || Regex.IsMatch(list[i + 1].ToString(), regex1))
                    {
                        if (list[i + 1].Contains(","))
                        {
                            res = list[i + 1].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 1].ToString();
                        }
                    }
                    else if (Regex.IsMatch(list[i + 2].ToString(), regex) || Regex.IsMatch(list[i + 2].ToString(), regex1))
                    {
                        if (list[i + 1].Contains(","))
                        {
                            res = list[i + 1].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 1].ToString();
                        }
                    }
                    //total cost Petrol
                    else if (item.ToLower().Contains("сума") && sum.Equals(""))
                    {
                        if (Regex.IsMatch(list[i + 1].ToString(), regex) || Regex.IsMatch(list[i + 1].ToString(), regex1))
                        {
                            if (list[i + 1].Contains(","))
                            {
                                res = list[i + 1].Replace(",", ".");
                                sum = res.ToString();
                            }
                            else
                            {
                                sum = list[i + 1].ToString();
                            }
                        }
                        else if (Regex.IsMatch(list[i + 2].ToString(), regex) || Regex.IsMatch(list[i + 2].ToString(), regex1))
                        {
                            if (list[i + 2].Contains(","))
                            {
                                res = list[i + 2].Replace(",", ".");
                                sum = res.ToString();
                            }
                            else
                            {
                                sum = list[i + 2].ToString();
                            }
                        }
                    }

                    //total cost Lukoil
                }
                else if (item.ToLower().Contains("общо") && sum.Equals(""))
                {
                    if (Regex.IsMatch(list[i + 1].ToString(), regex) || Regex.IsMatch(list[i + 1].ToString(), regex1))
                    {
                        if (list[i + 1].Contains(","))
                        {
                            res = list[i + 1].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 1].ToString();
                        }
                    }
                    else if (Regex.IsMatch(list[i + 2].ToString(), regex) || Regex.IsMatch(list[i + 2].ToString(), regex1))
                    {
                        if (list[i + 2].Contains(","))
                        {
                            res = list[i + 2].Replace(",", ".");
                            sum = res.ToString();
                        }
                        else
                        {
                            sum = list[i + 2].ToString();
                        }
                    }
                }
                //date OMV
                else if (item.ToLower().Contains("дата") && date.Equals(""))
                {
                    if (Regex.IsMatch(list[i + 1].ToString(), regexDate) || Regex.IsMatch(list[i + 1].ToString(), regexDate1))
                    {
                        date = list[i + 1].ToString();
                    }
                }
                //date Petrol,Lukoil, Shell
                else if ((Regex.IsMatch(item, regexDate) || Regex.IsMatch(item, regexDate1) || Regex.IsMatch(item, regexDate2) || Regex.IsMatch(item, regexDate3) || Regex.IsMatch(item, regexDate4)) && date.Equals(""))
                {
                    date = item;
                }
                //quantity Petrol
                else if ((Regex.IsMatch(item, regex) || Regex.IsMatch(item, regex1)) && quantity.Equals(""))
                {
                    if (list[i + 1].ToString().Contains("X") || list[i + 1].ToString().Contains("x"))
                    {
                        if (Regex.IsMatch(list[i + 2].ToString(), regex) || Regex.IsMatch(list[i + 2].ToString(), regex1))
                        {
                            if (item.Contains(",")) //list[i]
                            {
                                res = item.Replace(",", "."); //list[i]
                                quantity = res.ToString();
                            }
                            else
                            {
                                quantity = item;
                            }
                        }
                    }
                    else if (list[i + 2].ToString().Contains("X") || list[i + 2].ToString().Contains("x"))
                    {
                        if (Regex.IsMatch(list[i + 3].ToString(), regex) || Regex.IsMatch(list[i + 3].ToString(), regex1))
                        {
                            if (item.Contains(","))//list[i]
                            {
                                res = item.Replace(",", ".");//list[i]
                                quantity = res.ToString();
                            }
                            else
                            {
                                quantity = item;
                            }
                        }
                    }
                }
            }

            String[] result = { quantity.ToString(), sum.ToString(), date };

            return result;
        }


        protected void ddlFuel_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            priceTypeIns = ddlPrice.SelectedItem.Text;
            checkText(priceTypeIns);
        }

        protected void txtMileage_TextChanged(object sender, EventArgs e)
        {
            string s = txtMileage.Text;
            if (!s.ToString().Equals(""))
            {
                try
                {
                    int mileage = Int32.Parse(s);

                    if (carMileage != 0)
                    {
                        int distance = mileage - carMileage;
                        if (distance > 0)
                            txtDistance.Text = distance.ToString();
                        else
                            txtDistance.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        protected void txtSum_TextChanged(object sender, EventArgs e)
        {
            string text = txtSum.Text;
            checkText(text);
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            string text = txtQuantity.Text;
            checkText(text);
        }

        private void checkText(String s)
        {
            priceTypeIns = ddlPrice.SelectedItem.Text;

            if (!s.Equals("") && !txtQuantity.Text.Equals("")
                    && !txtSum.Text.Equals("")
                    && priceTypeIns.Equals("Общо")
                    && !fuelType.Equals("Electricity")
                    && !fuelType.Equals("Електричество"))
            {
                if (txtSum.Text.Contains(","))
                    txtSum.Text = txtSum.Text.Replace(",", ".");
                double totalCostTemp;
                try
                {
                    totalCostTemp = Convert.ToDouble(txtSum.Text);
                }
                catch (Exception ex)
                {
                    totalCostTemp = 0;
                    ExceptionUtility.LogException(ex, "InserFuel.aspx.cs");
                }
                if (txtQuantity.Text.Contains(","))
                    txtQuantity.Text = txtQuantity.Text.Replace(",", ".");
                double quantityTemp = Double.Parse(txtQuantity.Text);
                txtPriceInfo.Text = "Единична цена: " + (totalCostTemp / quantityTemp).ToString("0.##") + " BGN/л.";
            }
            else if (!s.Equals("")
                  && !txtQuantity.Text.Equals("")
                  && !txtSum.Text.Equals("")
                  && priceTypeIns.Equals("За единица")
                  && !fuelType.Equals("Electricity")
                  && !fuelType.Equals("Електричество"))
            {
                if (txtSum.Text.Contains(","))
                    txtSum.Text = txtSum.Text.Replace(",", ".");
                double unitCostTemp = Convert.ToDouble(txtSum.Text);
                if (txtQuantity.Text.Contains(","))
                    txtQuantity.Text = txtQuantity.Text.Replace(",", ".");
                double quantityTemp = Convert.ToDouble(txtQuantity.Text);
                txtPriceInfo.Text = "Обща цена: " + (unitCostTemp * quantityTemp).ToString("0.##") + " BGN/л.";
            }
            else if (!s.Equals("")
                  && !txtQuantity.Text.Equals("")
                  && !txtSum.Text.Equals("")
                  && priceTypeIns.Equals("За единица")
                  && (fuelType.Equals("Electricity") || !fuelType
                          .Equals("Електричество")))
            {
                if (txtSum.Text.Contains(","))
                    txtSum.Text = txtSum.Text.Replace(",", ".");
                double unitCostTemp = Double.Parse(txtSum.Text);
                if (txtQuantity.Text.Contains(","))
                    txtQuantity.Text = txtQuantity.Text.Replace(",", ".");
                double quantityTemp = Double.Parse(txtQuantity.Text);
                txtPriceInfo.Text = "Обща цена: " + (unitCostTemp * quantityTemp).ToString("0.##") + " BGN/кВч";
            }
            else if (!s.Equals("")
                  && !txtQuantity.Text.Equals("")
                  && !txtSum.Text.Equals("")
                  && priceTypeIns.Equals("Общо")
                  && (fuelType.Equals("Electricity") || !fuelType
                          .Equals("Електричество")))
            {
                if (txtSum.Text.Contains(","))
                    txtSum.Text = txtSum.Text.Replace(",", ".");
                double totalCostTemp = Double.Parse(txtSum.Text);
                if (txtQuantity.Text.Contains(","))
                    txtQuantity.Text = txtQuantity.Text.Replace(",", ".");
                double quantityTemp = Double.Parse(txtQuantity.Text);
                txtPriceInfo.Text = "Единична цена: " + (totalCostTemp / quantityTemp).ToString("0.##") + " BGN/кВч";
            }
            else if (s.Equals(""))
            {
                txtPriceInfo.Text = "";
            }

        }

        private void fillFuelDropDown()
        {
            String[] fuel = { };

            if (fuelType.Equals("Diesel") || fuelType.Equals("Дизел"))
            {
                String[] fuels = { "Дизел", "Биодизел", "Олио", "Премиум дизел" };
                fuel = fuels;
                lbQuantity.Text = "Количество(л)";
            }
            else if (fuelType.Equals("Gasoline") || fuelType.Equals("Бензин"))
            {
                String[] fuels = { "93 октана", "95 октана", "96 октана",
                    "98 октана", "100 октана", "Етанол (Е10)" };
                fuel = fuels;
                lbQuantity.Text = "Количество(л)";
            }
            else if (fuelType.Equals("AutoGas") || fuelType.Equals("Газ"))
            {
                String[] fuels = { "LPG" };
                fuel = fuels;
                lbQuantity.Text = "Количество(л)";
            }
            else if (fuelType.Equals("Metan") || fuelType.Equals("Метан"))
            {
                String[] fuels = { "CNG" };
                fuel = fuels;
                lbQuantity.Text = "Количество(л)";
            }
            else if (fuelType.Equals("Electricity")
                  || fuelType.Equals("Електричество"))
            {
                String[] fuels = { "Електричество" };
                fuel = fuels;
                lbQuantity.Text = "Количество(кВч)";
            }
            else
            {
                String[] fuels = { };
                fuel = fuels;
            }

            for (int i = 0; i < fuel.Length; i++)
            {
                ddlFuel.Items.Add(new ListItem(fuel[i]));
            }
        }

        private void fillPriceDropDown()
        {
            String[] prices = { };
            if (fuelType.Equals("Electricity") || fuelType.Equals("Електричество"))
            {
                String[] price = { "Общо" };
                prices = price;
            }
            else
            {
                String[] price = { "Общо", "За единица" };
                prices = price;
            }

            for (int i = 0; i < prices.Length; i++)
            {
                ddlPrice.Items.Add(new ListItem(prices[i]));
            }

        }

        protected void btnAddFuel_Click(object sender, EventArgs e)
        {
            string date = "";
            int mileage = 0, distance = 0;
            double quantity = 0, totalCost = 0, unitPrice = 0;
            priceTypeIns = ddlPrice.SelectedItem.Text;
            string dt = Request.Form[txtDate.UniqueID];

            if (!dt.Equals(""))
            {
                date = dt;

                if (checkDate(date))
                {
                    if (!txtMileage.Text.Equals(""))
                    {
                        try
                        {
                            mileage = Int32.Parse(txtMileage.Text);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if (mileage < carMileage)
                        {
                            createDialog(1);
                            goto end;
                        }
                    }
                    if (!txtDistance.Text.Equals(""))
                    {
                        try
                        {
                            distance = Int32.Parse(txtDistance.Text);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if (!txtQuantity.Text.Equals(""))
                        {
                            try
                            {
                                quantity = Double.Parse(txtQuantity.Text);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            if (!txtSum.Text.Equals(""))
                            {
                                if (priceTypeIns.Equals("Общо"))
                                {
                                    totalCost = Double.Parse(txtSum.Text);
                                    totalCost = Math.Round(totalCost, 2);
                                    unitPrice = totalCost / quantity;
                                    unitPrice = Math.Round(unitPrice, 2);
                                }
                                else if (priceTypeIns.Equals("За единица"))
                                {
                                    unitPrice = Double.Parse(txtSum.Text);
                                    unitPrice = Math.Round(unitPrice, 2);
                                    totalCost = unitPrice * quantity;
                                    totalCost = Math.Round(totalCost, 2);
                                }
                                double average = quantity / distance * 100;
                                average = Math.Round(average, 2);
                                fuel_consumption fuelCons = new fuel_consumption(car_id, date, mileage, fuelType, distance, quantity, unitPrice, totalCost, average);

                                Boolean insert = db.addRefueling(fuelCons);
                                if (insert)
                                {
                                    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/CarProfile.aspx", Response);
                                }
                                else
                                {
                                    Response.Write("<script>alert('Грешка при добавяне на зареждането !')</script>");
                                }
                            }
                            else
                            {
                                createDialog(6);
                            }
                        }
                        else
                        {
                            createDialog(5);
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
            else
            {
                createDialog(2);
            }

            end:;
        }

        private void createDialog(int domain)
        {
            switch (domain)
            {
                case 1: Response.Write("<script>alert('Моля попълненете коректна стойност за \"Kилометраж\" (предишната стойност е : " + carMileage + ") !')</script>"); break;
                case 2: Response.Write("<script>alert('Моля попълненете стойност за \"Дата\" !')</script>"); break;
                case 3: Response.Write("<script>alert('Моля попълнете стойност за \"Изминато\" !')</script>"); break;
                case 4: Response.Write("<script>alert('Моля попълнете коректна стойност за \"Дата\"! (попълнената дата е или по-голяма от системната дата или по-малка от последното ви зареждане " + dbDate + ") !')</script>"); break;
                case 5: Response.Write("<script>alert('Моля попълнете стойност за \"Количество\" !')</script>"); break;
                case 6: Response.Write("<script>alert('Моля попълнете стойност за \"Цена\" !')</script>"); break;
            }

        }

        private Boolean checkDate(string dateReceived)
        {

            // check inserted date -> compare it with current date and the date of
            // the last refueling from db

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

            dbDate = db.getFuelConsumptionLastDate(car_id);

            if (!dbDate.Equals(""))
            {
                try
                {
                    DateTime dbDateReceived = DateTime.ParseExact(dbDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    if ((inputDate.Date > dbDateReceived.Date
                            || inputDate.Date == dbDateReceived.Date) && (inputDate.Date < curDate.Date || inputDate.Date == curDate.Date))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    // Log the exception.
                    ExceptionUtility.LogException(ex, "InsertFuel.aspx.cs");
                    return false;
                }
            }
            else
            {
                if (inputDate.Date < curDate.Date)
                    return true;
                else
                    return false;
            }



        }
    }
}