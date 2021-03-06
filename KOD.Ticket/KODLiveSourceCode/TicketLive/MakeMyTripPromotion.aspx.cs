﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using KoDTicketing.BusinessLayer;

public partial class MakeMyTripPromotion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsgPromotionCode.Visible = false;
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
         String PNRnumber = txtMMTPNR.Text;
         String PromotionCode = txtMMTPromotionCode.Text;
         int list = listPromo.Count;
         if (txtMMTPromotionCode.Text.ToString().Length == 0)
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
                 bool isMatch = Regex.IsMatch(PNRnumber, "(?![N|n][P|p][0]{12})^([N|n][P|p])[0-9]{12}$");
                 bool isMatch2 = Regex.IsMatch(PNRnumber, "(?![N|n][H|h][0]{12})^([N|n][H|h])[0-9]{12}$");
                 if ((isMatch || isMatch2) && isMatchPromo)
                 {
                     if (listPromo[i].PromotionCode.ToString().ToUpper() == "MMTDOMESTIC")
                     {
                         listPromo[i].WebPromotionId = txtMMTPromotionCode.Text.ToUpper();
                         Session["PromotionCode"] = listPromo[i];
                         Response.Redirect("Default.aspx?MMTD=s");
                     }
                 }
             }
             PromotionNotValid();
         }
     }
}