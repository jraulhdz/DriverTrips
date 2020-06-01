using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Kata.Models;
using System.Data;

namespace Kata
{
    public partial class Driver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                //ACCESS THE FILE USING THE NAME OF THE HTML INPUT FILE.
                HttpPostedFile postedFile = Request.Files["FileUpload"];

                //CHECKING IF IT EXISTS
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    //CHANGING FILENAME TO KEEP A TRACK OF FILES UPLOADED
                    string newFileName = DateTime.Now.ToString("MMddyyyy_HHmmss_") + Path.GetFileName(postedFile.FileName);
                    //Save the File.
                    string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["path"]) + newFileName;
                    postedFile.SaveAs(filePath); //SAVING UPLOADED FILE IN ASSIGNED LOCATION
                    lblMessage.Visible = true;
                    driversMPH.Visible = true;
                    Read_File(filePath);

                }
                else { lblMessage.Visible = false; driversMPH.Visible = false; }
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        protected void Read_File(string textFile)
        {
            using (StreamReader file = new StreamReader(textFile))
            {
                List<Trips> trips = new List<Trips>();
                List<string> drivers = new List<string>();
                string ln;

                while ((ln = file.ReadLine()) != null)  //READING ALL LINES
                {
                    ln = ln.ToUpper().Trim();
                    if (ln != "")
                    {
                        if (ln.StartsWith("DRIVER")) //CATCHING ALL DRIVER REGISTERS
                        {
                            if (!drivers.Any(item => item.Equals(ln.Split()[1]))) //CHECKING IF THE DRIVER ALREADY EXISTS
                            {
                                drivers.Add(ln.Split()[1]); //REGISTERING ONLY NAME
                            }
                        }
                        //REGISTER TRIPS OF ONLY REGISTERED DRIVERS, DISCARDING OTHERS
                        if (ln.StartsWith("TRIP") & drivers.Any(item => item.Equals(ln.Split()[1]))) 
                        {
                            //CALC OF DURATION IN HOURS:MINUTES
                            TimeSpan duration = DateTime.Parse(ln.Split(' ')[3]).Subtract(DateTime.Parse(ln.Split(' ')[2]));
                            //CALC OF AVG MPH
                            int mph = (int)Math.Round(Convert.ToSingle(ln.Split(' ')[4]) / (Convert.ToSingle(duration.TotalSeconds) / 60 / 60),0);     
                            //CREATING A NEW TRIP
                            trips.Add(new Trips(ln.Split(' ')[1], Convert.ToInt32(ln.Split(' ')[4]), mph));
                        }
                        
                    }
                }
                file.Close();
                Calc_Trips(drivers, trips);
            }
        }

        protected void Calc_Trips(List<String> listDrivers, List<Trips> listTrips)
        {
            DataTable dt = new DataTable();
            DataView dv;
            float totDist = 0, avgMph = 0;
            dt.Clear();
            dt.Columns.Add("driver");
            dt.Columns.Add("distance");
            dt.Columns.Add("mph");
            
            foreach(String driver in listDrivers) //CHECKING TRIPS FOR EVERY REGISTERED DRIVER
            {
                if (listTrips.Any(item => item.driver.Equals(driver))) //IF DRIVER HAS TRIPS
                {
                    List<Trips> tempList = listTrips.FindAll(item => item.driver.Equals(driver)); //LOOKING FOR THOSE TRIPS
                        foreach (Trips trip in tempList) //ADDING EACH TRIP DISTANCE ANG MPH
                        {
                            totDist += trip.distance;
                            avgMph += trip.mph;
                        }

                        avgMph = (float)Math.Round(avgMph / tempList.Count,0); //CALCULATE AVG MPH
                        if (avgMph > 0 & avgMph < 100) //IF ITS > 0 OR < 100 IT ADDS DRIVER TO REPORT  
                        {
                            dt.Rows.Add(driver, totDist, avgMph);
                        }
                        else { //IF NOT ADD THE REGISTERED DRIVER BUT NOT COUNTRING TRIPS DUE TO 
                         dt.Rows.Add(driver, 0, 0); //AVG MPH < 0 OR AVG MPH > 100
                        }  

                        totDist = 0; //RESET TOTAL DISTANCE AND AVG MPH VARIABLE FOR NEXT DRIVER
                        avgMph = 0;
                }
                else //IF DRIVER HAS NO TRIPS, ADD DRIVER TO REPORT WITH 0 MILES AND 0 AVG SPEED
                {
                    dt.Rows.Add(driver, 0, 0);
                }
            }
            dv = dt.DefaultView;
            dv.Sort = "mph DESC"; //SORTING REPORT BY MPH
            dt = dv.ToTable();
            driversMPH.DataSource = dt;
            driversMPH.DataBind();
        }
    }
}