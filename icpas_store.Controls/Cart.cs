using icpas_store.Controls.General;
using icpas_store.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace icpas_store.Controls
{
	public class Cart : UserControl
	{
		protected HtmlGenericControl AlertWindow;
		protected ListView ListViewCart;
		protected HtmlGenericControl divUpdateCart;
		protected LinkButton btnUpdate;
		protected HtmlGenericControl divPrices;
		protected Literal PriceSubTotal;
		protected Literal PriceTax;
		protected Literal PriceShipping;
		protected Literal PriceTotal;
		protected HtmlGenericControl divCheckout;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
			{
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.LoadCartData();
			}
		}
		public void LoadCartData()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartOrderView details = Helpers.GetOrderDetails(auraId).FirstOrDefault<WebShoppingCartOrderView>();
			base.Session["Cart"] = details;
			if (details == null)
			{
				Helpers.NewCart(auraId);
				base.Session["CartDetails"] = null;
				base.Session["Cart"] = null;
			}
			else
			{
				List<WebShoppingCartItemView> list = (
					from item in details.Order.Lines
					let person = Helpers.GetPersonById(item.RequestedRegistrantId, auraId)
					let line = details.Lines.First((WebShoppingCartDetails x) => x.Id == item.RequestedLineId)
					select new WebShoppingCartItemView
					{
						Id = item.RequestedLineId,
						MeetingName = item.Meeting.MeetingTitle,
						ProductId = item.Product.Id,
						MeetingId = item.Meeting.Id,
						PersonId = item.RequestedRegistrantId,
						Price = item.Price - item.Price * (line.Discount / 100m),
						ClassPass = line.ClassPassCardApplied,
						PromoCode = line.CouponPromotionalCode,
						CLEId = item.Meeting.cleProductId,
						SessionCount = item.Meeting.SessionCount,
						AllowGuests = Helpers.IsGuestRegistrationAvailable(item.Product.Id.ToString(), auraId),
						Description = Helpers.FormatDescriptionLine(person, line, item)
					}).ToList<WebShoppingCartItemView>();
				HashSet<int> hashSet = new HashSet<int>();
				foreach (WebShoppingCartItemView current in list)
				{
					if (hashSet.Contains(current.Id))
					{
						current.IsSession = true;
					}
					hashSet.Add(current.Id);
				}
				decimal d = list.Sum((WebShoppingCartItemView x) => x.Price);
				this.PriceSubTotal.Text = d.ToString("C");
				this.PriceShipping.Text = details.Order.ShippingTotal.ToString("C");
				this.PriceTax.Text = details.Order.Tax.ToString("C");
				this.PriceTotal.Text = (d + details.Order.ShippingTotal + details.Order.Tax).ToString("C");
				base.Session["CartDetails"] = list;
			}
			this.BindListView();
			this.CheckSessionData();
		}
		public MeetingEx GetMeetingExById(int id)
		{
			string text = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getbyodatasingle/";
			RestClient restClient = new RestClient(string.Concat(new object[]
			{
				text,
				"?$filter=Product/Id eq '",
				id,
				"'"
			}));
			RestRequest restRequest = new RestRequest(Method.GET);
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			restRequest.AddHeader("x-aura-token", auraId);
			IRestResponse restResponse = restClient.Execute(restRequest);
			return JsonConvert.DeserializeObject<MeetingEx>(restResponse.Content);
		}
		public void DeleteCurrentCart()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/Delete/{id}";
			List<WebShoppingCartEx> cart = Helpers.GetCart(auraId);
			if (cart == null)
			{
				return;
			}
			string value = cart.First<WebShoppingCartEx>().Id.ToString();
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.DELETE);
			restRequest.AddHeader("x-aura-token", auraId);
			restRequest.AddUrlSegment("id", value);
			restClient.Execute(restRequest);
		}
		public void RemoveRow(string id)
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartDetails/Delete/{id}";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.DELETE);
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			restRequest.AddHeader("x-aura-token", auraId);
			restRequest.AddUrlSegment("id", id);
			restClient.Execute(restRequest);
		}
		private void BindListView()
		{
			List<WebShoppingCartItemView> list = (List<WebShoppingCartItemView>)base.Session["CartDetails"];
			this.divPrices.Visible = (list != null && list.Count != 0);
			this.divUpdateCart.Visible = (list != null && list.Count != 0);
			this.divCheckout.Visible = (list != null && list.Count != 0);
			this.ListViewCart.DataSource = list;
			this.ListViewCart.DataBind();
		}
		protected void btnUpdate_OnClick(object sender, EventArgs e)
		{
			WebShoppingCartOrderView webShoppingCartOrderView = (WebShoppingCartOrderView)base.Session["Cart"];
			using (IEnumerator<WebShoppingCartDetails> enumerator = webShoppingCartOrderView.Lines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WebShoppingCartDetails line = enumerator.Current;
					line.Price = webShoppingCartOrderView.Order.Lines.First((OrderLineViewModel x) => x.RequestedLineId == line.Id).Price;
				}
			}
			foreach (ListViewDataItem current in this.ListViewCart.Items)
			{
				CheckBox checkBox = (CheckBox)current.FindControl("Remove");
				int rowId = Convert.ToInt32(checkBox.InputAttributes["rowid"]);
				if (!Convert.ToBoolean(checkBox.InputAttributes["is-session"]))
				{
					if (checkBox.Checked)
					{
						this.RemoveRow(rowId.ToString());
					}
					else
					{
						CheckBox checkBox2 = (CheckBox)current.FindControl("ClassPass");
						webShoppingCartOrderView.Lines.First((WebShoppingCartDetails x) => x.Id == rowId).ClassPassCardApplied = checkBox2.Checked;
						TextBox textBox = (TextBox)current.FindControl("PromoCode");
						if (textBox.Text != string.Empty)
						{
							webShoppingCartOrderView.Lines.First((WebShoppingCartDetails x) => x.Id == rowId).CouponPromotionalCode = textBox.Text;
						}
					}
				}
			}
			WebShoppingCartEx obj = webShoppingCartOrderView;
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/UpdateDiscount";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.POST);
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			restRequest.AddHeader("x-aura-token", auraId);
			restRequest.RequestFormat = DataFormat.Json;
			restRequest.AddBody(obj);
			IRestResponse restResponse = restClient.Execute(restRequest);
			HttpStatusCode arg_271_0 = restResponse.StatusCode;
			this.LoadCartData();
		}
		protected void ListViewCart_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			if (e.Item.ItemType != ListViewItemType.DataItem)
			{
				return;
			}
			ListViewDataItem listViewDataItem = (ListViewDataItem)e.Item;
			WebShoppingCartItemView webShoppingCartItemView = (WebShoppingCartItemView)listViewDataItem.DataItem;
			CheckBox checkBox = (CheckBox)e.Item.FindControl("Remove");
			Control control = e.Item.FindControl("RemoveDiv");
			Control control2 = e.Item.FindControl("RegistrationDetailsDiv");
			checkBox.InputAttributes.Add("rowid", Convert.ToString(webShoppingCartItemView.Id));
			checkBox.InputAttributes.Add("is-session", Convert.ToString(webShoppingCartItemView.IsSession));
			if (webShoppingCartItemView.IsSession)
			{
				checkBox.Visible = false;
				control.Visible = false;
				control2.Visible = false;
			}
			Control control3 = e.Item.FindControl("ClassPassListItem");
			if (!Helpers.IsClassPassCardVisible(webShoppingCartItemView.PersonId.ToString(), webShoppingCartItemView.ProductId.ToString()))
			{
				control3.Visible = false;
			}
		}
		public void CheckSessionData()
		{
			this.AlertWindow.Visible = false;
			WebShoppingCartEx webShoppingCartEx = (WebShoppingCartEx)base.Session["Cart"];
			if (webShoppingCartEx == null || webShoppingCartEx.Lines == null)
			{
				return;
			}
			foreach (WebShoppingCartDetails current in webShoppingCartEx.Lines)
			{
				MeetingEx meetingByProductId = Helpers.GetMeetingByProductId(current.ProductId.ToString());
				List<MeetingEx> sessionsByMeetingId = Helpers.GetSessionsByMeetingId(meetingByProductId.Id.ToString());
				if (sessionsByMeetingId.Count > 0 && current.SessionRanks.Count == 0)
				{
					this.divCheckout.Visible = false;
					this.AlertWindow.Visible = true;
					return;
				}
			}
			List<WebShoppingCartItemView> list = (List<WebShoppingCartItemView>)base.Session["CartDetails"];
			this.divCheckout.Visible = (list != null && list.Count != 0);
		}
		protected void btnCLE_OnClick(object sender, EventArgs e)
		{
			LinkButton linkButton = (LinkButton)sender;
			string value = linkButton.CommandArgument.Split(new char[]
			{
				','
			})[0];
			int associatedId = Convert.ToInt32(linkButton.CommandArgument.Split(new char[]
			{
				','
			})[1]);
			string commandName = linkButton.CommandName;
			if (string.IsNullOrEmpty(commandName) || string.IsNullOrEmpty(value))
			{
				return;
			}
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			if (webShoppingCartEx == null)
			{
				Helpers.NewCart(auraId);
				webShoppingCartEx = new WebShoppingCartEx();
			}
			Helpers.AddToCart(webShoppingCartEx, Convert.ToInt32(commandName), Convert.ToInt32(value), auraId, null, associatedId, "", "");
			base.Response.Redirect(base.Request.RawUrl);
		}
	}
}
