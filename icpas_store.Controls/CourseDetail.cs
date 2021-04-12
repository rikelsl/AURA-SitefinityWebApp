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
	public class CourseDetail : UserControl
	{
		protected HtmlGenericControl AlertWindow;
		protected HtmlGenericControl Title;
		protected LinkButton btnRegister;
		protected LinkButton btnGroupRegister;
		protected HyperLink btnLogin;
		protected HtmlGenericControl RegistrationDetails;
		protected Image imgEMaterials;
		protected HtmlGenericControl MeetingDate;
		protected HtmlGenericControl StartDate;
		protected HtmlGenericControl spnDateDash;
		protected HtmlGenericControl EndDate;
		protected HtmlGenericControl RegistrationTime;
		protected HtmlGenericControl RegistrationOpen;
		protected HtmlGenericControl spnRegDash;
		protected HtmlGenericControl RegistrationClosed;
		protected HtmlGenericControl MeetingTime;
		protected HtmlGenericControl StartTime;
		protected HtmlGenericControl spnTimeDash;
		protected HtmlGenericControl EndTime;
		protected HtmlGenericControl hasAddress;
		protected HyperLink linkMaps;
		protected HtmlGenericControl FacilityName;
		protected HtmlGenericControl AddressLine1;
		protected HtmlGenericControl AddressLine2;
		protected HtmlGenericControl AddressOther;
		protected HtmlGenericControl AddressCity;
		protected HtmlGenericControl AddressState;
		protected HtmlGenericControl AddressZip;
		protected HtmlGenericControl noAddress;
		protected HtmlGenericControl MeetingType;
		protected HtmlGenericControl Credit;
		protected HtmlGenericControl FieldsOfStudy;
		protected HtmlGenericControl divSpecialtyCredits;
		protected HtmlGenericControl SpecialtyCredits;
		protected HtmlGenericControl Level;
		protected HtmlGenericControl PriceMember;
		protected Panel pnlNonMemPrice;
		protected HtmlGenericControl PriceNonMember;
		protected HtmlGenericControl divYourPrice;
		protected HtmlGenericControl YourPrice;
		protected Panel pnlAccPassMsg;
		protected Label lblAccPassMsg;
		protected HtmlGenericControl DiscountsAvailable;
		protected Literal litDescription;
		protected HtmlGenericControl overviewHeadline;
		protected HtmlGenericControl Overview;
		protected HtmlGenericControl highlightsHeadline;
		protected HtmlGenericControl Highlights;
		protected HtmlGenericControl speakersHeadline;
		protected HtmlGenericControl Speakers;
		protected HtmlGenericControl prerequisitesHeadline;
		protected HtmlGenericControl Prerequisites;
		protected HtmlGenericControl notesHeadline;
		protected HtmlGenericControl Notes;
		protected HtmlGenericControl onsiteHeadline;
		protected HtmlGenericControl OnSite;
		protected HtmlGenericControl divCantAttend;
		protected Literal litCantAttend;
		protected HtmlGenericControl RelatedDiv;
		protected Literal litMarketing;
		protected HtmlGenericControl regButtonsBottom;
		protected LinkButton btnRegisterBottom;
		protected LinkButton btnGroupRegisterBottom;
		protected PlaceHolder bioPlaceholder;
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.IsDesignMode())
			{
				return;
			}
			if (!this.Page.IsPostBack)
			{
				this.LoadData();
				this.CheckCartCount(0);
			}
		}
		public void CheckCartCount(int additionalCount = 0)
		{
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
			if (num + additionalCount <= count)
			{
				this.btnRegister.Visible = false;
				this.btnGroupRegister.Visible = false;
				this.regButtonsBottom.Visible = false;
				this.AlertWindow.Visible = true;
			}
		}
		public void LoadData()
		{
			this.btnLogin.NavigateUrl = "/Login?ReturnUrl=" + HttpUtility.UrlEncode(base.Request.Url.AbsoluteUri);
			string personId = Helpers.GetPersonId(Convert.ToString(base.Session["PersonID"]));
			if (!string.IsNullOrEmpty(personId))
			{
				this.btnRegister.Visible = true;
				this.btnRegisterBottom.Visible = true;
				this.btnLogin.Visible = false;
				string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
				if (Helpers.IsFirmAdminOrMember(personId, auraId))
				{
					this.btnGroupRegister.Visible = true;
					this.btnGroupRegisterBottom.Visible = true;
				}
			}
			string text = base.Request.QueryString["id"];
			if (string.IsNullOrEmpty(text))
			{
				throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
			}
			string text2 = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/meetingex/getsinglebyid";
			RestClient restClient = new RestClient(text2 + "?Id=" + text);
			RestRequest request = new RestRequest(Method.GET);
			IRestResponse restResponse = restClient.Execute(request);
			MeetingEx meetingEx = JsonConvert.DeserializeObject<MeetingEx>(restResponse.Content);
			if (meetingEx == null || meetingEx.Product == null || !meetingEx.Product.WebEnabled)
			{
				base.Response.Redirect("~/coursenotfound");
			}
			this.Title.InnerText = ((!string.IsNullOrEmpty(meetingEx.Product.Code)) ? string.Format("{0} ({1})", meetingEx.Product.WebName, meetingEx.Product.Code) : meetingEx.Product.WebName);
			this.StartDate.InnerText = meetingEx.StartDate.ToString("MM/dd/yyyy");
			this.EndDate.InnerText = meetingEx.EndDate.ToString("MM/dd/yyyy");
			this.RegistrationOpen.InnerText = meetingEx.OpenTime.ToString("h:mm tt");
			this.RegistrationClosed.InnerText = meetingEx.StartDate.ToString("h:mm tt");
			this.StartTime.InnerText = meetingEx.StartDate.ToString("h:mm tt");
			this.EndTime.InnerText = meetingEx.EndDate.ToString("h:mm tt");
			this.imgEMaterials.Visible = (meetingEx.EMaterials > 0);
			this.FieldsOfStudy.InnerText = Helpers.IfNull(meetingEx.FieldOfStudy, "Not Available");
			if (Helpers.IfNull(meetingEx.SpecialtyCredits, "") == "")
			{
				this.divSpecialtyCredits.Visible = false;
			}
			else
			{
				this.SpecialtyCredits.InnerText = meetingEx.SpecialtyCredits;
			}
			this.MeetingType.InnerText = Helpers.IfNull(meetingEx.Type.Name, "NotAvailable");
			this.FacilityName.InnerHtml = Helpers.IfNull(meetingEx.FacilityName, "Not Available");
			this.litMarketing.Text = meetingEx.CpeMarketingMeetingSideNavDescription;
			this.litDescription.Text = meetingEx.CpeMarketingMeetingDescription;
			if (!string.IsNullOrEmpty(meetingEx.RelatedWebcastInformation))
			{
				this.divCantAttend.Visible = true;
				this.litCantAttend.Text = meetingEx.RelatedWebcastInformation;
			}
			if (meetingEx.Location != null)
			{
				this.AddressLine1.InnerHtml = meetingEx.Location.Line1.Trim();
				this.AddressLine2.InnerHtml = ((!string.IsNullOrWhiteSpace(meetingEx.Location.Line2)) ? meetingEx.Location.Line2.Trim() : string.Empty);
				this.AddressCity.InnerText = meetingEx.Location.City.Trim();
				this.AddressState.InnerText = meetingEx.Location.StateProvince.Trim();
				this.AddressZip.InnerText = meetingEx.Location.PostalCode.Trim();
				string str;
				if (!string.IsNullOrWhiteSpace(meetingEx.Location.Line2))
				{
					str = meetingEx.Location.Line1.Trim() + " " + meetingEx.Location.Line2.Trim();
				}
				else
				{
					str = meetingEx.Location.Line1.Trim();
				}
				this.linkMaps.NavigateUrl = string.Format("http://maps.google.com/maps?daddr={0} {1} {2} {3}", new object[]
				{
					HttpUtility.UrlEncode(str),
					meetingEx.Location.City.Trim(),
					meetingEx.Location.StateProvince.Trim(),
					meetingEx.Location.PostalCode.Trim()
				});
			}
			else
			{
				this.FacilityName.InnerText = "No Location Available";
				this.AddressLine1.Visible = false;
				this.AddressLine2.Visible = false;
				this.AddressOther.Visible = false;
				this.linkMaps.Enabled = false;
			}
			if (meetingEx.Product.Category.Id == 6)
			{
				this.FacilityName.Visible = false;
				this.AddressLine1.Visible = false;
				this.AddressLine2.Visible = false;
				this.AddressOther.Visible = false;
				this.linkMaps.Enabled = false;
				this.noAddress.Visible = true;
				this.noAddress.InnerText = "Webinar";
			}
			if (meetingEx.Product.Category.Id == 34)
			{
				this.FacilityName.Visible = false;
				this.AddressLine1.Visible = false;
				this.AddressLine2.Visible = false;
				this.AddressOther.Visible = false;
				this.linkMaps.Enabled = false;
				this.MeetingTime.Visible = false;
				this.noAddress.Visible = true;
				this.noAddress.InnerText = "On-Demand";
			}
			if (meetingEx.StartDate.ToShortDateString() == meetingEx.EndDate.ToShortDateString())
			{
				this.EndDate.InnerText = "";
				this.spnDateDash.InnerText = "";
			}
			if (meetingEx.Type.Name.Contains("OnDemand"))
			{
				this.StartDate.InnerText = "onDemand";
				this.EndDate.InnerText = "";
				this.RegistrationOpen.InnerText = "onDemand";
				this.RegistrationClosed.InnerText = "";
				this.StartTime.InnerText = "onDemand";
				this.EndTime.InnerText = "";
				this.spnTimeDash.InnerText = "";
				this.spnRegDash.InnerText = "";
				this.spnDateDash.InnerText = "";
			}
			this.Level.InnerText = Helpers.IfNull(meetingEx.Level, "Not Available");
			this.Credit.InnerHtml = meetingEx.TotalCredits.ToString("F");
			this.PriceMember.InnerText = "Member: " + meetingEx.Product.Prices.First((ProductPrice q) => q.MemberType.Name == "Member").Price.ToString("C");
			this.PriceNonMember.InnerText = "Nonmember: " + meetingEx.Product.Prices.First((ProductPrice q) => q.MemberType.Name == "Non-Member").Price.ToString("C");
			decimal d = 0m;
			bool flag = false;
			if (base.Session["PersonID"] != null)
			{
				d = Helpers.GetProductPrice(meetingEx.Product.Id.ToString(), base.Session["PersonID"].ToString());
				flag = true;
				this.YourPrice.InnerText = d.ToString("C");
			}
			else
			{
				this.divYourPrice.Visible = false;
			}
			if (flag && d > 0m)
			{
				this.GetEventAccessPassResult();
			}
			text2 = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/productex/GetProductDiscountsById";
			restClient = new RestClient(text2 + "?productId=" + meetingEx.Product.Id);
			restResponse = restClient.Execute(request);
			List<ProductDiscountViewModel> list = JsonConvert.DeserializeObject<List<ProductDiscountViewModel>>(restResponse.Content);
			bool flag2 = true;
			string text3 = string.Empty;
			if (list != null && list.Any<ProductDiscountViewModel>())
			{
				foreach (ProductDiscountViewModel current in list)
				{
					if (flag2)
					{
						text3 = current.Name;
						flag2 = false;
					}
					else
					{
						text3 = text3 + "\n" + current.Name;
					}
				}
				this.DiscountsAvailable.InnerText = Helpers.IfNull(text3, "Not Available");
			}
			else
			{
				this.DiscountsAvailable.InnerText = Helpers.IfNull(text3, "Not Available");
			}
			bool flag3 = false;
			if (meetingEx.Speakers != null)
			{
				foreach (MeetingSpeakerEx current2 in meetingEx.Speakers)
				{
					if (current2.Status != null & current2.Status.TrimEnd(new char[0]) == "Accepted")
					{
						HtmlGenericControl htmlGenericControl = new HtmlGenericControl("div")
						{
							ID = "Bio_" + current2.Id,
							InnerHtml = current2.Speaker.CpaSpeakerBio ?? string.Empty,
							ClientIDMode = ClientIDMode.Static
						};
						htmlGenericControl.Attributes["class"] = "modal-body";
						this.bioPlaceholder.Controls.Add(htmlGenericControl);
						string text4 = string.Format("<a href='#' data-toggle='modal' data-target='#bio-modal' onClick='setbio(\"#{2}\")'>{0} {1}</a>", current2.Speaker.FirstName, current2.Speaker.LastName, htmlGenericControl.ClientID);
						if (current2.Speaker.Company != null)
						{
							text4 = text4 + ", " + current2.Speaker.Company.Name;
						}
						HtmlGenericControl child = new HtmlGenericControl("li")
						{
							ID = "Speaker_" + current2.Id,
							InnerHtml = text4
						};
						this.Speakers.Controls.Add(child);
						flag3 = true;
					}
				}
			}
			if (meetingEx.Objectives == null)
			{
				this.Overview.Visible = false;
				this.overviewHeadline.Visible = false;
			}
			if (meetingEx.Speakers == null || meetingEx.Speakers.Count == 0 || !flag3)
			{
				this.Speakers.Visible = false;
				this.speakersHeadline.Visible = false;
			}
			if (meetingEx.AdditionalInformation == null)
			{
				this.Notes.Visible = false;
				this.notesHeadline.Visible = false;
			}
			if (meetingEx.Objectives == null)
			{
				this.Overview.Visible = false;
				this.overviewHeadline.Visible = false;
			}
			if (meetingEx.Prerequisites == null)
			{
				this.Prerequisites.Visible = false;
				this.prerequisitesHeadline.Visible = false;
			}
			if (meetingEx.Summary == null)
			{
				this.Highlights.Visible = false;
				this.highlightsHeadline.Visible = false;
			}
			if (meetingEx.OnsiteDescription == null)
			{
				this.OnSite.Visible = false;
				this.onsiteHeadline.Visible = false;
			}
			this.Notes.InnerHtml = (meetingEx.AdditionalInformation ?? "No Additional Information");
			this.Overview.InnerHtml = (meetingEx.Objectives ?? "No Information Available");
			this.Prerequisites.InnerHtml = (meetingEx.Prerequisites ?? "No Information Available");
			this.Highlights.InnerHtml = (meetingEx.Summary ?? "No Information Available");
			this.OnSite.InnerHtml = (meetingEx.OnsiteDescription ?? "No Information Available");
			this.GetRelated(meetingEx.Product.Id);
			if ((meetingEx.MaxRegistrants <= 0 || meetingEx.AvailSpace > 0) && meetingEx.Product.WebEnabled && !meetingEx.NoRegistration && meetingEx.Product.IsSold)
			{
				if (!(meetingEx.Product.AvailableUntil < DateTime.Today))
				{
					goto IL_D22;
				}
				DateTime arg_CD5_0 = meetingEx.Product.AvailableUntil;
				if (meetingEx.Product.AvailableUntil.Year <= 1900)
				{
					goto IL_D22;
				}
			}
			this.btnRegister.Visible = false;
			this.btnLogin.Visible = false;
			this.btnGroupRegister.Visible = false;
			this.btnRegisterBottom.Visible = false;
			IL_D22:
			if (meetingEx.NoRegistration)
			{
				this.RegistrationDetails.Visible = true;
			}
			base.Session["ProductId"] = meetingEx.Product.Id;
		}
		public void GetRelated(int productId)
		{
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/MeetingEx/getrelatedmeetingsbyproductid?productId=" + productId;
			RestClient restClient = new RestClient(baseUrl);
			RestRequest request = new RestRequest(Method.GET);
			IRestResponse restResponse = restClient.Execute(request);
			List<MeetingEx> list = JsonConvert.DeserializeObject<List<MeetingEx>>(restResponse.Content);
			if (list == null || list.Count == 0)
			{
				return;
			}
			this.RelatedDiv.InnerHtml = string.Empty;
			foreach (MeetingEx current in list)
			{
				HtmlGenericControl expr_7B = this.RelatedDiv;
				expr_7B.InnerHtml += string.Format("<p><a href='/EventsCalendar/EventDetail?id={0}'>{1}</a></p>", current.Id, current.Product.WebName);
			}
		}
		private void GetEventAccessPassResult()
		{
			string auraId = Helpers.GetAuraId(Convert.ToString(base.Session["AptifyUniqueId"]));
			if (string.IsNullOrEmpty(auraId))
			{
				return;
			}
			string meetingId = base.Request.QueryString["id"];
			int id = Helpers.GetMeetingByMeetingId(meetingId).Product.Id;
			WebShoppingCartEx webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/GetEventAccessPassResult";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.GET);
			restRequest.AddParameter("lgAttendeeID", Convert.ToInt64(base.Session["PersonID"]));
			restRequest.AddParameter("lgProductID", Convert.ToInt64(id));
			if (webShoppingCartEx != null)
			{
				restRequest.AddParameter("cartId", webShoppingCartEx.Id);
			}
			else
			{
				restRequest.AddParameter("cartId", null);
			}
			IRestResponse restResponse = restClient.Execute(restRequest);
			string text = JsonConvert.DeserializeObject<string>(restResponse.Content);
			if (Helpers.TrimOrNull(text) != "" && !Helpers.TrimOrNull(text).Contains("Error occurred"))
			{
				this.pnlAccPassMsg.Visible = true;
				this.lblAccPassMsg.Text = "Because you are a <span style='font-style: italic;'>" + text.Trim() + "</span> holder, your price will be discounted when you register.";
				return;
			}
			this.pnlAccPassMsg.Visible = false;
		}
		protected void btnGroupRegister_OnClick(object sender, EventArgs e)
		{
			string text = base.Session["ProductId"].ToString();
			base.Session["ProductId"] = null;
			if (string.IsNullOrEmpty(text))
			{
				throw new HttpException(400, "Bad Request, Product Id Missing");
			}
			base.Response.Redirect("/Connections/Communities-(Member-Segment)/FirmAdminPortal/GroupRegistration?id=" + text);
		}
		protected void btnRegister_OnClick(object sender, EventArgs e)
		{
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
			string baseUrl = ConfigurationManager.AppSettings["ServicesUrl"] + "/icpas/api/WebShoppingCartEx/AddShoppingCartLine";
			RestClient restClient = new RestClient(baseUrl);
			RestRequest restRequest = new RestRequest(Method.PUT);
			string text = base.Request.QueryString["id"];
			int id = Helpers.GetMeetingByMeetingId(text).Product.Id;
			if (string.IsNullOrEmpty(text))
			{
				throw new HttpException(400, "Bad Request, QueryString Parameter Missing.");
			}
			restRequest.AddHeader("x-aura-token", auraId);
			int num;
			int registrantId = int.TryParse(Convert.ToString(base.Session["PersonID"]), out num) ? num : 107962;
			restRequest.RequestFormat = DataFormat.Json;
			webShoppingCartEx.Lines = new List<WebShoppingCartDetails>
			{
				new WebShoppingCartDetails
				{
					ProductId = Convert.ToInt32(id),
					RegistrantId = registrantId
				}
			};
			restRequest.AddBody(webShoppingCartEx);
			restClient.Execute(restRequest);
			decimal d;
			bool flag = decimal.TryParse(this.PriceMember.InnerText.Replace("$", ""), out d);
			if (flag && d > 0m)
			{
				this.GetEventAccessPassResult();
			}
			webShoppingCartEx = Helpers.GetCart(auraId).FirstOrDefault<WebShoppingCartEx>();
			base.Response.Redirect("/CPE/MeetingRegistration?OL=" + webShoppingCartEx.Lines[webShoppingCartEx.Lines.Count - 1].Id);
		}
	}
}
