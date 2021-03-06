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
    user ac = new user();
    userDatum dm = new userDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("UserManage", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();

            if (Request.QueryString["ID_User"] != null)
            {
                FillUser(decimal.Parse(Request.QueryString["ID_User"]));
            }
        }
    }


    private void FillGrid()
    {                
        GridView1.DataSource = ac.Select_All(dm);        
        GridView1.DataBind();
        //excel out
        DataTable dt = (DataTable)GridView1.DataSource;
        new DAL.WriteToExcel().WriteTable(dt, Server.MapPath(@"~/Administrator/files/ExcelOut"));
    }
    private void Cancel()
    {
        GridView1.SelectedIndex = -1;
        LblHidden.ToolTip = "";
        Button1.Text = "ثبت";
        Label18.Visible = false;
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        TextBox6.Text = "";
        TextBox7.Text = "";
        TextBox8.Text = "";
        TextBox9.Text = "";       
        DropDownList1.Text = "";
        TextBox10.Text = "";
        TextBox11.Text = "";
        DropDownList2.SelectedIndex = -1;
        CheckBox1.Checked = false;
        TextBox12.Text = "";
        RequiredFieldValidator1.ValidationGroup = "ok";
        Label3.Visible = false;

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        dm.Email = TextBox1.Text;
        dm.Name = TextBox4.Text;
        dm.Family = TextBox5.Text;
        dm.Tell = TextBox6.Text;
        dm.Mobile = TextBox7.Text;
        dm.Post_Code = TextBox8.Text;
        dm.Country = TextBox9.Text;
        dm.Province = DropDownList1.Text;
        dm.City = TextBox10.Text;
        dm.Address = TextBox11.Text;
        dm.How_Find = DropDownList2.Text;

        dm.Recive_News = CheckBox1.Checked.ToString();

        if (TextBox2.Text.Trim().Length > 0)
        {
            dm.Pass = TextBox2.Text;
        }
        else
        {
            dm.Pass = HiddenField1.Value;
        }
        dm.Company = TextBox12.Text;

        if (LblHidden.ToolTip.Length > 0)
        {
            if (ac.CheckUserExist(dm) && HiddenField2.Value != TextBox1.Text)
            {
                Label18.Visible = true;
                Label3.Visible = false;
            }
            else
            {
                dm.Id = decimal.Parse(LblHidden.ToolTip);
                ac.Update_User(dm);
                FillGrid();
                Cancel();
                Label3.Visible = true;

            }
        }
        else
        {
            if (ac.CheckUserExist(dm))
            {
                Label18.Visible = true;
                Label3.Visible = false;
            }
            else
            {
                ac.Insert(dm);
                FillGrid();
                Cancel();
                Label3.Visible = true;
            }

        }
    }

    //---------------------------------------Delete ---------------------------------------------------
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = ((Label)(GridView1.Rows[e.RowIndex].FindControl("Lblid"))).Text;
        dm.Id = decimal.Parse(id);
        ac.Delete(dm);
        Cancel();
        FillGrid();

    }
    //----------------------------------------------------------------------------------------------

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        FillUser(decimal.Parse(((Label)(GridView1.Rows[e.NewEditIndex].FindControl("lblID"))).Text));
        GridView1.SelectedIndex = e.NewEditIndex;

    }
    void FillUser(decimal id_user)
    {
        dm.Id = id_user;
        DataTable dt = ac.select_one_user(dm);
        
        LblHidden.ToolTip = dm.Id.ToString();
        Button1.Text = "ویرایش";

        TextBox2.Text = dt.Rows[0]["pass"].ToString();
        TextBox3.Text = dt.Rows[0]["pass"].ToString();
        TextBox4.Text = dt.Rows[0]["Name"].ToString();
        TextBox5.Text = dt.Rows[0]["Family"].ToString();
        TextBox6.Text = dt.Rows[0]["Tell"].ToString();
        TextBox7.Text = dt.Rows[0]["Mobile"].ToString();
        TextBox8.Text = dt.Rows[0]["Post_Code"].ToString();
        TextBox9.Text = dt.Rows[0]["Country"].ToString();
        DropDownList1.Text = dt.Rows[0]["Province"].ToString();
        TextBox10.Text = dt.Rows[0]["City"].ToString();
        TextBox11.Text = dt.Rows[0]["Address"].ToString();
        DropDownList2.Text = dt.Rows[0]["How_Find"].ToString();

        CheckBox1.Checked = bool.Parse(dt.Rows[0]["Recive_News"].ToString());

        TextBox2.Text = dt.Rows[0]["Pass"].ToString();
        HiddenField1.Value = dt.Rows[0]["Pass"].ToString();

        TextBox12.Text = dt.Rows[0]["Company"].ToString();

        TextBox1.Text = dt.Rows[0]["Email"].ToString();
        HiddenField2.Value = dt.Rows[0]["Email"].ToString();
        RequiredFieldValidator1.ValidationGroup = "No";

        Label3.Visible = false;

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        FillGrid();
        Cancel();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex != -1)
        {

            string id = ((Label)(e.Row.FindControl("Lblid"))).Text;
            Shoping ac2 = new Shoping();
            ShopingDatum dm2 = new ShopingDatum();
                        Support_Comment ac3 = new Support_Comment();
            Support_CommentDatum dm3 = new Support_CommentDatum();
            dm3.ID_User = decimal.Parse(id);
        dm3.SendType = "User";
             
            dm2.Register_Date = "";
            dm2.Id_User = decimal.Parse(id);
            int n=ac2.select_factor_nopeyment(dm2).Rows.Count;
            int p = ac2.select_factor_peyment(dm2).Rows.Count;
            int s = ac2.select_factor_sended(dm2).Rows.Count;
            int sp = ac3.Select(dm3).Rows.Count;
            if (n > 0)
            {
                ((HyperLink)(e.Row.FindControl("HyperLinkn"))).Text = "مشاهده ( " + n.ToString() + " )";
            }
            if (p > 0)
            {
                ((HyperLink)(e.Row.FindControl("HyperLinkp"))).Text = "مشاهده ( " + p.ToString() + " )";
            }
            if (s > 0)
            {
                ((HyperLink)(e.Row.FindControl("HyperLinks"))).Text = "مشاهده ( " + s.ToString() + " )";
            }
            if (sp > 0)
            {
                ((HyperLink)(e.Row.FindControl("HyperLinksp"))).Text = "مشاهده ( " + sp.ToString() + " )";
            }


        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

        dm.Email = TextBox1.Text;
        dm.Name = TextBox4.Text;
        dm.Family = TextBox5.Text;
        dm.Tell = TextBox6.Text;
        dm.Mobile = TextBox7.Text;
        dm.Post_Code = TextBox8.Text;
        dm.Country = TextBox9.Text;
        dm.Province = DropDownList1.Text;
        dm.City = TextBox10.Text;
        dm.Address = TextBox11.Text;
        dm.How_Find = DropDownList2.Text;
        dm.Recive_News = CheckBox1.Checked.ToString();      
        dm.Company = TextBox12.Text;

        
        GridView1.DataSource = ac.Find_All(dm);        
        GridView1.DataBind();
        //excel out
        DataTable dt = (DataTable)GridView1.DataSource;
        new DAL.WriteToExcel().WriteTable(dt, Server.MapPath(@"~/Administrator/files/ExcelOut"));
    }
    protected void btnClean_Click(object sender, EventArgs e)
    {

        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        TextBox6.Text = "";
        TextBox7.Text = "";
        TextBox8.Text = "";
        TextBox9.Text = "ایران";        
        TextBox10.Text = "";
        TextBox11.Text = "";
        TextBox12.Text = "";
        DropDownList1.SelectedIndex = 0;
        DropDownList2.SelectedIndex = 0;
        CheckBox1.Checked = false;
        dm = new userDatum();
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Comment")
        {
            Response.Redirect("~/Administrator/index.aspx?Type=UserSupportComment&ID_User="+ e.CommandArgument);
        }
    }
}
