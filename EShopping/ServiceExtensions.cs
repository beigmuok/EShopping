using EShopping.Services;

namespace EShopping
{
	public class ServiceExtensions
	{
		public static void setUpScopedServices(WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICartManager, CartManager>();

		}
	}
}
