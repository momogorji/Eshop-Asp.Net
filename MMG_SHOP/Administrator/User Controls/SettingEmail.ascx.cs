using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BLL;
using Common;

public partial class Administrator_User_Controls_MainMenu : System.Web.UI.UserControl
{
    OneRecord ac = new OneRecord();
    OneRecordDatum dm = new OneRecordDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("Setting", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }
            Fill();
        }
    }

    private void Fill()
    {
        DataTable dt;
        dm.Type = "EmailRegisterUser";
        dt= ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox1.Text = dt.Rows[0]["title"].ToString();
            FCKeditor1.Value = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailDisplay_Name";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox2.Text = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailFrom_Email_Address";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox3.Text = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailSmtp_Host";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox4.Text = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailSender_Email_Address";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox5.Text = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailSender_Email_Pass";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox6.Text = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailSendFactor";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox7.Text = dt.Rows[0]["title"].ToString();
            FCKeditor2.Value = dt.Rows[0]["text"].ToString();
        }
        dm.Type = "EmailForgetPassword";
        dt = ac.SelectOne(dm);
        if (dt.Rows.Count > 0)
        {
            TextBox8.Text = dt.Rows[0]["title"].ToString();
            FCKeditor3.Value = dt.Rows[0]["text"].ToString();
        }
    }
    private void Cancel()
    {
        Fill();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        dm.Title = TextBox1.Text;
        dm.Text = FCKeditor1.Value;
        dm.Type = "EmailRegisterUser";
        ac.Update(dm);
        dm.Title ="EmailSetting";
        dm.Text = TextBox2.Text;
        dm.Type = "EmailDisplay_Name";
        ac.Update(dm);
        dm.Title = "EmailSetting";
        dm.Text = TextBox3.Text;
        dm.Type = "EmailFrom_Email_Address";
        ac.Update(dm);
        dm.Title = "EmailSetting";
        dm.Text = TextBox4.Text;
        dm.Type = "EmailSmtp_Host";
        ac.Update(dm);
        dm.Title = "EmailSetting";
        dm.Text = TextBox5.Text;
        dm.Type = "EmailSender_Email_Address";
        ac.Update(dm);
        dm.Title = "EmailSetting";
        dm.Text = TextBox6.Text;
        dm.Type = "EmailSender_Email_Pass";
        ac.Update(dm);
        dm.Title = TextBox7.Text;
        dm.Text = FCKeditor2.Value;
        dm.Type = "EmailSendFactor";
        ac.Update(dm);
        dm.Title = TextBox8.Text;
        dm.Text = FCKeditor3.Value;
        dm.Type = "EmailForgetPassword";
        ac.Update(dm);
    }
}
