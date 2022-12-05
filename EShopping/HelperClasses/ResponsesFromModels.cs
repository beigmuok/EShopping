namespace EShopping.HelperClasses
{
	public class ResponsesFromModels
	{
     

        public ResponsesFromModels(int res, String sms)
		{
			 responseCode = res;	
			 responseMessage=sms;

		}
		public int responseCode { get; set; }
		public String? responseMessage { get;set; }
	}
}
