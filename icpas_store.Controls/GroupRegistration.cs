using icpas_store.Controls.General;
using icpas_store.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace icpas_store.Controls
{
	public class GroupRegistration : UserControl
	{
		protected HtmlGenericControl AlertWindow;
		protected ListView ListViewPeople;
		protected LinkButton btnProceed;
		protected HiddenField cartCount;
		protected HiddenField maxCartCount;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
			{
				return;
			}
			if (!this.Page.IsPostBack)
			{
				string personId = Helpers.GetPersonId(Convert.ToString(base.Session["PersonID"]));
				string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
				if (string.IsNullOrEmpty(auraId))
				{
					base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
				}
				string value = base.Request.QueryString["id"];
				if (string.IsNullOrEmpty(value))
				{
					throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
				}
				List<FirmAdminRegistrants> dataSource = Helpers.LoadFirmAdminRegistrants(personId, auraId);
				this.ListViewPeople.DataSource = dataSource;
				this.ListViewPeople.DataBind();
				this.CheckCartCount(0);
			}
		}
		public void CheckCartCount(int additionalCount = 0)
		{
			this.AlertWindow.Attributes.Add("style", "display:none");
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				return;
			}
			List<WebShoppingCartEx> cart = Helpers.GetCart(auraId);
			if (cart == null || cart.FirstOrDefault<WebShoppingCartEx>() == null)
			{
				return;
			}
			int num = Convert.ToInt32(ConfigurationManager.AppSettings["MaxCartCount"]);
			int count = cart.FirstOrDefault<WebShoppingCartEx>().Lines.Count;
			this.cartCount.Value = count.ToString();
			this.maxCartCount.Value = num.ToString();
			if (count + additionalCount >= num)
			{
				this.AlertWindow.Attributes.Add("style", "display:visible");
			}
		}
		protected void btnProceed_OnClick(object sender, EventArgs e)
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			string value = base.Request.QueryString["id"];
			if (string.IsNullOrEmpty(value))
			{
				throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
			}
			int detailId = Convert.ToInt32(value);
			WebShoppingCartEx cart = this.Setup_Cart(auraId);
			foreach (ListViewDataItem current in this.ListViewPeople.Items)
			{
				CheckBox checkBox = (CheckBox)current.FindControl("PersonCheck");
				int num = Convert.ToInt32(checkBox.InputAttributes["rowid"]);
				if (checkBox.Checked && num > 0)
				{
					Helpers.AddToCart(cart, num, detailId, auraId, null, 0, "", "");
				}
			}
			base.Response.Redirect("/ProductCatalog/ViewCart");
		}
		public WebShoppingCartEx Setup_Cart(string auraId)
		{
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			if (webShoppingCartEx != null)
			{
				return webShoppingCartEx;
			}
			Helpers.NewCart(auraId);
			return new WebShoppingCartEx();
		}
		protected void ListViewPeople_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			if (e.Item.ItemType != ListViewItemType.DataItem)
			{
				return;
			}
			ListViewDataItem listViewDataItem = (ListViewDataItem)e.Item;
			FirmAdminRegistrants firmAdminRegistrants = (FirmAdminRegistrants)listViewDataItem.DataItem;
			CheckBox checkBox = (CheckBox)e.Item.FindControl("PersonCheck");
			if (string.IsNullOrEmpty(firmAdminRegistrants.FirstLast))
			{
				e.Item.Visible = false;
			}
			checkBox.InputAttributes.Add("rowid", Convert.ToString(firmAdminRegistrants.Id));
		}
	}
}
