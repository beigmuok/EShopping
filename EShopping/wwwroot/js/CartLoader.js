
	$(document).ready(function () {
			 
    var url = "/UI/CartManager/getCartCount";
	$.ajax({
		type: "POST",
	url: url,
	success: function (response) {
						if (response['success'] == true) {
							//alertify.success(response['message'][0]);

							var SetData = $("#cart");
							SetData.html("");
							if (SetData != null)
							{
								 
								var data = '<a href="/UI/CartManager/Cart" asp-area="UI" asp-controller="CartManager" asp-action="Cart" class="cart position-relative d-inline-flex"><i class="bi-cart-plus-fill"></i><span id="count" class="cart-basket d-flex align-items-center justify-content-center text-danger"> ' + response['message'][0] + ' </span></a>'
							}

							SetData.append(data);

						}
						else {
							alertify.error(response['message'][0]);
						}
					}
				})
			
        });
