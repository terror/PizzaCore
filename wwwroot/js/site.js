/**
 * Google Maps
 */
var map;
function initMap() {
  // Coordinates for John Abbott.
  const myLatLng = { lat: 45.407082014717304, lng: -73.94177111534077 };

  // Initialize map
  const map = new google.maps.Map(document.getElementById("map"), {
    zoom: 12,
    center: myLatLng,
  });

  // Initialize marker on map
  new google.maps.Marker({
    position: myLatLng,
    map,
  });
}

/**
 * Navbar
 */
document.getElementById("english-lang").addEventListener("click", () => {
  document.getElementById("current-lang").textContent = "English";
});

document.getElementById("french-lang").addEventListener("click", () => {
  document.getElementById("current-lang").textContent = "French";
})

/**
 * ReCaptcha
 */
function enableSubmitBtn() {
  document.getElementById("submitBtn").disabled = false;
}

function resetReCaptcha() {
  grecaptcha.reset();
}

/**
 * Menu
 */
function updatePrice(target, pricesBySize) {
  const price = target.parentNode.getElementsByClassName("item-price")[0];
  const size = target.value;
  const addBtn = target.parentNode.parentNode.getElementsByClassName("item-cart-add")[0];
  
  // Update the displayed price
  pricesBySize.forEach(ps => {
    if (ps.Size == size) {
      let m = Number((Math.abs(ps.Price) * 100).toPrecision(15));
      
      price.textContent = "$" + (Math.round(m) / 100 * Math.sign(ps.Price)).toFixed(2);
      addBtn.value = ps.Id;
      return;
    }
  });
}

/**
* Checkout
*/

function onDeliveryMethodChange() {
  const deliveryMethod = document.getElementById("deliverySelect").value;
  const location = document.getElementsByClassName("order-location");
  for (let i = 0; i < location.length; ++i) {
    location[i].disabled = deliveryMethod === "Pickup";
    location[i].value = "";
  }
}

function onCheckoutGuest() {
  document.getElementById("checkout-warning").style.visibility = "visible";
}

function onCheckoutGuestCancel() {
  document.getElementById("checkout-warning").style.visibility = "hidden";
}
