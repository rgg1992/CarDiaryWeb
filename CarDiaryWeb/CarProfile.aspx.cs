using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace CarDiaryWeb
{
    public partial class CarProfile : System.Web.UI.Page
    {
        DatabaseUtility db = new DatabaseUtility();
        string user;

        protected void Page_Init(object sender, EventArgs e)
        {

            //user = "test";
            //Application["userName"] = user;

            if (Application["userName"] != null)
                user = Application["userName"].ToString();
            else if(User.Identity.Name != null && !User.Identity.Name.Equals("")) 
                user = User.Identity.Name;
            else IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/Account/Login.aspx", Response);


            Application["carID"] = null;

            List<int> carID = db.getCarIDsForUser(user);

            foreach (int id in carID)
            {
                addNewTable(db.readCar(id, user));
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //user = "test";
            //Application["userName"] = user;
            //if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/Account/Login.aspx", Response);
            //}
        }

        private void addNewTable(car car)
        {
            CarInformation carInfoDB = CarInformation.GetCarInformationFromDB(car.id);

            Table tbl = new Table();


            tbl.Width = new Unit("100%");
            tbl.Height = new Unit("100%");


            //------------------row-----------------------
            TableRow rw = new TableRow();
            //---------------------cell--------------
            TableCell cell = new TableCell();
            cell.Width = 300;

            LinkButton carTitle = new LinkButton();
            carTitle.Font.Size = 20;
            carTitle.Text = car.brand + " " + car.model;
            carTitle.Click += new EventHandler(LinkButton1_Click);
            carTitle.ID = "Title" + car.id.ToString();
            carTitle.CssClass = "linkbtn";
            cell.Controls.Add(carTitle);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            Panel div = new Panel();
            div.Style.Add("float", "right");

            Button btnEdit = new Button();
            btnEdit.Click += new EventHandler(btnAddFuel_Click);
            btnEdit.Text = "Редактирай";
            btnEdit.Style.Add("margin-left", "15px");
            btnEdit.Style.Add("margin-right", "15px");
            btnEdit.ID = "Edit" + car.id.ToString();
            btnEdit.CssClass = "btn";

            div.Controls.Add(btnEdit);

            cell.Controls.Add(div);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            Panel div2 = new Panel();
            div.Style.Add("float", "left");

            Button btnDel = new Button();
            btnDel.Click += new EventHandler(btnDel_Click);
            btnDel.Text = "Изтрий";
            btnDel.Style.Add("margin-left", "15px");
            btnDel.Style.Add("margin-right", "15px");
            btnDel.ID = "Delete" + car.id.ToString();
            btnDel.CssClass = "btn";

            div2.Controls.Add(btnDel);

            cell.Controls.Add(div2);
            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);


            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();
            cell.RowSpan = 5;
            cell.Width = 300;

            ImageButton img = new ImageButton();
            img.ImageUrl = car.image;
            img.Width = 300;
            img.Height = 150;
            img.Style.Add("min-height", "150px");
            img.Style.Add("min-width", "100px");
            img.ID = "Img" + car.id.ToString();
            img.ToolTip = "Виж история на автомобила";
            img.Click += new ImageClickEventHandler(img_Click);
            cell.Controls.Add(img);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Style.Add("height", "10px");

            Label carInfo = new Label();
            carInfo.Font.Size = 10;
            carInfo.Text = car.fuel + ", " + car.engine + ", " + car.h_powers + " к.с., " + car.year;

            cell.Controls.Add(carInfo);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 4;
            cell.RowSpan = 3;

            List<Tuple<Double?, DateTime?>> list = db.getTotalCostPerMonth(car.id);
            List<Double?> total_cost = new List<double?>();
            List<DateTime?> period = new List<DateTime?>();

            foreach (Tuple<Double?, DateTime?> item in list)
            {
                total_cost.Add(item.Item1);
                period.Add(item.Item2);
            }

            List<String> sPeriod = new List<string>();

            foreach (DateTime item in period)
            {
                sPeriod.Add(item.ToString("MM/yyyy"));
            }
            
            if (total_cost.Count > 0)
            {
                Chart chart = new Chart();

                Series series = new Series("Default");
                series.ChartType = SeriesChartType.Spline;
                series["PieLabelStyle"] = "Disabled";
                chart.Series.Add(series);

                ChartArea chartArea = new ChartArea();
                ChartArea3DStyle areaStyle = new ChartArea3DStyle(chartArea);
                areaStyle.Rotation = 0;

                Axis yAxis = new Axis(chartArea, AxisName.Y);
                Axis xAxis = new Axis(chartArea, AxisName.X);

                chart.ChartAreas.Add(chartArea);

                // Bind the data to the chart
                chart.Series["Default"].Points.DataBindXY(sPeriod, total_cost);

                chart.Width = new System.Web.UI.WebControls.Unit(600, System.Web.UI.WebControls.UnitType.Pixel);
                chart.Height = new System.Web.UI.WebControls.Unit(200, System.Web.UI.WebControls.UnitType.Pixel);
                //string filename = Server.MapPath("~") + "/Chart.png";
                //chart.SaveImage(filename, ChartImageFormat.Png);

                chart.Legends.Add(new Legend("Legend1"));

                chart.Series["Default"].LegendText = "Обща сума в лв.";

                cell.Controls.Add(chart);
            }

            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);

            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 2;

            Label averageCons = new Label();
            averageCons.Font.Bold = true;
            averageCons.Font.Size = 35;
            averageCons.Style.Add("margin-left", "10px");

            double average = db.getAvgCons(car.id);
            if (average == 0)
            {
                averageCons.Text = "---";
            }
            else
            {
                averageCons.Text = average.ToString("0.##");
            }

            cell.Controls.Add(averageCons);
            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);

            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 2;

            Label averageConsInfo = new Label();
            averageConsInfo.Font.Size = 10;
            averageConsInfo.Style.Add("margin-left", "10px");

            if (car.fuel.Equals("Електричество"))
            {
                averageConsInfo.Text = "кВч/100км " + car.fuel;
            }
            else averageConsInfo.Text = "л./100км " + car.fuel;

            cell.Controls.Add(averageConsInfo);
            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);

            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();

            Label fillUps = new Label();
            fillUps.Font.Size = 11;
            fillUps.Font.Bold = true;
            fillUps.Style.Add("margin-left", "10px");
            fillUps.Style.Add("text-align", "center");
            fillUps.Width = new Unit("100%");

            int numFillUps = carInfoDB.fill_ups;
            if (numFillUps == 0)
                fillUps.Text = "-";
            else
                fillUps.Text = numFillUps.ToString();

            cell.Controls.Add(fillUps);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            Label minCons = new Label();
            minCons.Font.Size = 11;
            minCons.Font.Bold = true;
            minCons.Style.Add("margin-left", "10px");
            minCons.Style.Add("text-align", "center");
            minCons.Width = new Unit("100%");

            double minConsDB = carInfoDB.min_cons;
            if (minConsDB == 0)
                minCons.Text = "-";
            else
                minCons.Text = minConsDB.ToString("0.##");

            cell.Controls.Add(minCons);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            Label km = new Label();
            km.Font.Size = 11;
            km.Font.Bold = true;
            km.Style.Add("margin-left", "10px");
            km.Style.Add("text-align", "center");
            km.Width = new Unit("100%");

            int kmDB = carInfoDB.distance;
            if (kmDB == 0)
                km.Text = "-";
            else
                km.Text = kmDB.ToString("0.##");

            cell.Controls.Add(km);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            Label pricePerKm = new Label();
            pricePerKm.Font.Size = 11;
            pricePerKm.Font.Bold = true;
            pricePerKm.Style.Add("margin-left", "10px");
            pricePerKm.Style.Add("text-align", "center");
            pricePerKm.Width = new Unit("100%");
            if (String.IsNullOrEmpty(carInfoDB.price_per_km))
            {
                carInfoDB.price_per_km = "0";
            }
            if (carInfoDB.price_per_km.ToString().Contains(","))
                carInfoDB.price_per_km = carInfoDB.price_per_km.Replace(",", ".");
            double price = Double.Parse(carInfoDB.price_per_km, CultureInfo.InvariantCulture);
            if (price != 0)
                pricePerKm.Text = price.ToString("0.##");
            else
                pricePerKm.Text = "-";

            cell.Controls.Add(pricePerKm);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();

            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);


            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("15%");

            Label fillUpsInfo = new Label();
            fillUpsInfo.Font.Size = 9;
            fillUpsInfo.Style.Add("margin-left", "10px");
            fillUpsInfo.Style.Add("text-align", "center");
            fillUpsInfo.Width = new Unit("100%");
            fillUpsInfo.Text = "зареждания";

            cell.Controls.Add(fillUpsInfo);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("15%");

            Label minConsInfo = new Label();
            minConsInfo.Font.Size = 9;
            minConsInfo.Style.Add("margin-left", "10px");
            minConsInfo.Style.Add("text-align", "center");
            minConsInfo.Width = new Unit("100%");

            if (car.fuel.Equals("Електричество"))
                minConsInfo.Text = "мин кВч/100 км";
            else
                minConsInfo.Text = "мин л./100 км";

            cell.Controls.Add(minConsInfo);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("15%");

            Label kmInfo = new Label();
            kmInfo.Font.Size = 9;
            kmInfo.Style.Add("margin-left", "10px");
            kmInfo.Style.Add("text-align", "center");
            kmInfo.Width = new Unit("100%");
            kmInfo.Text = "изминати км";

            cell.Controls.Add(kmInfo);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("15%");

            Label pricePerKmInfo = new Label();
            pricePerKmInfo.Font.Size = 9;
            pricePerKmInfo.Style.Add("margin-left", "10px");
            pricePerKmInfo.Style.Add("text-align", "center");
            pricePerKmInfo.Width = new Unit("100%");
            pricePerKmInfo.Text = "цена на км";

            cell.Controls.Add(pricePerKmInfo);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("20%");

            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.Width = new Unit("20%");
            rw.Cells.Add(cell);

            tbl.Controls.Add(rw);

            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();

            rw.Cells.Add(cell);
            tbl.Controls.Add(rw);

            //------------------row-----------------------
            rw = new TableRow();

            //---------------------cell--------------
            cell = new TableCell();
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 2;

            Button buttonAddFuel = new Button();
            buttonAddFuel.Click += new EventHandler(btnAddFuel_Click);
            buttonAddFuel.Width = new Unit("75%");
            buttonAddFuel.Text = "Добави гориво";
            buttonAddFuel.Style.Add("margin-left", "15px");
            buttonAddFuel.Style.Add("margin-right", "15px");
            buttonAddFuel.ID = "Fuel" + car.id.ToString();
            buttonAddFuel.CssClass = "btn";

            //buttonAddFuel 

            cell.Controls.Add(buttonAddFuel);
            rw.Cells.Add(cell);

            //---------------------cell--------------
            cell = new TableCell();
            cell.ColumnSpan = 2;

            Button buttonAddOtherCost = new Button();
            buttonAddOtherCost.Click += new EventHandler(btnAddFuel_Click);
            buttonAddOtherCost.Width = new Unit("75%");
            buttonAddOtherCost.Text = "Добави друг разход";
            buttonAddOtherCost.Style.Add("margin-left", "15px");
            buttonAddOtherCost.Style.Add("margin-right", "15px");
            buttonAddOtherCost.ID = "Other" + car.id.ToString();
            buttonAddOtherCost.CssClass = "btn";

            cell.Controls.Add(buttonAddOtherCost);
            rw.Cells.Add(cell);


            tbl.Controls.Add(rw);

            Panel1.Controls.Add(tbl);
            Panel1.Controls.Add(new LiteralControl("<br />"));
            Panel1.Controls.Add(new LiteralControl("<hr />"));


        }

        private void img_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton img = (ImageButton)sender;
            int car_id = 0;
            string id = img.ID.Replace("Img", "");
            car_id = Int32.Parse(id);
            Application["carID"] = car_id;
            IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/FuelHistory.aspx", Response);
        }

        private void LinkButton1_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            int car_id = 0;
            string id = link.ID.Replace("Title", "");
            car_id = Int32.Parse(id);
            Application["carID"] = car_id;
            IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/FuelHistory.aspx", Response);
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Application["carID"] = "";
            IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertCar.aspx", Response);
        }


        protected void btnAddFuel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = "";
            int car_id = 0;
            if (btn.ID.StartsWith("Fuel"))
            {
                id = btn.ID.Replace("Fuel", "");
                car_id = Int32.Parse(id);
                Application["carID"] = car_id;
                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertFuel.aspx", Response);
            }
            else if (btn.ID.StartsWith("Other"))
            {
                id = btn.ID.Replace("Other", "");
                car_id = Int32.Parse(id);
                Application["carID"] = car_id;
                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertOther.aspx", Response);
            }
            else if (btn.ID.StartsWith("Edit"))
            //извикай insert car с някакъв параметър, чрез който да се заредят данните за дадения автомобил
            {
                id = btn.ID.Replace("Edit", "");
                car_id = Int32.Parse(id);
                Application["carID"] = car_id;
                IdentityHelper.RedirectToReturnUrl(/*Request.QueryString["ReturnUrl"]*/"~/InsertCar.aspx", Response);
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string id = btn.ID.Replace("Delete", "");
            int car_id = Int32.Parse(id);

            if (db.deleteCar(car_id))
                Response.Redirect(Request.RawUrl);
            else
                Response.Write("<script>alert('Грешка при изтриване на автомобил !')</script>");
        }

    }
}