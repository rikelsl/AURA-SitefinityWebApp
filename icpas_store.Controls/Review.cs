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
	public class Review : UserControl
	{
		protected Panel Panel1;
		protected HtmlGenericControl SessionInfo;
		protected HtmlGenericControl AlertWindow;
		protected Label lblerrormsg;
		protected ListView ListViewCart;
		protected HtmlGenericControl divReviewDetails;
		protected Literal PriceSubTotal;
		protected Literal PriceTax;
		protected Literal PriceShipping;
		protected Literal PriceTotal;
		protected HtmlGenericControl paymentSeciton;
		protected HtmlGenericControl billAddress;
		protected Label txtName;
		protected Label txtAddress;
		protected Label txtCity;
		protected Label txtState;
		protected Label txtZip;
		protected RequiredFieldValidator reqCardType;
		protected DropDownList ddlCardType;
		protected RequiredFieldValidator reqCartNumber;
		protected TextBox txtCardNumber;
		protected RequiredFieldValidator reqSecNumber;
		protected TextBox txtCardSecurityNumber;
		protected RequiredFieldValidator reqMonth;
		protected DropDownList ddlMonth;
		protected RequiredFieldValidator reqYear;
		protected DropDownList ddlYear;
		protected Label hiddenCartId;
		protected Button btnSubmit;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
			{
				return;
			}
			if (!this.Page.IsPostBack)
			{
				if (this.LoadCartData())
				{
					this.PopulateDate();
					this.PopulateCards();
					this.CheckSessionData();
					return;
				}
				this.CheckSessionData();
				this.paymentSeciton.Visible = false;
				this.btnSubmit.CausesValidation = false;
			}
		}
		public void CheckSessionData()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartOrderView webShoppingCartOrderView = Helpers.GetOrderDetails(auraId).FirstOrDefault<WebShoppingCartOrderView>();
			if (webShoppingCartOrderView == null || webShoppingCartOrderView.Lines == null)
			{
				return;
			}
			foreach (WebShoppingCartDetails current in webShoppingCartOrderView.Lines)
			{
				MeetingEx meetingByProductId = Helpers.GetMeetingByProductId(current.ProductId.ToString());
				List<MeetingEx> sessionsByMeetingId = Helpers.GetSessionsByMeetingId(meetingByProductId.Id.ToString());
				if (sessionsByMeetingId.Count > 0 && current.SessionRanks.Count == 0)
				{
					this.paymentSeciton.Visible = false;
					this.btnSubmit.Visible = false;
					this.SessionInfo.Attributes.Add("style", "display:visible");
					return;
				}
			}
			this.SessionInfo.Attributes.Add("style", "display:none");
		}
		public void PopulateCards()
		{
			List<WebShoppingCartItemView> source = (List<WebShoppingCartItemView>)base.Session["CartDetails"];
			List<int> obj = (
				from x in source
				select x.ProductId).ToList<int>();
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PaymentType/icpasGetPaymentTypes";
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
			List<PaymentType> dataSource = JsonConvert.DeserializeObject<List<PaymentType>>(restResponse.Content);
			this.ddlCardType.DataSource = dataSource;
			this.ddlCardType.DataBind();
			this.ddlCardType.Items.Insert(0, new ListItem("Card Type", "0"));
		}
		public void PopulateDate()
		{
			List<Param> list = new List<Param>();
			for (int i = 0; i < 9; i++)
			{
				int id = DateTime.Now.Year + i;
				list.Add(new Param
				{
					Id = id,
					Name = id.ToString()
				});
			}
			this.ddlYear.DataSource = list;
			this.ddlYear.DataBind();
			this.ddlYear.Items.Insert(0, new ListItem("Year", "0"));
		}
		public bool LoadCartData()
		{
			decimal num = 0m;
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			WebShoppingCartOrderView details = Helpers.GetOrderDetails(auraId).FirstOrDefault<WebShoppingCartOrderView>();
			if (details == null)
			{
				base.Session["CartDetails"] = null;
				this.billAddress.Visible = false;
			}
			else
			{
				this.billAddress.Visible = true;
				List<WebShoppingCartItemView> list = (
					from item in details.Order.Lines
					let person = Helpers.GetPersonById(item.RequestedRegistrantId, auraId)
					let line = details.Lines.First((WebShoppingCartDetails x) => x.Id == item.RequestedLineId)
					select new WebShoppingCartItemView
					{
						Id = item.RequestedLineId,
						MeetingName = item.Meeting.MeetingTitle,
						ProductId = item.Product.Id,
						Description = Helpers.FormatDescriptionLine(person, line, item),
						Price = item.Price - item.Price * (line.Discount / 100m)
					}).ToList<WebShoppingCartItemView>();
				num = list.Sum((WebShoppingCartItemView x) => x.Price);
				this.PriceSubTotal.Text = num.ToString("C");
				this.PriceShipping.Text = details.Order.ShippingTotal.ToString("C");
				this.PriceTax.Text = details.Order.Tax.ToString("C");
				this.PriceTotal.Text = (num + details.Order.ShippingTotal + details.Order.Tax).ToString("C");
				this.txtName.Text = details.Order.ShipToPerson.FirstName + details.Order.ShipToPerson.LastName;
				this.txtAddress.Text = details.Order.ShippingAddress.Line1 + details.Order.ShippingAddress.Line2 + details.Order.ShippingAddress.Line3 + details.Order.ShippingAddress.Line4;
				this.txtCity.Text = details.Order.ShippingAddress.City;
				this.txtState.Text = details.Order.ShippingAddress.StateProvince;
				this.txtZip.Text = details.Order.ShippingAddress.PostalCode;
				this.hiddenCartId.Text = details.Id.ToString();
				base.Session["CartDetails"] = list;
			}
			this.BindListView();
			return details.Order.Tax + details.Order.ShippingTotal + num > 0m;
		}
		public void ProcessOrder()
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/ProcessShoppingCart";
			WebShoppingCartProcessRequestViewModel obj = new WebShoppingCartProcessRequestViewModel
			{
				PaymentTypeId = string.IsNullOrEmpty(this.ddlCardType.SelectedValue) ? 0 : Convert.ToInt32(this.ddlCardType.SelectedValue),
				CardExpirationMonth = Convert.ToInt32(this.ddlMonth.SelectedValue),
				CardExpirationYear = string.IsNullOrEmpty(this.ddlYear.SelectedValue) ? 0 : Convert.ToInt32(this.ddlYear.SelectedValue),
				SavedShoppingCartId = Convert.ToInt32(this.hiddenCartId.Text),
				CardNumber = this.txtCardNumber.Text,
				CardSvn = this.txtCardSecurityNumber.Text,
				MarketingSourceId = 0
			};
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
			if (restResponse.StatusCode == HttpStatusCode.InternalServerError)
			{
				string str = "";
				if (restResponse.Content.Contains("Credit Card Verification Failed"))
				{
					str = "<br/><br/>The payment save failed: Credit Card Verification Failed. Please check your credit card input.";
				}
				this.AlertWindow.Visible = true;
				this.lblerrormsg.Text = "Error Proccessing Order." + str + "<br/><br/><strong>If you need assistance please send an email to helpdesk@icpas.org or call the ICPAS Member Service center at 800 - 993 - 0407 option 4.</strong>";
				return;
			}
			WebShoppingCartProcessRequestViewModel webShoppingCartProcessRequestViewModel = JsonConvert.DeserializeObject<WebShoppingCartProcessRequestViewModel>(restResponse.Content);
			base.Session["CartDetails"] = null;
			base.Response.Redirect("/CustomerService/OrderConfirmation?id=" + webShoppingCartProcessRequestViewModel.Order.Id);
		}
		private void BindListView()
		{
			List<WebShoppingCartItemView> list = (List<WebShoppingCartItemView>)base.Session["CartDetails"];
			this.ListViewCart.DataSource = list;
			this.ListViewCart.DataBind();
			this.divReviewDetails.Visible = (list != null && list.Count != 0);
		}
		protected void btnSubmit_OnClick(object sender, EventArgs e)
		{
			if (this.GetAccessPassCartValidationResult())
			{
				this.ProcessOrder();
			}
		}
		private bool GetAccessPassCartValidationResult()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				return false;
			}
			string arg_3A_0 = base.Request.QueryString["id"];
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			if (webShoppingCartEx == null)
			{
				return true;
			}
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GetAccessPassCartValidationResult";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.GET);
			restRequest.AddParameter("cartId", webShoppingCartEx.Id);
			IRestResponse restResponse = restClient.Execute(restRequest);
			string text = JsonConvert.DeserializeObject<string>(restResponse.Content);
			if (Helpers.TrimOrNull(text) != "" && !Helpers.TrimOrNull(text).Contains("Error occurred"))
			{
				this.AlertWindow.Visible = true;
				this.AlertWindow.InnerText = text;
				return false;
			}
			return true;
		}
	}
}
