using System;
namespace icpas_store.Models
{
	public class WebShoppingCartProcessRequestViewModel
	{
		public int SavedShoppingCartId
		{
			get;
			set;
		}
		public int PaymentTypeId
		{
			get;
			set;
		}
		public string PaymentSource
		{
			get;
			set;
		}
		public string CardNumber
		{
			get;
			set;
		}
		public int CardExpirationMonth
		{
			get;
			set;
		}
		public int CardExpirationYear
		{
			get;
			set;
		}
		public string CardSvn
		{
			get;
			set;
		}
        public string ReferenceTransactNum { get; set; }
        public int ReferenceExpirationMonth { get; set; }
        public int ReferenceExpirationYear { get; set; }
        public string CardPartial { get; set; }
        public string ActivePaymentType { get; set; }
        public string ACHNickname { get; set; }
        public string ACHBankRouting { get; set; }
        public string ACHAccountNumber { get; set; }
        public string ACHBankName { get; set; }
        public string ACHCheckNumber { get; set; }
        public Order Order
		{
			get;
			set;
		}
		public string PaymentIssue
		{
			get;
			set;
		}
		public int MarketingSourceId
		{
			get;
			set;
		}
	}
}
