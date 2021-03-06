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
    Page_Manage ac = new Page_Manage();
    Page_ManageDatum dm = new Page_ManageDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("PageManage", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();
        }
    }


    private void FillGrid()
    {
        dm.Place = DropDownList1.SelectedValue;
        GridView1.DataSource = ac.Select(dm);
        GridView1.DataBind();
    }
    private void Cancel()
    {
        GridView1.SelectedIndex = -1;
        TextTitle.Text = "";
        TextBox1.Text = "";
        TextBox2.Text = "";

        TextTitle.Focus();
        CheckBox1.Checked = true;
        FCKeditor1.Value = "";
        LblHidden.ToolTip = "";
        Button1.Text = "ثبت";
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        int i = 0;
        for (i = 0; i < GridView1.Rows.Count; i++)
        {
            dm.Row_View = decimal.Parse(((TextBox)(GridView1.Rows[i].FindControl("txtrowview"))).Text);
            dm.Id = decimal.Parse(((Label)(GridView1.Rows[i].FindControl("Lblid"))).Text);
            ac.Update_Row_View(dm);
        }

        FillGrid();

        for (i = 0; i < GridView1.Rows.Count; i++)
        {
            dm.Row_View = decimal.Parse(((TextBox)(GridView1.Rows[i].FindControl("txtrowview"))).Text);

            if (ac.Check_Row_View_Exist(dm))
            {
                ((TextBox)(GridView1.Rows[i].FindControl("txtrowview"))).BackColor = System.Drawing.Color.Gray;
            }
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        dm.Title = TextTitle.Text.Trim();
        dm.Text = FCKeditor1.Value;
        PublicClass p = new PublicClass();
        dm.Date_Send = p.GetDate();
        dm.Id_Admin = 1;
        dm.Show_Page = CheckBox1.Checked.ToString();
        dm.MetaKeyword = TextBox1.Text;
        dm.MetaDescription = TextBox2.Text;
        dm.Place = DropDownList1.SelectedValue;
        if (LblHidden.ToolTip.Length > 0)
        {
            dm.Id = decimal.Parse(LblHidden.ToolTip);
            ac.Update(dm);
        }
        else
        {

            dm.Row_View = ac.SelectMaxRowview(dm);

            ac.Insert(dm);
        }
        FillGrid();
        Cancel();
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
        dm.Id = decimal.Parse(((Label)(GridView1.Rows[e.NewEditIndex].FindControl("lblID"))).Text);
        DataTable dt = ac.Select_page_One(dm);
        TextTitle.Text = dt.Rows[0]["Title"].ToString();
        TextBox1.Text = dt.Rows[0]["MetaKeyword"].ToString();
        TextBox2.Text = dt.Rows[0]["MetaDescription"].ToString();
        FCKeditor1.Value = dt.Rows[0]["text"].ToString();
        CheckBox1.Checked = bool.Parse(dt.Rows[0]["Show_Page"].ToString());
        LblHidden.ToolTip = dm.Id.ToString();
        GridView1.SelectedIndex = e.NewEditIndex;
        Button1.Text = "ویرایش";

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        FillGrid();
        Cancel();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}
