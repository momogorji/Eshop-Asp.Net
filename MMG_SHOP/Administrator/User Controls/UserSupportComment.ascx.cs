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
    Support_Comment ac = new Support_Comment();
    Support_CommentDatum dm = new Support_CommentDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("UserManage", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid("User");
        }
    }

    private void FillGrid(string SendType)
    {
        dm.ID_User = decimal.Parse(Request.QueryString["ID_User"]);
        dm.SendType = SendType;
        if (SendType == "Admin")
        {
            LinkButton1.Enabled = false;
            LinkButton2.Enabled = true;
        }
        else if (SendType == "User")
        {
            LinkButton2.Enabled = false;
            LinkButton1.Enabled = true;

        }
        GridView1.DataSource = ac.Select(dm);
        GridView1.DataBind();
        HiddenField1.Value = SendType;
    }

    //---------------------------------------Delete ---------------------------------------------------
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)(GridView1.Rows[e.RowIndex].FindControl("Lblid"))).Text;
        dm.Id = decimal.Parse(id);
        ac.Delete(dm);
        Cancel();
        FillGrid(HiddenField1.Value);


    }
    //----------------------------------------------------------------------------------------------

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        Cancel();
        FillGrid(HiddenField1.Value);

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        FillGrid(HiddenField1.Value);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Cancel();
        FillGrid("Admin"); 

    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
         Cancel();
       FillGrid("User"); 

    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        dm.ID_User = decimal.Parse(Request.QueryString["ID_User"]);
        dm.Title = TextBox1.Text;
        dm.Text = TextBox11.Text;
        PublicClass p = new PublicClass();
        dm.Date_send = p.GetDate();
        dm.SendType = "Admin";
        ac.Insert(dm);
        Cancel();
        FillGrid("Admin");

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    void Cancel()
    {
        TextBox1.Text = "";
        TextBox11.Text = "";
        GridView1.EditIndex = -1;
        FillGrid(HiddenField1.Value);

    }
}
