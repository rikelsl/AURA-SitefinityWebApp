using icpas_store.Controls.General;
using icpas_store.Models;
using Newtonsoft.Json;
using RestSharp;
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
	public class Confirmation : UserControl
	{
		protected Literal txtOrderNumberTitle;
		protected Literal txtMeetingPhone;
		protected Literal txtMeetingFax;
		protected Literal txtOrderNumber;
		protected Literal txtCustomerNumber;
		protected HtmlGenericControl PaymentSection;
		protected Literal txtCardType;
		protected Literal txtLastFour;
		protected HtmlGenericControl billAddressHeader;
		protected HtmlGenericControl billAddress;
		protected Literal txtBillingName;
		protected Literal txtBillingLine1;
		protected Literal txtBillingLine2;
		protected Literal txtBillingCity;
		protected Literal txtBillingState;
		protected Literal txtBillingZip;
		protected HtmlGenericControl shipAddressHeader;
		protected HtmlGenericControl shipAddress;
		protected Literal txtShippingName;
		protected Literal txtShippingLine1;
		protected Literal txtShippingLine2;
		protected Literal txtShippingCity;
		protected Literal txtShippingState;
		protected Literal txtShippingZip;
		protected ListView ListViewCart;
		protected HtmlGenericControl divPrices;
		protected Literal PriceSubTotal;
		protected Literal PriceShipping;
		protected Literal PriceTax;
		protected Literal PriceTotal;
		protected Literal PaymentTotal;
		protected Literal PriceBalance;
		protected HtmlAnchor lnkCourseCatalog;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
			{
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.LoadOrderData();
			}
		}
		public void LoadOrderData()
		{
			string text = base.Request.QueryString["id"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
			}
			Order order = Helpers.GetOrderById(text);
			string personId = Helpers.GetPersonId(Convert.ToString(base.Session["PersonID"]));
			if (string.IsNullOrEmpty(personId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			bool flag = order.BillToPerson.Id == Convert.ToInt32(personId);
			if (!flag && order.ShipToPerson.Id == Convert.ToInt32(personId))
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			this.txtOrderNumberTitle.Text = order.Id.ToString();
			this.txtOrderNumber.Text = order.Id.ToString();
			if (order.BillToPerson != null)
			{
				this.txtCustomerNumber.Text = order.BillToPerson.Id.ToString();
			}
			else
			{
				if (order.ShipToPerson != null)
				{
					this.txtCustomerNumber.Text = order.ShipToPerson.Id.ToString();
				}
			}
			List<PaymentType> cards = this.GetCards();
			if (order.PaymentTypeId == 0 && order.CardNumber == null)
			{
				this.PaymentSection.Visible = false;
			}
			else
			{
				PaymentType paymentType = cards.FirstOrDefault((PaymentType x) => x.Id == order.PaymentTypeId);
				this.txtCardType.Text = ((paymentType == null) ? "None" : paymentType.Name.Substring(0, paymentType.Name.IndexOf("-", StringComparison.Ordinal) - 1));
				if (order.CardNumber != null && order.CardNumber.Length > 3)
				{
					this.txtLastFour.Text = "ending in " + order.CardNumber.Substring(order.CardNumber.Length - 4);
				}
			}
			if (order.BillToPerson != null)
			{
				Address billingAddress = order.BillingAddress;
				if (order.BillToCompany != null)
				{
					this.txtBillingName.Text = string.Concat(new string[]
					{
						order.BillToPerson.FirstName.ToString(),
						" ",
						order.BillToPerson.LastName.ToString(),
						" / ",
						order.BillToCompany.Name.ToString()
					});
				}
				else
				{
					this.txtBillingName.Text = order.BillToPerson.FirstName.ToString() + " " + order.BillToPerson.LastName.ToString();
				}
				this.txtBillingLine1.Text = billingAddress.Line1;
				this.txtBillingLine2.Text = billingAddress.Line2;
				this.txtBillingCity.Text = billingAddress.City;
				this.txtBillingState.Text = billingAddress.StateProvince;
				this.txtBillingZip.Text = billingAddress.PostalCode;
			}
			else
			{
				this.billAddress.Visible = false;
				this.billAddressHeader.Visible = false;
			}
			if (order.ShipToPerson != null && order.ShipToPerson.HomeAddress != null)
			{
				Address shippingAddress = order.ShippingAddress;
				if (order.ShipToCompany != null)
				{
					this.txtShippingName.Text = string.Concat(new string[]
					{
						order.ShipToPerson.FirstName.ToString(),
						" ",
						order.ShipToPerson.LastName.ToString(),
						" / ",
						order.ShipToCompany.Name.ToString()
					});
				}
				else
				{
					this.txtShippingName.Text = order.ShipToPerson.FirstName.ToString() + " " + order.ShipToPerson.LastName.ToString();
				}
				this.txtShippingLine1.Text = shippingAddress.Line1;
				this.txtShippingLine2.Text = shippingAddress.Line2;
				this.txtShippingCity.Text = shippingAddress.City;
				this.txtShippingState.Text = shippingAddress.StateProvince;
				this.txtShippingZip.Text = shippingAddress.PostalCode;
			}
			else
			{
				this.shipAddress.Visible = false;
				this.shipAddressHeader.Visible = false;
			}
			bool flag2 = false;
			List<WebShoppingCartItemView> list = new List<WebShoppingCartItemView>();
			foreach (OrderLine current in order.Lines)
			{
				MeetingEx meetingByProductId = this.GetMeetingByProductId(current.Product.Id);
				if (meetingByProductId != null)
				{
					list.Add(new WebShoppingCartItemView
					{
						Id = current.RequestedLineId,
						MeetingName = meetingByProductId.MeetingTitle,
						MeetingStart = meetingByProductId.StartDate,
						MeetingEnd = meetingByProductId.EndDate,
						ProductId = current.Product.Id,
						Description = current.Description,
						Price = current.Extended,
						IsMeeting = true,
						Location = (meetingByProductId.Location != null) ? string.Concat(new string[]
						{
							meetingByProductId.Location.Line1,
							" ",
							meetingByProductId.Location.Line2,
							" ",
							meetingByProductId.Location.Line3,
							" ",
							meetingByProductId.Location.Line4,
							", ",
							meetingByProductId.Location.City,
							", ",
							meetingByProductId.Location.StateProvince,
							" ",
							meetingByProductId.Location.PostalCode,
							" ",
							meetingByProductId.Location.Country
						}) : "",
						WebDescription = current.Product.WebDescription
					});
					flag2 = true;
				}
				else
				{
					list.Add(new WebShoppingCartItemView
					{
						Id = current.RequestedLineId,
						MeetingName = current.Product.WebName,
						ProductId = current.Product.Id,
						Description = current.Description,
						Price = current.Extended,
						IsMeeting = false,
						WebDescription = current.Product.WebDescription
					});
				}
			}
			if (!flag2)
			{
				this.lnkCourseCatalog.Visible = false;
			}
			this.PriceSubTotal.Text = order.SubTotal.ToString("C");
			this.PriceShipping.Text = order.ShippingTotal.ToString("C");
			this.PriceTax.Text = order.Tax.ToString("C");
			this.PriceTotal.Text = order.GrandTotal.ToString("C");
			this.PaymentTotal.Text = order.PaymentTotal.ToString("C");
			this.PriceBalance.Text = order.Balance.ToString("C");
			this.ListViewCart.DataSource = list;
			this.ListViewCart.DataBind();
		}
		public MeetingEx GetMeetingByProductId(int productId)
		{
			string text = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getbyodatasingle/";
			RestClient restClient = new RestClient(string.Concat(new object[]
			{
				text,
				"?$filter=Product/Id eq '",
				productId,
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
		public List<PaymentType> GetCards()
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/PaymentType/GetByOdata/?$filter=Active eq 'true' and Type eq 'Credit Card'";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.GET);
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				base.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri));
			}
			restRequest.AddHeader("x-aura-token", auraId);
			IRestResponse restResponse = restClient.Execute(restRequest);
			return JsonConvert.DeserializeObject<List<PaymentType>>(restResponse.Content);
		}
	}
}
