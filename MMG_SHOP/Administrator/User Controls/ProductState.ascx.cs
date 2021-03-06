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
    Product_State ac = new Product_State();
    Product_StateDatum dm = new Product_StateDatum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!new Admin().CheckSecurity("ProductState", decimal.Parse(Request.Cookies["ID_Admin"].Value)))
            {
                Response.Redirect("~/Administrator/index.aspx?Type=Accessdenied");
            }

            FillGrid();
        }
    }


    private void FillGrid()
    {
        GridView1.DataSource = ac.Select_All();
        GridView1.DataBind();
    }
    private void Cancel()
    {
        GridView1.SelectedIndex = -1;
        TextTitle.Text = "";
        TextTitle.Focus();
        LblHidden.ToolTip = "";
        Button1.Text = "ثبت";
        Label3.Visible = false;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        Cancel();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        dm.Title = TextTitle.Text.Trim();
        if (LblHidden.ToolTip.Length > 0)
        {
            dm.Id = decimal.Parse(LblHidden.ToolTip);
            ac.Update(dm);
        }
        else
        {
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
        Product ac2 = new Product();
        ProductDatum dm2 = new ProductDatum();
        dm2.Id_State = dm.Id;
        DataTable dt= ac2.Select_Product_State(dm2);
        if (dt.Rows.Count == 0)
        {
            ac.Delete(dm);
            Cancel();
            FillGrid();
        }
        else
        {
            Label3.Visible = true;
        }

    }
    //----------------------------------------------------------------------------------------------

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dm.Id = decimal.Parse(((Label)(GridView1.Rows[e.NewEditIndex].FindControl("lblID"))).Text);
        DataTable dt = ac.SelectOne(dm);
        TextTitle.Text = dt.Rows[0]["Title"].ToString();
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

}
