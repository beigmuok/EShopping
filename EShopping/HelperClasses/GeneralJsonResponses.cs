using System.Text.Json.Nodes;

namespace EShopping.HelperClasses
{
	public class GeneralJsonResponses
	{
		public bool success { get; set; }
		public List<String> Message { get; set; }
		public JsonObject Result { get; set; }

		public int StatusCode { get; set; }

		
	}
}
