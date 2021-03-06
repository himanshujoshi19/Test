﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using KoDTicketing.BusinessLayer;
using KoDTicketingLibrary.DTO;
using KoDUtilities;

public partial class MMTPromotion_Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsgPromotionCode.Visible = false;
        Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write("MMT promotion start");
        if (!IsPostBack)
        {
            List<KoDTicketingLibrary.DTO.Promotion> listPromo1 = VistaBOL.GetPromostionCode();
            int cont = listPromo1.Count;
            for (int i = 0; i < cont; i++)
            {
                if (listPromo1[i].PromotionCode.ToString().ToUpper() == "MMT")
                {
                    string date1 = "";
                    //string endDat = "16 Aug 2013";
                    DateTime Enddate = listPromo1[i].EndDate;
                    Enddate = Enddate.AddDays(11);
                    double days = Enddate.Subtract(DateTime.Now).TotalDays;
                    for (double j = 0; j <= days; j++)
                    {
                        DateTime date = DateTime.Now.AddDays(j);
                        string day = date.DayOfWeek.ToString().ToUpper();
                        date1 = date.ToString("ddd, MMM dd,yyyy");
                        if (day == "MONDAY")
                        {
                            ddl_date.Items.Remove(date1);
                        }
                        else
                        {
                            ddl_date.Items.Add(date1);
                        }
                    }
                }
            }
            
            
        }
        
        
    }
    private void PromotionNotValid()
    {
        lblMsgPromotionCode.Visible = true;
        lblMsgPromotionCode.Text = "Either Promotion Code or PNR Number is not Valid";
        txtMMTPromotionCode.Text = "";
        txtMMTPNR.Text = "";
        txtMMTPromotionCode.Focus();
    }
    

    protected void btnMMTPromotionCode_Click(object sender, EventArgs e)
    {

        List<KoDTicketingLibrary.DTO.Promotion> listPromo = VistaBOL.GetPromostionCode();
        string sum=GTICKBOL.MMTpackage_check(txtMMTPNR.Text);
        String PNRnumber = txtMMTPNR.Text;
        String PromotionCode = txtMMTPromotionCode.Text;
        int list = listPromo.Count;
        int no=int.Parse(sum);
        int select = int.Parse(ddl_noofpackage.SelectedValue.ToString());
        if (no >=4)
        {
            lblMsgPromotionCode.Visible = true;
            lblMsgPromotionCode.Text = "Sorry, maximum 4 bookings are allowed on each Booking Id.";
            txtMMTPromotionCode.Text = "";
            txtMMTPNR.Text = "";
            txtMMTPromotionCode.Focus();
            return;

        }
        else if((no+select)>4)
        {
            lblMsgPromotionCode.Visible = true;
            lblMsgPromotionCode.Text = "Sorry, you can select only " + (4 - no) + " package"; ;
            txtMMTPromotionCode.Text = "";
            txtMMTPNR.Text = "";
            txtMMTPromotionCode.Focus();
            return;
        
        }
        else if (txtMMTPromotionCode.Text.ToString().Length == 0)
        {
            lblMsgPromotionCode.Visible = true;
            lblMsgPromotionCode.Text = "Please Enter Your Promotion Code";
            txtMMTPromotionCode.Focus();
            return;
        }
        else if (txtMMTPNR.Text == "")
        {
            lblMsgPromotionCode.Visible = true;
            lblMsgPromotionCode.Text = "Please Enter Your PNR Number";
            txtMMTPNR.Focus();
            return;
        }
        else
        {
            
            for (int i = 0; i < list; i++)
            {
                bool isMatchPromo = Regex.IsMatch(PromotionCode, listPromo[i].RegexValidator);
                bool isMatch = Regex.IsMatch(PNRnumber, "(?![U|u][S|s][0]{4}[B|b][0]{5})^([U|u][S|s][0-9]{4}[B|b]{1}[0-9]{5})$");
                bool isMatch1 = Regex.IsMatch(PNRnumber, "(?![A|a][N|n][0]{12})^([A|a][N|n])[0-9]{12}$");
                if ((isMatch||isMatch1) && isMatchPromo)
                {
                    if (listPromo[i].PromotionCode.ToString().ToUpper() == "MMT")
                    {
                        decimal TotalAmount = Convert.ToDecimal(ddl_noofpackage.SelectedValue) * Convert.ToDecimal("6000");
                        decimal PayableAmount = Convert.ToDecimal("4500") * Convert.ToDecimal(ddl_noofpackage.SelectedValue);
                        Session["PayableAmount"] = PayableAmount;
                        Session["TotalAmount"] = TotalAmount;
                        Session["NoofPackages"] = ddl_noofpackage.SelectedValue.ToString();
                        Session["pnr"] = txtMMTPNR.Text.ToUpper();
                        Session["promocode"] =txtMMTPromotionCode.Text.ToUpper();
                        Session["day"] = ddl_date.SelectedItem.Text.ToString();
                        Response.Redirect("MMTContactDetails.aspx");
                    }
                }
            }
        }
        PromotionNotValid();
    }
}