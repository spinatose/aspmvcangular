
$(document).ready(function () {
    //let myForm = $("#myform");
    let btnBuy = $("#btnBuy");
    let prodInfo = $(".product-props li");

    //myForm.hide();
    btnBuy.on("click", function () {
        console.log("Buying item...");
    });

    prodInfo.on("click", function () {
        console.log("you clicked: " + $(this).text());
    });

    let loginToggle = $("#loginToggle");
    let popupForm = $(".popup-form");

    loginToggle.on("click", () =>
    {
        popupForm.fadeToggle(500);
    });
});