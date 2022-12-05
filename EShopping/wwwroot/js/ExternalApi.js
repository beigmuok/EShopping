//Region Javascript code to pull data from api
const baseUrl = "https://fakestoreapi.com/products";
const newProduct = document.getElementById("newProducts");
const searchBar = document.getElementById("searchBar");
const btnContainer = document.querySelector("#btn-container");// to be used for pagination
var products = [];

//load products in javascript

const loadProducts = async () => {
    try {
        const res = await fetch(baseUrl);
        products = await res.json();
        console.log(products)
        displayProducts(products)
        if (!res.ok) {
            throw new Error('$(products.message) ${res.status}');
        }
    } catch (err) {
        console.log(err);
    }

}

const displayProducts = (products) => {

    const htmlString = products.map((product) => {
        return `
                    
                                        <div class="col-md-4 mt-4">
                                            <div class="card">
                                                <div class="card-header text-white bg-info">
                                                        <p class="card-text">
                                                            <h5 class="card-title">
                                                          ${product.category}
                                                                     
                                                                <a class="btn text-white float-md-end"> <i class="bi bi-pencil-square"> </i> </a>

                                                            </h5>
                                                        </p>
                                                    </div>
                                                </div>
                                                <img class="card-i" width = "100" height ="250" src="${product.image}" alt="card image cap">
                                                <div class ="card-body">
                                                    <h5 class = "card-title"> ${product.title} </h5>
                                                    <a href="#" class="btn btn-primary"> Go To Products </a>
                                                </div>
                                            </div>
                                        </div> `

    }).join('');
    newProducts.innerHTML = htmlString;

}

//adding search funtionality
searchBar.addEventListener("keyup", function (e) {
    //get input value from the searchbar
    var enteredString = e.target.value.toLowerCase() || '';
    //console.log(enteredString)
    //now we update list of products based on the enteredString
    var productsFiltered = products.filter(product => {
        return product.category.toLowerCase().includes(enteredString) ||
            product.title.toLowerCase().includes(enteredString)

    });

    displayProductsusingjquery(productsFiltered);

});


//loadProducts();
//end of java script code
//start of same code in Jquey 


var ourProducts = [];


function getProducts() {
    try {
        $(document).ready(function () {
            $.ajax({
                url: baseUrl,
                async: false,
                //The success callback function is passed the returned data, which will be an XML root element, text string, 
                //JavaScript file, or JSON object, depending on the MIME type of the response. It is also passed the text status                                                             of the response.
                success: function (response) {
                    ourProducts = response;

                    displayProductsusingjquery(response);
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }

            });

        });
        console.log("out y ")
        console.log(ourProducts)
    } catch (err) {
        console.log(err);
    }

}

//add search functionalityy with jquery
$(document).ready(function () {
    $("#searchBar").keyup(function () {
        console.log("Step 1 before filter")
        console.log(ourProducts)
        var enteredString = $("#searchBar").val().toLowerCase();
        console.log("entered value " + enteredString)

        //using grep functions 
        //var filteredProducts  = $.grep( ourProducts,function(item)
        // {
        //         if ($.trim(enteredString).length > 0) 
        //    {
        //            return (item.category.toLowerCase().indexOf(ourProducts) != -1)
        //        
        //    }
        //    else
        //    {
        //            return ourProducts;
        //    }
        // });

        //using filter function for arrays

        var filteredProducts = ourProducts.filter(item => item.category.toLowerCase().indexOf(enteredString) > -1
            || item.title.toLowerCase().indexOf(enteredString) > -1
        )
        displayProductsusingjquery(filteredProducts)

    });
});



const displayProductsusingjquery = (products) => {

    const htmlString = products.map((product) => {
        return `
                                                <div class="col-md-4 mt-4 col-sm-6 border-primary mb-3">
                                    <div class="card mb-3" style="max-width:400px;">
                                    <div class="row g-0">
                                    
                                        <div class="col-md-12">
                                            <div class="card-header text-white bg-info">
                                                <p class="card-text">
                                                    <h7 class="card-title">
                                                            ${product.category}
                                                      
                                                                                        <a class="btn text-white float-md-end"> <i class="bi bi-pencil-square"> </i> </a>

                                                                                    </h7>
                                                                                </p>
                                                                            </div>
                                                                        </div>
                                                       
                                                                        <div class="col-md-4">
                                                                                <img src="${product.image}" width="100%" alt="${product.category}">
                                                                        </div>
                                                       
                                                                        <div class="col-md-8">
                                                                            <div class="card-body">
                                                                                     
                                                                                 <p class="card-text">${product.title} </p>
                                   
                                                                                     <p class="card-text"> <b>Cost </b>${product.price} </p>                               
                                                                                     <p class="card-text"><b> Raitings: </b> ${product.rating.rate}  </p>
                                
                                
                                                                                
                               
                                                                            </div>
                                                                        </div>
                                                       
                                                                        <div class="col-md-12">
                                            <div class="card-footer">
                                 
                                                 <a href="#"" class="btn btn-success text-white" > Go To Products </a>
                                                 <a class="btn btn-outline-primary float-end"><i class="bi bi-eye-fill"></i>More Details</a>

                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>`

    }).join('');
    newProducts.innerHTML = htmlString;

}
getProducts();